# 游戏智能

游戏智能，可理解为：在游戏规则约束下，通过适当的**算法**使得游戏中 NPC（Non-Player Character） 呈现为具有一定人类智能行为的**博弈对手**，让游戏玩家面临不间断的**挑战**，并在挑战中有所收获，包括知识和技能等。

博客链接：[blog.jiahonzheng.cn/2019/12/06/游戏智能/](https://blog.jiahonzheng.cn/2019/12/06/%E6%B8%B8%E6%88%8F%E6%99%BA%E8%83%BD)

GitHub：[github.com/Jiahonzheng/Unity-3D-Learning](https://github.com/Jiahonzheng/Unity-3D-Learning/tree/HW9/HW9)

## 游戏状态

我们将**左右两岸的牧师、魔鬼的数量**以及**船的位置**看作为**游戏状态**。我们在游戏实现中，实现了 `AIState` 类，用于维护游戏状态，具体定义代码如下。

```csharp
public class AIState
{
    // 指明左岸的牧师数量。
    public int leftPriests;
    // 指明左岸的魔鬼数量。
    public int leftDevils;
    // 指明右岸的牧师数量。
    public int rightPriests;
    // 指明右岸的魔鬼数量。
    public int rightDevils;
    // 指明船的位置：true 表示在左岸；false 表示在右岸。
    public bool location;
    // 指明父状态。
    public AIState parent;
}
```

### 游戏初始状态

在初始状态中，左岸的牧师和魔鬼数量均为 0 ，右岸的牧师和魔鬼数量均为 3 ，船位于右岸。

```csharp
// 指明游戏初始的状态。
public static AIState startState = new AIState(0, 0, 3, 3, false, null);
```

### 游戏结束状态

在结束状态中，左岸的牧师和魔鬼数量均为 3 ，右岸的牧师和魔鬼数量均为 0 ，船位于左岸。

```csharp
// 指明游戏终止的状态。
public static AIState endState = new AIState(3, 3, 0, 0, true, null);
```

### 相关函数

我们在 `AIState` 类中对外提供了一个 `Valid` 方法，用于**判断当前状态是否是可行的**。

```csharp
// 判断当前状态是否可行。
public bool Valid()
{
    return ((leftPriests == 0 || leftPriests >= leftDevils) &&
        (rightPriests == 0 || rightPriests >= rightDevils));
}
```

我们对 `AIState` 类的 `Equals` 和 `GetHashCode` 方法进行了**重写**。在重写 `Equals` 方法中，我们先判断 `obj` 是否为空，随后判断二者类型是否相同，最后判断各成员是否相同。当我们重写 `Equals` 方法时，也必须同时重写 `GetHashCode` 方法。在实现中，我们使用各成员的 `HashCode` 的累加和作为 `AIState` 的 `HashCode` 。

```csharp
// 重写 Equals 函数。
public override bool Equals(object obj)
{
    // 判断 obj 是否为空。
    if (obj == null)
    {
        return false;
    }
    // 判断类型是否相同。
    if (obj.GetType().Equals(this.GetType()) == false)
    {
        return false;
    }
    // 判断成员是否相等。
    AIState temp = (AIState)obj;
    return leftPriests.Equals(temp.leftPriests) &&
        leftDevils.Equals(temp.leftDevils) &&
        rightDevils.Equals(temp.rightDevils) &&
        rightPriests.Equals(temp.rightPriests) &&
        location.Equals(temp.location);
}

// 配合 Equals 函数，重写 GetHashCode 函数。
public override int GetHashCode()
{
    // 我们使用各成员的 HashCode 的累加和作为 AIState 的 HashCode 。
    return leftDevils.GetHashCode() + leftPriests.GetHashCode() +
        rightDevils.GetHashCode() + rightPriests.GetHashCode() +
        location.GetHashCode();
}
```

此外，我们对 `AIState` 进行了操作符 `==` 和 `!=` 的重载。在判断对象是否为空时，我们不能使用 `obj == null` 的判断方法，而是应该使用 `ReferenceEquals` 的方法，否则会出现**由于无限递归导致的栈溢出**的问题。

```csharp
// 重载 == 操作符。
public static bool operator ==(AIState lhs, AIState rhs)
{
    if (AIState.ReferenceEquals(lhs, rhs))
    {
        return true;
    }
    if (AIState.ReferenceEquals(lhs, null))
    {
        return false;
    }
    return lhs.Equals(rhs);
}

// 重载 != 操作符。
public static bool operator !=(AIState lhs, AIState rhs)
{
    return !(lhs == rhs);
}
```

## 状态转换

在游戏中，**状态与状态之间是可以相互转换的**：我们通过**移动人物**完成状态转换。

+ **在移动人物时，需要考虑船只的位置**，因为不同的船只位置决定着不同的人物过河策略。
+ 移动人物，在数量上存在两种移动方式：**单人过河**和**双人过河**。
+ 在**单人过河**中，在人物选择上存在两种过河方式：**牧师过河**和**魔鬼过河**。
+ 在**双人过河**中，在人物选择上存在三种过河方式：**两牧师过河**、**两魔鬼过河**、**牧师和魔鬼过河**。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%B8%B8%E6%88%8F%E6%99%BA%E8%83%BD_1.png)

## 计算策略

我们使用 `AI` 类实现下一步策略的计算，具体成员如下。

```csharp
public class AI
{
    // 指明游戏初始的状态。
    public static AIState startState = new AIState(0, 0, 3, 3, false, null);
    // 指明游戏终止的状态。
    public static AIState endState = new AIState(3, 3, 0, 0, true, null);
    // 指明当前状态。
    private AIState currentState;
}
```

根据上述的状态转化逻辑，我们在 `AI` 类中实现了 `Hint` 方法，用于**计算下一步策略**。在策略的搜索中，我们使用了**广度优先搜索**：

1. 当前状态进入队列。
2. 队列不为空：
   1. 队头出队。
   2. 访问状态节点。当状态节点为结束状态时，回溯至开始状态；否则计算下一个合法状态并压入队列。

以下即为 `Hint` 函数的具体实现代码。

```csharp
// 计算下一步。
public AIState Hint()
{
    // 使用 BFS 广度搜索寻找可行解。
    Queue<AIState> found = new Queue<AIState>();
    Queue<AIState> visited = new Queue<AIState>();
    AIState temp = new AIState(currentState.leftPriests, currentState.leftDevils, currentState.rightPriests, currentState.rightDevils, currentState.location, null);
  	// 当前状态进入队列。
    found.Enqueue(temp);

    while (found.Count > 0)
    {
      	// 队头出队。
        temp = found.Peek();
				
      	// 当状态为结束状态时，回溯至开始状态，并返回；否则计算下一个合法状态，并压入队列。
        if (temp == endState)
        {
            while (temp != null && temp.parent != currentState)
            {
                temp = temp.parent;
            }
          	// 避免返回空对象。
            return temp == null ? endState : temp;
        }
				
        found.Dequeue();
        visited.Enqueue(temp);
      
      	// 计算下一个合法状态，并压入队列。

        // 当船在左岸时。
        if (temp.location)
        {
            // 尝试移动左岸的 1 个牧师至右岸。
            if (temp.leftPriests > 0)
            {
                AIState next = new AIState(temp);
                next.parent = new AIState(temp);
                next.location = false;
                next.leftPriests--;
                next.rightPriests++;
                // 尝试移动。
                if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                {
                    found.Enqueue(next);
                }
            }
            // 尝试移动左岸的 1 个魔鬼至右岸。
            if (temp.leftDevils > 0)
            {
                AIState next = new AIState(temp);
                next.parent = new AIState(temp);
                next.location = false;
                next.leftDevils--;
                next.rightDevils++;
                // 尝试移动。
                if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                {
                    found.Enqueue(next);
                }
            }
            // 尝试移动左岸的 2 个牧师至右岸。
            if (temp.leftPriests > 1)
            {
                AIState next = new AIState(temp);
                next.parent = new AIState(temp);
                next.location = false;
                next.leftPriests -= 2;
                next.rightPriests += 2;
                next.parent = new AIState(temp);
                if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                {
                    found.Enqueue(next);
                }
            }
            // 尝试移动左岸的 2 个魔鬼至右岸。
            if (temp.leftDevils > 1)
            {
                AIState next = new AIState(temp);
                next.parent = new AIState(temp);
                next.location = false;
                next.leftDevils -= 2;
                next.rightDevils += 2;
                if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                {
                    found.Enqueue(next);
                }
            }
            // 尝试移动左岸的 1 个牧师和 1 个魔鬼至右岸。
            if (temp.leftPriests > 0 && temp.leftDevils > 0)
            {
                AIState next = new AIState(temp);
                next.parent = new AIState(temp);
                next.location = false;
                next.leftPriests--;
                next.leftDevils--;
                next.rightPriests++;
                next.rightDevils++;
                if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                {
                    found.Enqueue(next);
                }
            }
        }
        else
        {
            // 尝试移动右岸的 1 个牧师至左岸。
            if (temp.rightPriests > 0)
            {
                AIState next = new AIState(temp);
                next.parent = new AIState(temp);
                next.location = true;
                next.rightPriests--;
                next.leftPriests++;
                if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                {
                    found.Enqueue(next);
                }
            }
            // 尝试移动左岸的 1 个魔鬼至右岸。
            if (temp.rightDevils > 0)
            {
                AIState next = new AIState(temp);
                next.parent = new AIState(temp);
                next.location = true;
                next.rightDevils--;
                next.leftDevils++;
                if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                {
                    found.Enqueue(next);
                }
            }
            // 尝试移动右岸的 2 个牧师至左岸。
            if (temp.rightDevils > 1)
            {
                AIState next = new AIState(temp);
                next.parent = new AIState(temp);
                next.location = true;
                next.rightDevils -= 2;
                next.leftDevils += 2;
                if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                {
                    found.Enqueue(next);
                }
            }
            // 尝试移动右岸的 2 个牧师至左岸。
            if (temp.rightPriests > 1)
            {
                AIState next = new AIState(temp);
                next.parent = new AIState(temp);
                next.location = true;
                next.rightPriests -= 2;
                next.leftPriests += 2;
                if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                {
                    found.Enqueue(next);
                }
            }
            // 尝试移动右岸的 1 个牧师和 1 个魔鬼至左岸。
            if (temp.rightPriests > 0 && temp.rightDevils > 0)
            {
                AIState next = new AIState(temp);
                next.parent = new AIState(temp);
                next.location = true;
                next.rightPriests--;
                next.rightDevils--;
                next.leftPriests++;
                next.leftDevils++;
                if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                {
                    found.Enqueue(next);
                }
            }
        }
    }
  	// 避免返回空对象。
    return endState;
}
```

## 原有代码改造

由于引入了**游戏智能**模块，我们需要对现有模块进行功能扩充。

### State

首先，我们需要在 `Game` 类中，增加游戏状态的维护逻辑。

```csharp
public class Game
{
    // 表示游戏结果。
    public Result result = Result.NOT_FINISHED;
    private Boat boat;
    private Coast leftCoast;
    private Coast rightCoast;
  	// 表示当前的游戏状态。
    private State state;
    // 用于通知场景控制器游戏的胜负。
    public event EventHandler onChange;
}
```

上述代码的 `State` 类，用于存储当前游戏的状态（左右两岸的牧师和魔鬼数量、船只的位置），具体定义代码如下。

我们定义了 `State` 类，用于存储当前游戏的状态（左右两岸的牧师和魔鬼数量、船只的位置）。

```csharp
// It represents the state of the game.
public class State
{
    // 指明左岸的牧师数量。
    public int leftPriests;
    // 指明左岸的魔鬼数量。
    public int leftDevils;
    // 指明右岸的牧师数量。
    public int rightPriests;
    // 指明右岸的魔鬼数量。
    public int rightDevils;
    // 指明船的位置：true 表示在左岸；false 表示在右岸。
    public bool location;

    public State(int leftPriests, int leftDevils, int rightPriests, int rightDevils, bool location)
    {
        this.leftPriests = leftPriests;
        this.leftDevils = leftDevils;
        this.rightPriests = rightPriests;
        this.rightDevils = rightDevils;
        this.location = location;
    }
}
```

每次玩家移动人物后，都会调用 `Game` 的 `CheckWinner` 方法，因此我们可以将游戏状态的更新逻辑放置在 `CheckWinner` 函数中。

```csharp
// It determines whether the player wins the game.
public void CheckWinner()
{
    // ......

    // Update the state.
    state.leftPriests = leftPriests;
    state.leftDevils = leftDevils;
    state.rightPriests = rightPriests;
    state.rightDevils = rightDevils;
    state.location = boat.location == Location.Left;
		
  	// ......
}
```

除此之外，我们还在 `Game` 类中对外提供了**获取当前游戏状态**的接口。

```csharp
// It returns the current state.
public State GetState()
{
    return state;
}
```

### GUI

我们在游戏界面中，添加一个 Hint 按钮，当玩家点击该按钮后，即可查看当前的可行策略。

```csharp
// Show the hints.
var hintStr = "Hint:\nLeft:\tPriests:\t" + hint.leftPriests + "\tDevils:\t" + hint.leftDevils + "\nRight:\tPriests:\t" + hint.rightPriests + "\tDevils:\t" + hint.rightDevils;
GUI.Label(new Rect(Screen.width / 2 - 450, Screen.height / 2 - 220, 100, 50), hintStr, hintStyle);
// Show the Hint button.
if (GUI.Button(new Rect(Screen.width / 2 - 450, Screen.height / 2 - 280, 100, 50), "Hint", buttonStyle))
{
    action.ShowHint();
}
```

当玩家点击 Hint 按钮后，会调用 `GameController` 的 `ShowHint` 方法，该方法调用了 `AI` 类的 `Hint` 方法，获得下一步的策略。

```csharp
// Show the hint.
public void ShowHint()
{
    gui.hint = ai.Hint();
}
```

我们需要实时跟踪当前的游戏状态，因此我们在 `GameController` 中的 `ClickBoat` 方法中添加了 `UpdateAIState` 的调用。

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
    actionManager.MoveBoat(boat);
    game.CheckWinner();
    // Update the AI.
    UpdateAIState();
}

// Update the AI State.
private void UpdateAIState()
{
    var state = game.GetState();
    ai.Update(state.leftPriests, state.leftDevils, state.rightPriests, state.rightDevils, state.location);
}
```

## 游戏演示

在线演示：[demo.jiahonzheng.cn/Priests-and-Devils-V3](https://demo.jiahonzheng.cn/Priests-and-Devils-V3/)

演示视频：[www.bilibili.com/video/av78318532](https://www.bilibili.com/video/av78318532)

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%B8%B8%E6%88%8F%E6%99%BA%E8%83%BD_2.png)