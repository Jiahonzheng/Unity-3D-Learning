# 模型与动画

## 智能巡逻兵

项目地址：[github.com/Jiahonzheng/Unity-3D-Learning](https://github.com/Jiahonzheng/Unity-3D-Learning/tree/HW6/HW6)

在线演示：[demo.jiahonzheng.cn/Patrol](https://demo.jiahonzheng.cn/Patrol)

演示视频：[www.bilibili.com/video/av74087162/](https://www.bilibili.com/video/av74087162/)

### 模型动画

#### Player

我们使用 FreeVoxelGirl 素材包来构建 `Player` **玩家**人物模型，效果如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%A8%A1%E5%9E%8B%E4%B8%8E%E5%8A%A8%E7%94%BB_1.png)

我们为其添加了 `Collider` 和 `Animator` 组件，实现与其它游戏对象的碰撞检测、动画效果。以下是 `Player` 游戏对象的 `Inspector` 栏设置。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%A8%A1%E5%9E%8B%E4%B8%8E%E5%8A%A8%E7%94%BB_2.png)

我们期待 `Player` 能够不同的游戏状态展现不同的游戏动画：**空闲**、**奔跑**、**死亡**，因此我们需要设计 `Animator` 工具。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%A8%A1%E5%9E%8B%E4%B8%8E%E5%8A%A8%E7%94%BB_3.png)

上图即为 `Player` 的 `Animator` 动画控制器，其对外暴露了两个参数：**isRunning** 和 **isDead** 。`Player` 默认处于**空闲**状态，当 **isRunning** 被设置为 `true` 时，即进行**奔跑**动画。当 **isDead** 被触发时，`Player` 进行**死亡**动画的播放。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%A8%A1%E5%9E%8B%E4%B8%8E%E5%8A%A8%E7%94%BB_4.png)

#### Soldier

我们使用 VoxelCharacters 素材包来构建 `Soldier`  **巡逻兵**人物模型，效果如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%A8%A1%E5%9E%8B%E4%B8%8E%E5%8A%A8%E7%94%BB_5.png)

和 `Player` 类似，`Soldier` 也需要进行碰撞体和动画的设置，具体如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%A8%A1%E5%9E%8B%E4%B8%8E%E5%8A%A8%E7%94%BB_6.png)

下图是 `Soldier` 的动画状态机模型，其对外暴露了一个参数：**isRunning** 。当 **isRunning** 被设置为 `true` 时，`Soldier` 即进行**行走**动画的播放。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%A8%A1%E5%9E%8B%E4%B8%8E%E5%8A%A8%E7%94%BB_7.png)

### 地图生成

我们使用代码构造游戏场景地图，具体实现为 `Map` 类。我们在 `Map` 类中，声明了**地图平面预制**、**篱笆预制**、**区域Collider预制**以及**区域中心点坐标**。

```csharp
// 地图平面预制。
private static GameObject planePrefab = Resources.Load<GameObject>("Prefabs/Plane");
// 篱笆预制。
private static GameObject fencePrefab = Resources.Load<GameObject>("Prefabs/Fence");
// 区域Collider预制。
private static GameObject areaColliderPrefab = Resources.Load<GameObject>("Prefabs/AreaCollider");
// 地图 9 个区域的中心点位置。
public static Vector3[] center = new Vector3[] { new Vector3(-10, 0, -10), new Vector3(0, 0, -10), new Vector3(10, 0, -10), new Vector3(-10, 0, 0), new Vector3(0, 0, 0), new Vector3(10, 0, 0), new Vector3(-10, 0, 10), new Vector3(0, 0, 10), new Vector3(10, 0, 10) };
```

我们在函数 `LoadBoundaries` 中实现地图边界的构建。

```csharp
// 构造地图边界篱笆。
public static void LoadBoundaries()
{
    for (int i = 0; i < 12; ++i)
    {
        GameObject fence = Instantiate(fencePrefab);
        fence.transform.position = new Vector3(-12.5f + 2.5f * i, 0, -15);
    }
    for (int i = 0; i < 12; ++i)
    {
        GameObject fence = Instantiate(fencePrefab);
        fence.transform.position = new Vector3(-12.5f + 2.5f * i, 0, 15);
    }
    for (int i = 0; i < 12; ++i)
    {
        GameObject fence = Instantiate(fencePrefab);
        fence.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        fence.transform.position = new Vector3(-15, 0, -15 + 2.5f * i);
    }
    for (int i = 0; i < 12; ++i)
    {
        GameObject fence = Instantiate(fencePrefab);
        fence.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        fence.transform.position = new Vector3(15, 0, -15 + 2.5f * i);
    }
}
```

我们使用 `LoadFences` 方法构建地图内部的篱笆和通道。

