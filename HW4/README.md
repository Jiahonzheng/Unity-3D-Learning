# 与游戏世界交互

游戏交互是玩家体验的基础，典型的游戏交互模式包括：角色扮演模型、多视角交互模型、团队交互模型、竞技交互模式、桌面与移动交互模式。

游戏创新方法，有很多层次：

- 交互装备创新：Nintendo 的《精灵宝可梦 GO》将现实地图与 GPS 位置引入游戏，预示 AR 游戏时代的到来。
- 机制创新：探索游戏在新领域（医疗、公益、电商、社交）的应用，探索游戏与智能技术的结合，探索游戏玩法的创新（Flappy Bird、Temple Run），探索游戏题材的创新（如挑战各种极限和史上最难）。
- 以客户为中心的创新：“吃鸡”类游戏很流行，大家都在忙着开发和推广这类游戏，同类游戏，各游戏厂商 PK 什么：更为细腻逼真的 3D 素材、热门故事（历史、政治事件）、满足各种“脑残粉”（开心消消乐、贪玩蓝月）。

课程讲义地址：[3D 游戏编程与设计](https://pmlpml.github.io/unity3d-learning/) 。

## 游戏规则

- 按下 Space 键发射飞碟。
- 在飞碟落地前，点击飞碟得分，飞碟落地则扣分。
- 游戏共有 10 个 Round ，每个 Round 共有 10 次 Trial ，游戏难度随 Round 增加。
- **游戏胜利**的条件为：玩家在完成 10 个 Round （即 100 次 Trial）后，其分数非负。其余所有的情况都视为**游戏失败**。

## 实现细节

在游戏总体设计中，我们使用了 MVC 模式进行程序编写，实现游戏功能的解耦合。

项目地址：[github.com/Jiahonzheng/Unity-3D-Learning](https://github.com/Jiahonzheng/Unity-3D-Learning/tree/HW4/HW4) 。

在线预览：[demo.jiahonzheng.cn/UFO](https://demo.jiahonzheng.cn/UFO) 。

预览视频：[Unity 打飞碟](https://www.bilibili.com/video/av70495951) 。

### 文件说明

#### Model

- `GameModel` ：裁判类，管理游戏关卡和判断游戏胜负。
- `UFOModel` ：飞碟，管理飞碟的位置、速度和缩放比例。

#### View

- `GameGUI` ：显示轮次、得分，展示游戏画面。

#### Controller

- `Director` ：导演类，控制场景切换。
- `GameController` ：场景控制类，控制游戏场景。
- `Ruler` ：协助 `GameController` ，控制飞碟的各种特性（位置、方向、得分、大小）。
- `UFOFactory` ：带有缓存的工厂，生成飞碟对象。

#### Editors

- `UFOEditor` ：`UFO` 游戏对象的编辑器扩展。

### 对象回收

在游戏中，对象的新建和销毁的开销是巨大的，是不可忽视的。对于频繁出现的游戏对象，我们应该使用**对象池**技术缓存，从而降低对象的新建和销毁开销。在本游戏中，飞碟是频繁出现的游戏对象，我们使用**带缓存的工厂模式**管理不同飞碟的生产和回收。对于该飞碟工厂，我们使用**单例模式**。

```csharp
public class UFOFactory
{
    // 定义飞碟颜色。
    public enum Color
    {
        Red,
        Green,
        Blue
    }

    // 单例。
    private static UFOFactory factory;
    // 维护正在使用的飞碟对象。
    private List<GameObject> inUsed = new List<GameObject>();
    // 维护未被使用的飞碟对象。
    private List<GameObject> notUsed = new List<GameObject>();
    // 空闲飞碟对象的空间位置。
    private readonly Vector3 invisible = new Vector3(0, -100, 0);

    // 使用单例模式。
    public static UFOFactory GetInstance()
    {
        return factory ?? (factory = new UFOFactory());
    }

    // 获取特定颜色的飞碟。
    public GameObject Get(Color color)
    {
        GameObject ufo;
        if (notUsed.Count == 0)
        {
            ufo = Object.Instantiate(Resources.Load<GameObject>("Prefabs/UFO"), invisible, Quaternion.identity);
            ufo.AddComponent<UFOModel>();
        }
        else
        {
            ufo = notUsed[0];
            notUsed.RemoveAt(0);
        }

        // 设置 Material 属性（颜色）。
        Material material = Object.Instantiate(Resources.Load<Material>("Materials/" + color.ToString("G")));
        ufo.GetComponent<MeshRenderer>().material = material;

        // 添加对象至 inUsed 列表。
        inUsed.Add(ufo);
        return ufo;
    }

    // 回收飞碟对象。
    public void Put(GameObject ufo)
    {
        // 设置飞碟对象的空间位置和刚体属性。
        var rigidbody = ufo.GetComponent<Rigidbody>();
        // 以下两行代码很关键！我们需要设置对象速度为零！
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.useGravity = false;
        ufo.transform.position = invisible;
        // 维护 inUsed 和 notUsed 列表。
        inUsed.Remove(ufo);
        notUsed.Add(ufo);
    }
}
```

在 `UFOFactory` 中，我们使用两个列表来分别维护**正在使用**和**未被使用**的飞碟对象。我们对外提供了 `Get` 获取飞碟对象和 `Put` 回收飞碟对象的基本接口。由于使用了单例模式，我们还对外提供了 `GetInstance` 的静态方法。

在新建飞碟对象时，我们为其添加了 `UFOModel` 的 `Mono` 脚本，主要管理飞碟对象的**初始位置**、**初始速度**和**缩放比例**，其具体内容如下。

```csharp
public class UFOModel : MonoBehaviour
{
    // 记录当前飞碟的分数。
    public int score;
    // 记录飞碟在左边的初始位置。
    public static Vector3 startPosition = new Vector3(-3, 2, -15);
    // 记录飞碟在左边的初始速度。
    public static Vector3 startSpeed = new Vector3(3, 11, 8);
    // 记录飞碟的初始缩放比例。
    public static Vector3 localScale = new Vector3(1, 0.08f, 1);
    // 表示飞碟的位置（左边、右边）。
    private int leftOrRight;

    // 获取实际初速度。
    public Vector3 GetSpeed()
    {
        Vector3 v = startSpeed;
        v.x *= leftOrRight;
        return v;
    }

    // 设置实际初位置。
    public void SetSide(int lr, float dy)
    {
        Vector3 v = startPosition;
        v.x *= lr;
        v.y += dy;
        transform.position = v;
        leftOrRight = lr;
    }

    // 设置实际缩放比例。
    public void SetLocalScale(float x, float y, float z)
    {
        Vector3 lc = localScale;
        lc.x *= x;
        lc.y *= y;
        lc.z *= z;
        transform.localScale = lc;
    }
}
```

### 发射飞碟

我们在场景控制器 `GameController` 的 `Update` 函数，对用户的输入进行监听。在特定游戏状态下（避免在本次 Trial 飞碟未销毁的情况下，进入下一 Trial），当用户按下空格键时，我们触发飞碟的发射函数 `ruler.GetUFOs` 。

```csharp
if (model.game == GameState.Running)
{
    if (model.scene == SceneState.Waiting && Input.GetKeyDown("space"))
    {
        model.scene = SceneState.Shooting;
        model.NextTrial();
      	// 添加此判断的原因：对于最后一次按下空格键，若玩家满足胜利条件，则不发射飞碟。
        if (model.game == GameState.Win)
        {
            return;
        }
        UFOs.AddRange(ruler.GetUFOs());
    }
}
```

我们使用 `Ruler` 管理飞碟特性（颜色、分值、同时出现的数目）与关卡进度的关系，其对外提供了 `GetUFOs` 方法，用于发射飞碟。

```csharp
// 在用户按下空格键后被触发，发射飞碟。
public List<GameObject> GetUFOs()
{
    List<GameObject> ufos = new List<GameObject>();
    // 随机生成飞碟颜色。
    var index = random.Next(colors.Length);
    var color = (UFOFactory.Color)colors.GetValue(index);
    // 获取当前 Round 下的飞碟产生数。
    var count = GetUFOCount();
    for (int i = 0; i < count; ++i)
    {
        // 调用工厂方法，获取指定颜色的飞碟对象。
        var ufo = UFOFactory.GetInstance().Get(color);
        // 设置飞碟对象的分数。
        var model = ufo.GetComponent<UFOModel>();
        model.score = score[index] * (currentRound + 1);
        // 设置飞碟对象的缩放比例。
        model.SetLocalScale(scale[index], 1, scale[index]);
        // 随机设置飞碟的初始位置（左边、右边）。
        var leftOrRight = (random.Next() & 2) - 1; // 随机生成 1 或 -1 。
        model.SetSide(leftOrRight, i);
        // 设置飞碟对象的刚体属性，以及初始受力方向。
        var rigidbody = ufo.GetComponent<Rigidbody>();
        rigidbody.AddForce(0.2f * speed[index] * model.GetSpeed(), ForceMode.Impulse);
        rigidbody.useGravity = true;
        ufos.Add(ufo);
    }
    return ufos;
}
```

### 点击判断

在游戏中，玩家通过鼠标点击飞碟，从而得分。这当中涉及到一个**点击判断**的问题。在 Unity 中，我们可以调用 `ScreenPointToRay` 方法，**构造由摄像头和屏幕点击点确定的射线**，**与射线碰撞的游戏对象即为玩家点击的对象**。

```csharp
// 光标拾取单个游戏对象。
// 构建射线。
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
// 当射线与飞碟碰撞时，即说明我们想用鼠标点击此飞碟。
if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.tag == "UFO")
{
    OnHitUFO(hit.collider.gameObject);
}
```

### 爆炸效果

我们在 Assets Store 下载 "Particle Dissolve Shader by Moonflower Carnivore" 素材库，引入其中的爆炸效果 `Prefab` 。由于，我们期待爆炸效果能够持续一段时间，然后停止，因此，我们使用**协程**对爆炸效果进行管理。

我们在 `OnHitUFO` 函数中，创建协程。在该协程中，我们实例化预制件，并赋予其被点击的 UFO 的位置，随后调用 `WaitForSeconds` 方法，并让出协程。在重新获得执行机会后，我们调用 `Destroy` 方法，销毁爆炸效果对象。

```csharp
// 在用户成功点击飞碟后被触发。
private void OnHitUFO(GameObject ufo)
{
    // 增加分数。
    model.AddScore(ufo.GetComponent<UFOModel>().score);
    // 创建协程，用于控制飞碟爆炸效果的延续时间。
    StartCoroutine("DestroyExplosion", ufo);
}

// 该协程用于控制飞碟爆炸效果。
private IEnumerator DestroyExplosion(GameObject ufo)
{
    // 实例化预制。
    GameObject explosion = Instantiate(explosionPrefab);
    // 设置爆炸效果的位置。
    explosion.transform.position = ufo.transform.position;
    // 回收飞碟对象。
    DestroyUFO(ufo);
    // 爆炸效果持续 1.2 秒。
    yield return new WaitForSeconds(1.2f);
    // 销毁爆炸效果对象。
    Destroy(explosion);
}
```

鼠标点击飞碟后的爆炸效果演示如下。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/与游戏世界交互_1.jpg)

### 裁判类

基于职责分离的考虑，我们将游戏胜负判断的逻辑抽离出来，用 `GameModel` 类实现。

#### 数据成员

为了管理游戏的不同状态，我们声明了以下枚举常量。

```csharp
public enum GameState
{
    Running, // 正在进行
    Lose, // 玩家失败
    Win // 玩家胜利
}

public enum SceneState
{
    Waiting, // 等待用户按下空格键
    Shooting // 等待用户点击鼠标左键
}
```

裁判类具有以下数据成员。

```csharp
public class GameModel
{
    // 表示当前游戏状态（正在进行、玩家胜利、玩家失败）。
    public GameState game = GameState.Running;
    // 表示当前场景状态（等待用户按下空格键、等待用户点击鼠标左键）。
    public SceneState scene = SceneState.Waiting;
    // 表示最大 Round 次数。
    public readonly int maxRound = 10;
    // 表示单个 Round 中最大 Trial 次数。
    public readonly int maxTrial = 10;
    // 表示当前是第几个 Round 。
    public int currentRound { get; private set; } = 0;
    // 表示当前是第几个 Trial 。
    public int currentTrial { get; private set; } = 0;
    // 维护当前的玩家分数。
    public int score { get; private set; } = 0;
}
```

#### 玩家分数管理

当玩家**点击飞碟**或**错失飞碟**时，场景控制器 `GameController` 会调用裁判类的 `AddScore` 和 `SubScore` 方法，更新玩家分数。

```csharp
// 增加玩家分数。
public void AddScore(int score)
{
    this.score += score;
    // 通知场景控制器需要更新游戏画面。
    onRefresh.Invoke(this, EventArgs.Empty);
}

// 扣除玩家分数。
public void SubScore()
{
    this.score -= (currentRound + 1) * 10;
    // 检测玩家是否失败。
    if (score < 0)
    {
        Reset(GameState.Lose);
    }
    onRefresh.Invoke(this, EventArgs.Empty);
}
```

注意到，在上述代码中，我们使用了 `onRefresh.Invoke` 方法，因为我们的裁判类对外提供了多个 `EventHandler` 类型变量，用于**事件的通知传递**，实现解耦合。

```csharp
// 通知场景控制器更新游戏画面。
public EventHandler onRefresh;
// 通知场景控制器更新 Ruler 。
public EventHandler onEnterNextRound;
```

在 `GameController` 场景控制器的 `Awake` 函数中，我们对裁判类的 `EventHandler` 变量进行了设置。

```csharp
// model 即为裁判类。
// 更新游戏画面。
model.onRefresh += delegate
{
    view.state = model.game;
    view.round = model.currentRound;
    view.trial = model.currentTrial;
    view.score = model.score;
};
// 更新 Ruler 。
model.onEnterNextRound += delegate
{
    ruler = new Ruler(model.currentRound);
};
```

#### 关卡机制

在裁判类，我们使用 `currentRound` 和 `currentTrial` 维护当前游戏的关卡进行状态。当用户按下空格键发射飞碟，即表示开启新的 Trial ，裁判类的 `NextTrial` 方法被调用。在此方法中，我们实现了 `currentRound` 和 `currentTrial` 的联动更新，以及对玩家胜利的判断。

```csharp
// 进入下一 Trial 。
public void NextTrial()
{
    ++currentTrial;
    if (currentTrial == maxTrial)
    {
        currentTrial = 0;
        ++currentRound;
        // 检测玩家是否胜利。
        if (currentRound == maxRound)
        {
            Reset(GameState.Win);
        }
        else
        {
            onEnterNextRound.Invoke(this, EventArgs.Empty);
        }
    }
    onRefresh.Invoke(this, EventArgs.Empty);
}
```

`NextTrial` 调用了 `Reset` 方法，其具体代码如下。

```csharp
// 重置裁判类。
public void Reset(GameState _game = GameState.Running)
{
    game = _game;
    scene = SceneState.Waiting;
    currentRound = 0;
    currentTrial = 0;
    score = 0;
    // 通知场景控制器需要重置游戏画面。
    onRefresh.Invoke(this, EventArgs.Empty);
    // 通知场景控制器需要重置 Ruler 。
    onEnterNextRound.Invoke(this, EventArgs.Empty);
}
```

## 游戏演示

游戏胜利页面如下。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/与游戏世界交互_2.jpg)

游戏失败页面如下。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/与游戏世界交互_3.jpg)

## 自定义 Component

为了更为方便地设置飞碟参数，我们在 `UFOEditor` 类中对其进行了编辑器扩展，具体代码如下所示。

```csharp
[CustomEditor(typeof(UFOModel))]
public class UFOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var target = (UFOModel)serializedObject.targetObject;

        EditorGUILayout.Space();
        Vector3 startPosition = EditorGUILayout.Vector3Field("Start Position", UFOModel.startPosition);
        UFOModel.startPosition = startPosition;

        EditorGUILayout.Space();
        Vector3 startSpeed = EditorGUILayout.Vector3Field("Initial Speed", UFOModel.startSpeed);
        UFOModel.startSpeed = startSpeed;

        EditorGUILayout.Space();
        Vector3 localScale = EditorGUILayout.Vector3Field("Local Scale", UFOModel.localScale);
        UFOModel.localScale = localScale;
    }
}
```

在 `UFOEditor` 中，我们创建了分别对飞碟的**初始位置**、**初始速度**、**缩放比例**进行设置的编辑菜单。点击运行按钮后，发射飞碟，并点击任意 `UFO` 游戏对象，即可在右侧的 Inspector 菜单观察到以下功能区。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/与游戏世界交互_4.jpg)
