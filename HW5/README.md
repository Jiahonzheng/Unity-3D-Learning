# 物理系统与碰撞

博客链接：[物理系统与碰撞](https://blog.jiahonzheng.cn/2019/10/17/%E7%89%A9%E7%90%86%E7%B3%BB%E7%BB%9F%E4%B8%8E%E7%A2%B0%E6%92%9E/) 。

游戏世界运动分为两大类：**运动学**、**动力学**。

**运动学**运用几何学的方法研究物体的运动：

+ 不考虑外部力作用
+ 将一个物体作为几何部件，抽象为**质点运动模型**
+ 仅考虑物体位置、速度、角度等
+ 具体实现方法：线性代数的矩阵变换

**动力学**以牛顿运动定律为基础，研究运动速度远小于光速的宏观物体。在游戏物理引擎中，主要是**刚体动力学**，主要包括质点系动力学的基本定理，由动量定理、动量矩定理、动能定理以及由这三个基本定理推导出来的一些定理。**动量**、**动量矩**和**动能**是描述质点、质点系和刚体运动的基本物理量。

+ 考虑外部力对物体运动的影响
+ 将一个物体当作**刚体**
+ 考虑物体的重力、阻力、摩擦力、重量、形状以及弹性等
+ 具体实现方式：物理引擎

**物理引擎**是一个软件组件，将游戏世界对象赋予现实世界物理属性（重量、形状等），并抽象为**刚体**模型，使得游戏物体在力的作用下，仿真现实世界的运动及其之间的碰撞过程。

## 《打飞碟 V2》

项目地址：[github.com/Jiahonzheng/Unity-3D-Learning](https://github.com/Jiahonzheng/Unity-3D-Learning/tree/HW5/HW5/UFO)

在线预览：[demo.jiahonzheng.cn/UFO-V2](demo.jiahonzheng.cn/UFO-V2) 。

预览视频：[Unity 打飞碟 V2](https://www.bilibili.com/video/av71369612)  。

在 [与游戏世界交互](https://blog.jiahonzheng.cn/2019/10/08/与游戏世界交互/) 文章中，我们实现了[《打飞碟》](https://demo.jiahonzheng.cn/UFO)游戏，在游戏中，我们已使用了**动力学模型**实现飞碟的运动。现在，我们使用 **Adapter** 设计模式，为游戏添加**运动学模型**的支持，实现二者模型的自由切换。

### Adapter 接口

首先，我们定义 Adapter 接口 `IActionManager` ，其含有 `SetAction` 方法，用于设置游戏对象的运动学模型。

```csharp
public interface IActionManager
{
    void SetAction(GameObject ufo);
}
```

### 动力学模型

这里，我们对上一版本的动力学模型代码进行重构，具体代码位于 `PhysicActionManager` 。

```csharp
public class PhysicActionManager : IActionManager
{
    public void SetAction(GameObject ufo)
    {
        var model = ufo.GetComponent<UFOModel>();
        var rigidbody = ufo.GetComponent<Rigidbody>();
        // 对物体添加 Impulse 力。
        rigidbody.AddForce(0.2f * model.GetSpeed(), ForceMode.Impulse);
        rigidbody.useGravity = true;
    }
}
```

### 运动学模型

我们在 `CCActionManager` 实现运动学模型，具体代码如下。在 `SetAction` 方法中，我们设置了**刚体重力**属性，同时对游戏对象添加了 `CCAction` 脚本。在此脚本中，我们调用 `Vector3.MoveTowards` 方法赋予游戏对象运动。

```csharp
public class CCActionManager : IActionManager
{
    private class CCAction : MonoBehaviour
    {
        void Update()
        {
            // 关键！当飞碟被回收时，销毁该运动学模型。
            if (transform.position == UFOFactory.invisible)
            {
                Destroy(this);
            }
            var model = gameObject.GetComponent<UFOModel>();
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-3f + model.GetID() * 2f, 10f, -2f), 5 * Time.deltaTime);
        }
    }

    public void SetAction(GameObject ufo)
    {
        // 由于预设使用了 Rigidbody ，故此处取消重力设置。
        ufo.GetComponent<Rigidbody>().useGravity = false;
        // 添加运动学（转换）运动。
        ufo.AddComponent<CCAction>();
    }
}
```

在游戏实现中，我们在 `Ruler` 类的 `GetUFOs` 方法中实现不同关卡下的飞碟对象生成功能，我们使用 `actionManager` 对飞碟对象的运动学模型进行设置。

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
        model.SetSide(leftOrRight);
        // 设置飞碟的速度比例。
        model.SetSpeedScale(speed[index]);
        // 设置飞碟 ID 。
        model.SetID(i);
        // 设置飞碟对象的运动学属性。
        actionManager.SetAction(ufo);
        ufos.Add(ufo);
    }
    return ufos;
}
```

### 游戏演示

在线预览：

+ 运动学模型：[demo.jiahonzheng.cn/UFO-V2](demo.jiahonzheng.cn/UFO-V2) 
+ 动力学模型：[demo.jiahonzheng.cn/UFO](demo.jiahonzheng.cn/UFO) 

演示视频：

+ 运动学模型：[Unity 打飞碟 V2](https://www.bilibili.com/video/av71369612) 
+ 动力学模型：[Unity 打飞碟](https://www.bilibili.com/video/av70495951) 

以下是使用**运动学模型**的游戏截屏。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%89%A9%E7%90%86%E7%B3%BB%E7%BB%9F%E4%B8%8E%E7%A2%B0%E6%92%9E_1.png)

以下是使用**动力学模型**的游戏截屏。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%89%A9%E7%90%86%E7%B3%BB%E7%BB%9F%E4%B8%8E%E7%A2%B0%E6%92%9E_2.png)

## 《射箭》

项目地址：[github.com/Jiahonzheng/Unity-3D-Learning](https://github.com/Jiahonzheng/Unity-3D-Learning/tree/HW5/HW5/Archery) 。

在线预览：[demo.jiahonzheng.cn/Archery/](https://demo.jiahonzheng.cn/Archery/) 。

演示视频：

### 游戏规则

+ 靶对象为 5 环，按环计分。
+ 按下空格键获取箭，鼠标控制射箭方向，点击鼠标左键发射箭。
+ 游戏只有 1 Round，但有无限 Trials 
  + 增强要求：添加一个风向和强度标志，提高难度。

### 获取箭

我们实现了 `ArrowFactory` 工厂，用于生成和回收箭对象。

```csharp
public class ArrowFactory
{
    // 单例。
    private static ArrowFactory factory;
    // 维护正在使用的箭对象。
    private List<GameObject> inUsed = new List<GameObject>();
    // 维护未被使用的箭对象。
    private List<GameObject> notUsed = new List<GameObject>();
    // 空闲箭的空间位置。
    public static Vector3 INITIAL_POSITION = new Vector3(0, 0, -19);

    // 使用单例模式。
    public static ArrowFactory GetInstance()
    {
        return factory ?? (factory = new ArrowFactory());
    }

    // 获取一支箭。
    public GameObject Get()
    {
        GameObject arrow;
        if (notUsed.Count == 0)
        {
            arrow = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"));
            inUsed.Add(arrow);
        }
        else
        {
            arrow = notUsed[0];
            notUsed.RemoveAt(0);
            arrow.SetActive(true);
            inUsed.Add(arrow);
        }
        return arrow;
    }

    // 回收一支箭。
    public void Put(GameObject arrow)
    {
        arrow.GetComponent<Rigidbody>().isKinematic = true;
        arrow.SetActive(false);
        arrow.transform.position = INITIAL_POSITION;
        notUsed.Add(arrow);
        inUsed.Remove(arrow);
    }
}
```

### 控制箭方向

我们在 `GameController` 捕捉用户的鼠标位置，从而控制箭的方向。

```csharp
var direction = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
// 控制箭的方向为鼠标指针方向。
MoveArrow(direction);
```

我们在 `MoveArrow` 方法中，对箭的方向进行调整。

```csharp
// 控制箭的指向。
public void MoveArrow(Vector3 direction)
{
    holdingArrow.transform.LookAt(30 * direction);
}
```

### 碰撞检测

当用户点击鼠标左键后，会触发 `ShootArrow` 方法，从而实现射箭的动作。在此方法中，我们设置了箭的 `isKinematic` 属性为 `false` ，同时为其添加属性为 `Impulse` 的力。

```csharp
// 发射箭。
public void ShootArrow(Vector3 direction)
{
    var collider = holdingArrow.GetComponentInChildren<ArrowCollider>();
    // 重置箭的击中状态。
    collider.Reset();
    // 设置箭击中物体后的回调函数。
    collider.onArrowHitObject = (sender, e) =>
    {
        OnArrowHitObject(e);
    };
    // 添加 Impulse 力。
    var rigidbody = holdingArrow.GetComponent<Rigidbody>();
    rigidbody.isKinematic = false;
    rigidbody.AddForce(30 * direction, ForceMode.Impulse);
    holdingArrow = null;
    // 设置游戏状态。
    model.scene = SceneState.Shooting;
}
```

我们的箭对象的层次结构如下，注意到 `Head` 添加了 `ArrowCollider` 脚本。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%89%A9%E7%90%86%E7%B3%BB%E7%BB%9F%E4%B8%8E%E7%A2%B0%E6%92%9E_4.png)

我们的箭靶对象的层次结构如下，由 5 个 Cylinder 构成，注意到它们都已设置了 `target` 标签。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%89%A9%E7%90%86%E7%B3%BB%E7%BB%9F%E4%B8%8E%E7%A2%B0%E6%92%9E_5.png)

在 `ArrowCollider` 脚本中，我们实现了 `OnTriggerEnter` 方法，用于判断与箭触碰的游戏对象类型。

```csharp
public class ArrowCollider : MonoBehaviour
{
    public EventHandler<ArrowHitObjectEvent> onArrowHitObject;
    public bool isHitTarget = true;

    public void Reset()
    {
        isHitTarget = true;
    }

    void OnTriggerEnter(Collider other)
    {
        var otherObject = other.gameObject;
        var arrow = gameObject.transform.parent.gameObject;
        // 当箭击中箭靶时。
        if (otherObject.tag == "target")
        {
            arrow.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.SetActive(false);
            int target = int.Parse(otherObject.name);
            isHitTarget = true;
            onArrowHitObject.Invoke(this, new ArrowHitObjectEvent(arrow, target));
        }
        else // 当箭击中其他物体时。
        {
            isHitTarget = false;
            onArrowHitObject.Invoke(this, new ArrowHitObjectEvent(arrow, 0));
        }
    }
}
```

我们使用了 `EventHandler` 实现 `ArrowCollider` 和 `GameController` 的通信。首先，我们实现了 `ArrowHitObjectEvent` 类，用于定义通讯内容，其具体代码如下。

```csharp
public class ArrowHitObjectEvent : EventArgs
{
    public GameObject arrow;
    public int target;

    public ArrowHitObjectEvent(GameObject arrow, int target)
    {
        this.arrow = arrow;
        this.target = target;
    }
}
```

随后，我们在 `GameController` 实现对此事件的响应。

```csharp
// 当箭命中物体时，此函数被触发执行。
void OnArrowHitObject(ArrowHitObjectEvent e)
{
    model.AddScore(e.target);
    // 只有当 target 不为零时，才是击中箭靶，否则只是击中箭。
    if (e.target != 0)
    {
        arrows.Remove(e.arrow);
        model.scene = SceneState.WaitToGetArrow;
    }
}
```

### 风向与风速

为了实现随机风向和风速的生成，我们需要扩充现有的 `GameModel` 的逻辑，更改代码如下。

```csharp
public class GameModel
{
    public SceneState scene = SceneState.WaitToGetArrow;
    public int score = 0;
    public EventHandler<GameModelChangedEvent> onGameModelChanged;
    public Wind currentWind = new GameModel.Wind(Vector3.zero, 0, "");
    // 风向。
    private Vector3[] winds = new Vector3[8] { new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector3(1, -1, 0), new Vector3(0, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 0, 0), new Vector3(-1, 1, 0) };
    // 风向描述。
    private string[] windsText = new string[8] { "↑", "↗", "→", "↘", "↓", "↙", "←", "↖" };

    public class Wind
    {
        public Vector3 direction;
        public int strength;
        public string text;

        public Wind(Vector3 d, int s, string t)
        {
            direction = d;
            strength = s;
            text = t;
        }
    }

    // 添加分数
    public void AddScore(int target)
    {
        score += target;
        onGameModelChanged.Invoke(this, new GameModelChangedEvent(score, target));
    }

    // 添加随机风向。
    public void AddWind()
    {
        var index = UnityEngine.Random.Range(0, 8);
        var strength = UnityEngine.Random.Range(5, 10);
        currentWind.direction = winds[index] * strength;
        currentWind.strength = strength;
        currentWind.text = windsText[index];
    }
}
```

我们定义了 `GameModel.Wind` 的类，用于表示当前风向和风速。我们期望在玩家按下空格键获取时，风向得到改变，因此我们在 `GetArrow` 方法添加 `AddWind` 调用。

```csharp
// 获取箭。
public void GetArrow()
{
    holdingArrow = arrowFactory.Get();
    arrows.Add(holdingArrow);
    // 设置风向和风速。
    model.AddWind();
    view.ShowWind(model.currentWind);
    // 设置游戏状态。
    model.scene = SceneState.WaitToShootArrow;
}
```

在射出箭时，我们需要将风力反应到箭的运动上。

```csharp
// 发射箭。
public void ShootArrow(Vector3 direction)
{
    var collider = holdingArrow.GetComponentInChildren<ArrowCollider>();
    // 重置箭的击中状态。
    collider.Reset();
    // 设置箭击中物体后的回调函数。
    collider.onArrowHitObject = (sender, e) =>
    {
        OnArrowHitObject(e);
    };
    // 添加 Impulse 力。
    var rigidbody = holdingArrow.GetComponent<Rigidbody>();
    rigidbody.isKinematic = false;
    rigidbody.AddForce(30 * direction, ForceMode.Impulse);
    holdingArrow = null;
    // 添加风力
    rigidbody.AddForce(5 * model.currentWind.direction, ForceMode.Force);
    // 设置游戏状态。
    model.scene = SceneState.Shooting;
}
```

### 游戏演示

在线预览：[demo.jiahonzheng.cn/Archery/](https://demo.jiahonzheng.cn/Archery/) 。

演示视频：[www.bilibili.com/video/av71662069/](https://www.bilibili.com/video/av71662069/) 。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%89%A9%E7%90%86%E7%B3%BB%E7%BB%9F%E4%B8%8E%E7%A2%B0%E6%92%9E_6.png)