```csharp
// 构造内部篱笆。
public static void LoadFences()
{
    //  为 0 表示通道，为 1 表示篱笆。
    var row = new int[2, 12] { { 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1 }, { 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0 } };
    var col = new int[2, 12] { { 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1 }, { 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0 } };
    for (int i = 0; i < 2; ++i)
    {
        for (int j = 0; j < 12; ++j)
        {
            if (row[i, j] == 1)
            {
                GameObject fence = Instantiate(fencePrefab);
                fence.transform.position = new Vector3(-12.5f + 2.5f * j, 0, -5 + 10 * i);
            }
        }
    }
    for (int i = 0; i < 2; ++i)
    {
        for (int j = 0; j < 12; ++j)
        {
            if (col[i, j] == 1)
            {
                GameObject fence = Instantiate(fencePrefab);
                fence.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                fence.transform.position = new Vector3(-5 + 10 * i, 0, -15 + 2.5f * j);
            }
        }
    }
}
```

为了探测玩家的所在区域号，我们需要为 9 个区域分别添加 `AreaCollider` 检测脚本。

```csharp
// 构造区域Collider。
public static void LoadAreaColliders()
{
    for (int i = 0; i < 9; ++i)
    {
        GameObject collider = Instantiate(areaColliderPrefab);
        collider.name = "AreaCollider" + i;
        collider.transform.position = center[i];
        // 添加区域检测脚本。
        collider.AddComponent<AreaCollider>().area = i;
    }
}
```

最终，游戏场景地图如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%A8%A1%E5%9E%8B%E4%B8%8E%E5%8A%A8%E7%94%BB_9.png)

### 区域检测

在 `LoadAreaColliders` 函数中，我们为 9 个区域分别添加了 `AreaCollider` 检测脚本。

```csharp
public class AreaCollider : MonoBehaviour
{
    // 记录当前区域号。
    public int area;

    public void OnTriggerEnter(Collider collider)
    {
        // 当玩家进入区域时。
        if (collider.gameObject.tag == "Player")
        {
            GameEventManager.GetInstance().PlayerEnterArea(area);
        }
    }
}
```

我们使用 `OnTriggerEnter` 钩子函数更新玩家当前所在的区域号，为此，我们需要设置 `AreaCollider` 预制的 `Is Trigger` 属性。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%A8%A1%E5%9E%8B%E4%B8%8E%E5%8A%A8%E7%94%BB_8.png)

在区域检测中，我们使用了**订阅与发布模式**，对游戏逻辑进行了解耦。在 `GameController` 中，我们实现了 `OnPlayerEnterArea` 方法用于订阅**玩家进入区域**的事件，该方法在 `OnTriggerEnter` 触发时被调用，即玩家摆脱一位巡逻兵，进入新区域时。

```csharp
// 当玩家摆脱一位巡逻兵，进入新区域时。
private void OnPlayerEnterArea(int area)
{
    if (model.state != GameState.RUNNING)
    {
        return;
    }
    if (currentArea != area)
    {
        // 更新分数。
        model.AddScore(1);
        // 设置追随玩家的巡逻兵。
        soldiers[currentArea].GetComponent<Soldier>().isFollowing = false;
        currentArea = area;
        soldiers[currentArea].GetComponent<Soldier>().isFollowing = true;
        actionManager.Trace(soldiers[currentArea], player);
    }
}
```

### 碰撞检测

我们在生成巡逻兵实例时，为其添加了 `SoldierCollider` 碰撞检测脚本，用于判定游戏的胜负：当巡逻兵与玩家碰撞时，游戏失败。

```csharp
public class SoldierCollider : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        // 当巡逻兵与玩家碰撞时。
        if (collision.gameObject.tag == "Player")
        {
            GameEventManager.GetInstance().SoldierCollideWithPlayer();
        }
    }
}
```

我们使用 `GameEventManager` 类（订阅与发布模式）解耦了巡逻兵与玩家碰撞时的事件处理机制。在 `GameController` 类中，我们实现了 `OnSoldierCollideWithPlayer` 方法，用于订阅**巡逻兵与玩家碰撞**的事件。

```csharp
// 当巡逻兵与玩家碰撞时。
private void OnSoldierCollideWithPlayer()
{
    view.state = model.state = GameState.LOSE;
    // 设置玩家的“死亡”动画。
    player.GetComponent<Animator>().SetTrigger("isDead");
    player.GetComponent<Rigidbody>().isKinematic = true;
    soldiers[currentArea].GetComponent<Soldier>().isFollowing = false;
    // 取消所有巡逻兵的动画。
    actionManager.Stop();
    for (int i = 0; i < 9; ++i)
    {
        soldiers[i].GetComponent<Animator>().SetBool("isRunning", false);
    }
}
```

### 动作分离

在游戏中，巡逻兵有两种动作可以展现：**自主巡逻**和**追随玩家**。为此，我们使用了**动作分离**的技术，具体代码参照 `GameActionManager` 类的实现。

#### 自主巡逻

我们在 `MoveToAction` 类中实现了巡逻兵的自主巡逻。

