# Unity 3D Learning

> HW3 牧师与魔鬼 动作分离版

在线演示：[demo.jiahonzheng.cn/Priests-and-Devils-V2/](https://demo.jiahonzheng.cn/Priests-and-Devils-V2/) 。

演示视频：[www.bilibili.com/video/av70059452](https://www.bilibili.com/video/av70059452) 。

博客链接：[游戏对象与图形基础](https://blog.jiahonzheng.cn/2019/10/04/%E6%B8%B8%E6%88%8F%E5%AF%B9%E8%B1%A1%E4%B8%8E%E5%9B%BE%E5%BD%A2%E5%9F%BA%E7%A1%80/) 。

基于面向对象设计的思考，我们使用专用的对象管理游戏对象的运动。因为游戏对象执行的动作，可划分为多个基础动作，我们对其进行提炼，抽离成公共动作，最后使用动作管理者调度游戏对象之间的动作。

我们在原有的《牧师与魔鬼》的基础上，**实现动作分离的职责划分**。同时，我们还**实现了裁判类**，用于当游戏到达结束条件时，通知场景控制器游戏结束。

## 动作基类

我们先构建动作基类，它需要用户实现两个方法 `Start` 和 `Update` ，分别用于初始化动作和实现动作逻辑。除此之外，我们在动作执行完毕后，需要通知更高级的对象“动作已执行完毕”，因此我们还需要设置 `IActionCallback` 。

```csharp
namespace PriestsAndDevils
{
  	// 动作基类。
    public class Action : ScriptableObject
    {
        public bool enable = true;
        // 若为 true ，表示动作已完成。
        public bool destroy = false;
        // 表示需要进行运动的游戏对象。
        public GameObject gameObject { get; set; }
        public Transform transform { get; set; }
        // 表示在动作执行完毕后，需要通知的对象。
        public IActionCallback callback;

				// 在此方法中实现动作的初始化操作。
        public virtual void Start()
        {
            // 提示用户需要实现此方法！
            throw new System.NotImplementedException();
        }

				// 在此方法中实现动作逻辑。
        public virtual void Update()
        {
            // 提示用户需要实现此方法！
            throw new System.NotImplementedException();
        }
    }
}
```

接口 `IActionCall` 的代码如下，其中的 `ActionDone` 用于通知更高级的对象动作已执行完毕。

```csharp
public interface IActionCallback
{
  	// 用于通知更高级对象动作已执行完毕。
    void ActionDone(Action action);
}
```

## 基础动作

### 直线运动

在动作基类的基础上，我们根据牧师与魔鬼的需求，实现了直线运动 `MoveToAction` 。我们在静态方法中，实现了 `MoveToAction` 的创建；在 `Update` 方法中，使用 `Vector3.MoveTowards` 实现直线运动的逻辑。由于直线运动无需初始化操作，故 `Start` 函数体为空。

```csharp
public class MoveToAction : Action
{
    // 表示运动目的地。
    public Vector3 destination;
    // 表示运动速度。
    public float speed;

    // 创建 MoveToAction 。
    public static MoveToAction GetAction(GameObject gameObject, IActionCallback callback, Vector3 destination, float speed)
    {
        MoveToAction action = CreateInstance<MoveToAction>();
        // 设置需要进行直线运动的游戏对象。
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = callback;
        // 设置直线运动的终点。
        action.destination = destination;
        // 设置直线运动的速度。
        action.speed = speed;
        return action;
    }

    public override void Start() { }

    // 在此方法中实现直线运动的逻辑。
    public override void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        if (transform.position == destination)
        {
            destroy = true;
            callback?.ActionDone(this);
        }
    }
}
```

### 序列运动

由于游戏对象可能需要依次进行多个动作，例如牧师和魔鬼在上下船时的折线式运动可分解为**横向的直线运动**和**纵向的直线运动**，故我们需要实现一个动作队列，**组合各种子动作为一个动作**。在实现中，我们的动作队列是由 `SequenceAction` 实现的。

```csharp
public class SequenceAction : Action, IActionCallback
{
    // 用于存储多个顺序执行的动作。
    public List<Action> sequence;
    // 指明动作执行次数，若为负数则表示该动作重复执行。
    public int repeat = 1;
    // 表示当前进行的动作。
    public int currentActionIndex = 0;

    // 创建 SequenceAction 。
    public static SequenceAction GetAction(IActionCallback callback, List<Action> sequence, int repeat = 1, int currentActionIndex = 0)
    {
        SequenceAction action = CreateInstance<SequenceAction>();
        action.callback = callback;
        action.sequence = sequence;
        action.repeat = repeat;
        action.currentActionIndex = currentActionIndex;
        return action;
    }

    // 设置每个子动作的 callback ，使得子动作完成时，SequenceAction 可切换至下一动作。
    public override void Start()
    {
        foreach (Action action in sequence)
        {
            action.callback = this;
            action.Start();
        }
    }

    // 执行子动作。
    public override void Update()
    {
        if (sequence.Count == 0)
        {
            return;
        }
        if (currentActionIndex < sequence.Count)
        {
            sequence[currentActionIndex].Update();
        }
    }

    // 子动作完成时的钩子函数，用于切换下一子动作。
    public void ActionDone(Action action)
    {
        action.destroy = false;
        currentActionIndex++;
        if (currentActionIndex >= sequence.Count)
        {
            currentActionIndex = 0;
            // 判断是否需要重复执行。
            if (repeat > 0)
            {
                repeat--;
            }
            if (repeat == 0)
            {
                destroy = true;
                callback?.ActionDone(this);
            }
        }
    }

    // 响应 Object 被销毁的事件。
    void OnDestroy()
    {
        foreach (Action action in sequence)
        {
            Destroy(action);
        }
    }
}
```

## 动作管理者

在动作管理者中，我们使用 `Dictionary<int, Action>` 管理所有动作，对其进行调度。为了抽离公用的管理代码，我们实现了动作管理者基类 `ActionManager` 。

```csharp
public class ActionManager : MonoBehaviour, IActionCallback
{
    // 存储所有动作。
    private Dictionary<int, Action> actions = new Dictionary<int, Action>();
    private List<Action> waitToAdd = new List<Action>();
    private List<int> waitToDelete = new List<int>();

    protected void Update()
    {
        foreach (Action action in waitToAdd)
        {
            actions[action.GetInstanceID()] = action;
        }
        waitToAdd.Clear();
        // 执行每一个动作。
        foreach (KeyValuePair<int, Action> kv in actions)
        {
            Action action = kv.Value;
            if (action.destroy)
            {
                waitToDelete.Add(action.GetInstanceID());
            }
            else if (action.enable)
            {
                action.Update();
            }
        }
        // 删除已完成的动作对应的数据结构。
        foreach (int k in waitToDelete)
        {
            Action action = actions[k];
            actions.Remove(k);
            Destroy(action);
        }
        waitToDelete.Clear();
    }

    // 添加动作。
    public void AddAction(Action action)
    {
        waitToAdd.Add(action);
        action.Start();
    }

		// ActionDone 这里我们设置其函数体为空。
    public void ActionDone(Action action) { }
  }
```

在《牧师与魔鬼》游戏中，我们能进行动作的游戏对象有：船只、人物（牧师、魔鬼）。我们在 `ActionManager` 的基础上，实现了其子类 `GameActionManager` ，其含有**控制船只运动**和**控制人物运动**的方法。

```csharp
public class GameActionManager : ActionManager
{
    // 移动船只。
    public void MoveBoat(BoatController boat)
    {
        // 创建船的直线运动。
        MoveToAction action = MoveToAction.GetAction(boat.gameObject, this, boat.GetDestination(), 20);
        AddAction(action);
    }

    // 移动人物。
    public void MoveCharacter(CharacterController character)
    {
        Vector3 destination = character.GetDestination();
        GameObject gameObject = character.gameObject;
        Vector3 currentPosition = character.transform.position;
        //横向直线运动和纵向直线运动的转折点。
        Vector3 pivotPosition = currentPosition;
        if (destination.y > currentPosition.y)
        {
            pivotPosition.y = destination.y;
        }
        else
        {
            pivotPosition.x = destination.x;
        }
        // 创建序列动作来表示人物的折线运动：横向的直线运动、纵向的直线运动。
        Action action1 = MoveToAction.GetAction(gameObject, null, pivotPosition, 20);
        Action action2 = MoveToAction.GetAction(gameObject, null, destination, 20);
        SequenceAction action = SequenceAction.GetAction(this, new List<Action> { action1, action2 });
        AddAction(action);
    }
}
```

在引入了动作管理者，我们的代码变得“清爽很多”，例如在点击船只，使其移动时，现在的实现代码是这样的。

```csharp
// It is called when player clicks the boat.
public void ClickBoat()
{
    if (boat.model.IsEmpty())
    {
        return;
    }
    // Update the model.
    boat.Move();
    // Update the view.
  	// 使用动作管理者调度动作。
    actionManager.MoveBoat(boat);
    game.CheckWinner();
}
```

## 裁判类

我们前面提到，我们基于职责划分的考虑，设计了裁判类来判定游戏胜负。首先，我们需要枚举类型 `Result` 表示游戏胜负情况。`Game` 即为裁判类，它通过传入的 `Boat` 、`Coast` 控制器来初始化，对外提供了 `CheckWinner` 方法用于判定游戏胜负情况，使用了 `EventHandler` 来通知场景控制器游戏胜负情况。

```csharp
// It represents the result of the game.
public enum Result
{
    NOT_FINISHED,
    WINNER,
    LOSER,
}

public class Game
{
    // 表示游戏结果。
    public Result result = Result.NOT_FINISHED;
    private Boat boat;
    private Coast leftCoast;
    private Coast rightCoast;
    // 用于通知场景控制器游戏的胜负。
    public event EventHandler onChange;
    // 根据传入的控制器生成裁判类。
    public Game(Boat boat, Coast leftCoast, Coast rightCoast)
    {
        this.boat = boat;
        this.leftCoast = leftCoast;
        this.rightCoast = rightCoast;
    }

    // It determines whether the player wins the game.
    public void CheckWinner()
    {
        result = Result.NOT_FINISHED;
        // Calculate the amount.
        int leftPriests = leftCoast.GetCharacterAmount()[0];
        int leftDevils = leftCoast.GetCharacterAmount()[1];
        int rightPriests = rightCoast.GetCharacterAmount()[0];
        int rightDevils = rightCoast.GetCharacterAmount()[1];
        // When all the characters has gone across the river.
        if (leftPriests + leftDevils == 6)
        {
            result = Result.WINNER;
        }
        // When the boat is on the right side.
        if (boat.location == Location.Right)
        {
            rightPriests += boat.GetCharacterAmount()[0];
            rightDevils += boat.GetCharacterAmount()[1];
        }
        else // When the boat is on the left side.
        {
            leftPriests += boat.GetCharacterAmount()[0];
            leftDevils += boat.GetCharacterAmount()[1];
        }
        // In this case, player lose the game.
        if ((rightPriests < rightDevils && rightPriests > 0) ||
            (leftPriests < leftDevils && leftPriests > 0))
        {
            result = Result.LOSER;
        }
        // 通知场景控制器。
        onChange?.Invoke(this, EventArgs.Empty);
    }
}
```

在实现完裁判类后，我们需要在 `GameController` 中的 `Awake` 函数对裁判进行初始化。注意到，在下面的代码中，我们使用 `delegate` 关键字完成了裁判和场景控制器的交互。

```csharp
void Awake()
{
    // Set the current scene controller and load resources.
    Director.GetInstance().OnSceneWake(this);
    // Add GUI.
    gui = gameObject.AddComponent<GameGUI>() as GameGUI;
    // Initialize the action manager.
    actionManager = gameObject.AddComponent<GameActionManager>();
    // Initialize the game model.
    // 初始化裁判类。
    game = new Game(boat.model, leftCoast.model, rightCoast.model);
    // 裁判类通知场景控制器游戏胜负。
    game.onChange += delegate
    {
        gui.result = game.result;
    };
}
```

## 游戏截图

为了美观，我们为游戏添加了 Skybox 、Terrain 以及 Water Material，其运行界面如下。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%B8%B8%E6%88%8F%E5%AF%B9%E8%B1%A1%E4%B8%8E%E5%9B%BE%E5%BD%A2%E5%9F%BA%E7%A1%80_1.jpg)