```csharp
// 存储自主巡逻动作。
Dictionary<int, MoveToAction> moveToActions = new Dictionary<int, MoveToAction>();

// 巡逻兵自主巡逻。
public void GoAround(GameObject patrol)
{
    var area = patrol.GetComponent<Soldier>().area;
    // 防止重入。
    if (moveToActions.ContainsKey(area))
    {
        return;
    }
    // 计算下一巡逻目的地。
    var target = GetGoAroundTarget(patrol);
    MoveToAction action = MoveToAction.GetAction(patrol, this, target, 1.5f, area);
    moveToActions.Add(area, action);
    AddAction(action);
}
```

在自主巡逻中，确定巡逻目的地，是一个核心问题。我们在 `GetGoAroundTarget` 方法中，通过随机生成目的地，并对其进行合法性判断，确定巡逻目的地。

```csharp
// 计算下一巡逻目的地。
private Vector3 GetGoAroundTarget(GameObject patrol)
{
    Vector3 pos = patrol.transform.position;
    var area = patrol.GetComponent<Soldier>().area;
    // 计算当前区域的边界。
    float x_down = -15 + (area % 3) * 10;
    float x_up = x_down + 10;
    float z_down = -15 + (area / 3) * 10;
    float z_up = z_down + 10;
    // 随机生成运动。
    var move = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
    var next = pos + move;
    int tryCount = 0;
    // 边界判断。
    while (!(next.x > x_down + 0.1f && next.x < x_up - 0.1f && next.z > z_down + 0.1f && next.z < z_up - 0.1f) || next == pos)
    {
        move = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
        next = pos + move;
        // 当无法获取到符合要求的 target 时，抛出异常。
        if ((++tryCount) > 100)
        {
            Debug.LogFormat("point {0}, area({1}, {2}, {3}, {4}, {5})", pos, area, x_down, x_up, z_down, z_up);
            throw new System.Exception("Too many loops for finding a target");
        }
    }
    return next;
}
```

#### 追随玩家

我们在 `TraceAction` 类中，实现了巡逻兵追随玩家的动作，具体方式是调用 `Vector3.MoveTowards` 方法。

```csharp
// 巡逻兵追随玩家。
public void Trace(GameObject patrol, GameObject player)
{
    var area = patrol.GetComponent<Soldier>().area;
    // 防止重入。
    if (area == currentArea)
    {
        return;
    }
    currentArea = area;
    if (moveToActions.ContainsKey(area))
    {
        moveToActions[area].destroy = true;
    }
    TraceAction action = TraceAction.GetAction(patrol, this, player, 1.5f);
    AddAction(action);
}
```

我们在 `GameController` 的 `update` 方法中，根据不同的区域，设置巡逻兵的动作类型。

```csharp
// 设置巡逻兵动作类型。
for (int i = 0; i < 9; ++i)
{
    // 不在当前区域的巡逻兵进行自主巡逻。
    if (i != currentArea)
    {
        actionManager.GoAround(soldiers[i]);
    }
    else // 在当前区域的巡逻兵对玩家进行追随。
    {
        soldiers[i].GetComponent<Soldier>().isFollowing = true;
        actionManager.Trace(soldiers[i], player);
    }
}
```

### 订阅与发布模式

在实现中，我们使用了**订阅与发布模式**对游戏逻辑进行了解耦。在 `GameEventManager` 中，我们提供了**玩家进入区域**、**巡逻兵与玩家碰撞**事件的钩子函数。

```csharp
public class GameEventManager
{
    // Singleton instance.
    private static GameEventManager instance;

    public delegate void OnPlayerEnterArea(int area);
    public static event OnPlayerEnterArea onPlayerEnterArea;

    public delegate void OnSoldierCollideWithPlayer();
    public static event OnSoldierCollideWithPlayer onSoldierCollideWithPlayer;

    // 使用单例模式。
    public static GameEventManager GetInstance()
    {
        return instance ?? (instance = new GameEventManager());
    }

    // 当玩家进入区域。
    public void PlayerEnterArea(int area)
    {
        onPlayerEnterArea?.Invoke(area);
    }

    // 当巡逻兵与玩家碰撞。
    public void SoldierCollideWithPlayer()
    {
        onSoldierCollideWithPlayer?.Invoke();
    }
}
```

在 `GameController` 的 `Awake` 函数中，我们注册了对应事件的处理函数。

```csharp
// 设置游戏事件及其处理函数。
GameEventManager.onPlayerEnterArea += OnPlayerEnterArea;
GameEventManager.onSoldierCollideWithPlayer += OnSoldierCollideWithPlayer;
```

### 游戏演示

在线演示：[demo.jiahonzheng.cn/Patrol](https://demo.jiahonzheng.cn/Patrol)

演示视频：[www.bilibili.com/video/av74087162/](https://www.bilibili.com/video/av74087162/)

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E6%A8%A1%E5%9E%8B%E4%B8%8E%E5%8A%A8%E7%94%BB_10.png)