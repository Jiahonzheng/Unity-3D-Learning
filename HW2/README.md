# 空间与运动

博客链接：[空间与运动](https://blog.jiahonzheng.cn/2019/09/16/空间与运动/)

不管游戏世界是虚拟的或现实的，游戏世界中所有物体（游戏对象）都必须在特定的空间、时间下出现、变化、消失。因此，游戏设计必须定义空间、时间等。

在[《游戏设计基础》](https://book.douban.com/subject/3529767/)中，作者对游戏设计中的空间维度进行了分析：

- 自由度：2D 或 2D 卷轴、2.5D（如 Aircraft）、3D。
- 尺度：游戏世界的度量单位，如米、公里、光年，特别是其他物体与玩家对象的相对大小设计。
- 边界：玩家可以看到的地图与场景。

课程讲义地址：[3D 游戏编程与设计](https://pmlpml.github.io/unity3d-learning/03-space-and-motion) 。

## 游戏空间模型

### 世界坐标

一个游戏或游戏场景的 **绝对坐标** 系统。每个游戏对象的位置、角度、比例的值都这个坐标系下是唯一的。

### 对象坐标

游戏对象相对父游戏对象的位置、角度、比例，又称为 **相对坐标** 。

### 3D 空间

3D 空间坐标比较简单，下面是一个典型的 3D 正交坐标系统。

- Z 轴：深度维度，前后方向。Z 越小越靠前
- Y 轴：高度维度，上下方向。Y 越大越高
- X 轴：水平维度，左右方向。

3D 正交坐标系统大致分为两种：**左手坐标系**、**右手坐标系** ，Unity3D 所使用的坐标系为左手坐标系。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%A9%BA%E9%97%B4%E4%B8%8E%E8%BF%90%E5%8A%A8_2.png)

### 2D 空间

相比于 3D 空间坐标系，2D 空间坐标系更为复杂。

- 离散 2D 坐标。瓦片空间，或网格空间，或棋盘空间都是一个概念。即使用整数完成游戏对象运动、碰撞等计算（早期计算机浮点性能很差哦），特别是蜂窝状六边形地图。
- 连续 2D 坐标。
- 混合坐标系统。看上去游戏对象连续运动，但内部计算是网格模型。例如，3D 象棋。

## Answers

下面是 [空间与运动](https://pmlpml.github.io/unity3d-learning/03-space-and-motion) 章节的作答。

### Question 1.1

> 游戏对象运动的本质是什么？

游戏运动本质就是使用**矩阵变换**（平移、旋转、缩放）改变游戏对象的空间属性。

下面是**平移**的示意代码。

```csharp
public class Translate : MonoBehaviour
{
    // Speed of Translation.
    public float speed = 2;
    // Direction of Translation.
    public Vector3 translation = Vector3.forward;

    void Update()
    {
        // It moves a little in every frame.
        transform.Translate(speed * Time.deltaTime * translation);
    }
}
```

下面是**旋转**的示意代码。

```csharp
public class Rotate : MonoBehaviour
{
    // Speed of Rotation
    public float speed = 20;
    public Vector3 axis = Vector3.forward;

    void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}
```

下面是**缩放**的示意代码。

```csharp
public class Zoom : MonoBehaviour
{
    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * Time.deltaTime;
        transform.localScale += new Vector3(x, 0, z);
    }
}
```

### Question 1.2

> 请用三种方法以上方法，实现物体的抛物线运动。

#### 直接设置物体的位置

物体的抛物线运动，可以用函数 $y(x)=x^2$ 表示，从该角度出发，我们可以通过**直接设置物体的位置**使其进行抛物线运动。

```csharp
using UnityEngine;

public class Example : MonoBehaviour
{
    // Update is called once per frame.
    void Update ()
    {
        transform.position += Vector3.right * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.x * transform.position.x, 0);
    }
}
```

#### transform.Translate

与直接设置物体的位置类似，我们使用 `transform.Translate` 设置物体的位置，从而使其进行抛物线运动。

```csharp
using UnityEngine;

public class Example : MonoBehaviour
{
    private float t = 1;
    // Update is called once per frame
    void Update ()
    {
        t += Time.deltaTime;
        transform.Translate(Vector3.left * Time.deltaTime, Space.World);
        transform.Translate(Vector3.down * t * Time.deltaTime, Space.World);
    }
}
```

#### Rigidbody

我们可以为物体设置刚体属性，使其能受重力影响，最后给予其水平初速度，即可使其进行抛物线运动。

```csharp
using UnityEngine;

public class Example : MonoBehaviour
{
    // Update is called once per frame.
    void Update ()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        // Give it a horizontal speed.
        rb.velocity = Vector3.left * 4;
    }
}
```

#### Vector3.Slerp

我们可以使用 `Vector3.Slerp` 插值实现物体的抛物线运动。

```csharp
using UnityEngine;

public class Example : MonoBehaviour
{
    private float t = 1;

    // Update is called once per frame.
    void Update ()
    {
    		t += Time.deltaTime;
        Vector3 next = transform.position + Vector3.left * Time.deltaTime + Vector3.down * t * Time.deltaTime;
        transform.position = Vector3.Slerp(transform.position, next, 1);
    }
}
```

### Question 1.3

> 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。

**项目地址**：[github.com/Jiahonzheng/Unity-3D-Learning](https://github.com/JIahonzheng/Unity-3D-Learning/tree/HW2/HW2/Solar-System)。

**在线演示**：[demo.jiahonzheng.cn/Solar-System](https://demo.jiahonzheng.cn/Solar-System/) 。

**演示视频**：[Unity 模拟太阳系](https://www.bilibili.com/video/av68043118/) 。

#### GameObject

为实现一个完整的太阳系的模拟，我们需要 `Sphere` 模拟太阳系中的各个星体：`Sun` 、`Mercury` 、`Venus` 、`Earth`、`Mars`、`Jupiter`、`Saturn`、`Uranus`、`Neptune`。注意到，`Moon` 是归属于 `Earth` 的子对象。

为了使得星体更为逼真，我们基于 `Surface` 中的图片制作了各星体的 `Material` 。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%A9%BA%E9%97%B4%E4%B8%8E%E8%BF%90%E5%8A%A8_1.png)

#### 公转

`Revolution` 是星体公转的脚本代码，其中的 `center` 指明公转运动的中心，即指明绕谁公转。我们在 `Start` 函数中，随机设置公转速度，以及公转法平面 `(x, y, 0)` 。在 `Update` 函数中，使用 `RotateAround` 函数进行公转的模拟。

```csharp
public class Revolution : MonoBehaviour
{
    public Transform center;
    private float speed;
    private float x;
    private float y;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(9, 12);
        x = Random.Range(-50, 50);
        y = Random.Range(-50, 50);
    }

    // Update is called once per frame
    void Update()
    {
        var axis = new Vector3(0, x, y);
        transform.RotateAround(center.position, axis, speed * Time.deltaTime);
    }
}
```

#### 自转

`Rotation` 是星体自转运动的脚本代码。我们在 `Update` 函数中，使用 `RotateAround` 使得星体绕着星体所在位置的 `Vector3.up` 进行旋转，来模拟星体的自转运动。

```csharp
public class Rotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, Random.Range(1, 3));
    }
}
```

#### 演示

**在线演示**：[demo.jiahonzheng.cn/Solar-System](https://demo.jiahonzheng.cn/Solar-System/) 。

**演示视频**：[Unity 模拟太阳系](https://www.bilibili.com/video/av68043118/) 。

为了记录星体运动轨迹，我们为各个星体添加了 `Trail Renderer` 组件。

注意：`Trail Renderer` 需要设置 `Material` 才可显示所设置的颜色。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%A9%BA%E9%97%B4%E4%B8%8E%E8%BF%90%E5%8A%A8_3.png)

### Question 2.1

> 阅读以下游戏脚本:
>
> > Priests and Devils
> >
> > Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many > ways. Keep all priests alive! Good luck!
>
> 程序需要满足的要求：
>
> - Play the game ( http://www.flash-game.net/game/2535/priests-and-devils.html )。
> - 列出游戏中提及的事物（Objects）。
> - 用表格列出玩家动作表（规则表），注意，动作越少越好。
> - 请将游戏中对象做成预制。
> - 在 GenGameObjects 中创建长方形、正方形、球及其色彩代表游戏中的对象。
> - 使用 C# 集合类型有效组织对象。
> - 整个游戏仅主摄像机 和 一个 Empty 对象， **其他对象必须代码动态生成！！！** 。 整个游戏不许出现 Find 游戏对象， SendMessage 这类突破程序结构的**通讯耦合**语句。 **违背本条准则，不给分**。
> - 请使用课件架构图编程，**不接受非 MVC 结构程序**。
> - 注意细节，例如：船未靠岸，牧师与魔鬼上下船运动中，均不能接受用户事件！

**项目地址**：[github.com/Jiahonzheng/Unity-3D-Learning](https://github.com/JIahonzheng/Unity-3D-Learning/tree/HW2/HW2/Priests-and-Devils)。

**在线演示**：[demo.jiahonzheng.cn/Priests-and-Devils](https://demo.jiahonzheng.cn/Priests-and-Devils/) 。

**演示视频**：[Unity 牧师与魔鬼](https://www.bilibili.com/video/av68507262/) 。

#### GameObject

游戏涉及到的游戏对象（`GameObject`）：Priests、Devils、Boat、Coast、River 。

#### 玩家动作表

由于游戏规则并不算复杂，我们可以得到以下简单的玩家动作表。

| 动作                | 结果                               |
| ------------------- | ---------------------------------- |
| 点击 Character 人物 | 对应人物上船（离岸）、下船（上岸） |
| 点击 Boat 船只      | 船只运动（过河）                   |

#### Prefabs

游戏对象及其预设的关系如下。

| GameObject  | Prefab 名称 | 3D Object 类型 |
| ----------- | ----------- | -------------- |
| Priest 牧师 | Priest      | Cube           |
| Devil 魔鬼  | Devil       | Sphere         |
| Boat 船只   | Boat        | Cube           |
| Coast 河岸  | Stone       | Cube           |
| River 河流  | Water       | Cube           |

为了使得游戏对象变得更为美观，我们为其添加了 `Material` 贴图，以下是该游戏的预设资源预览。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%A9%BA%E9%97%B4%E4%B8%8E%E8%BF%90%E5%8A%A8_7.jpg)

#### 基于职责的设计

设计一个游戏如同组织一场话剧，既然要搞话剧或游戏，就至少需要以下角色（**划分职责**）：

- 导演，1 名（仅要一个）。
  - 具体类型：`Director` 。
  - 职责：把握全局，控制场景的切换。
- 场记若干，话剧有很多场，每场需要一个。
  - 抽象类型：`ISceneController` 。
  - 职责：每一场的场记，控制布景、演员的上下场、管理动作等执行。
- 吃瓜群众，1 个。
  - 抽象类型：`IUserAction` 。
  - 职责：边吃瓜子边和场记聊天。

我们使用**面向对象**技术设计游戏，其核心是基于职责的设计。为此，我们设计 `Director` 类，其职责大致如下：

- 获取当前游戏的场景
- 管理游戏全局状态

```csharp
namespace PriestsAndDevils
{
    public class Director : System.Object
    {
        // Singlton instance.
        private static Director instance;
        public ISceneController currentSceneController { get; set; }
        public bool running { get; set; }
        public int fps
        {
            get
            {
                return Application.targetFrameRate;
            }
            set
            {
                Application.targetFrameRate = value;
            }
        }

        public static Director GetInstance()
        {
            return instance ?? (instance = new Director());
        }
    }
}
```

由于本游戏场景不多，我们只有一位场记 `GameController` ，它通过 `ISceneController` 接口与导演 `Director` 交互。与此同时，我们玩家（“吃瓜群众”），需要与 `GameController` 互动，因此我们设计了 `IUserAction` 用户交互接口，也实现了用户行为与游戏系统规则计算的分离，实现**解耦合**。

以下是 `ISceneController` 和 `IUserAction` 接口的代码实现。

```csharp
namespace PriestsAndDevils
{
    public interface ISceneController
    {
        void LoadResources();
    }

    public interface IUserAction
    {
        void ClickBoat();
        void ClickCharacter(CharacterController c);
        void Reset();
    }
}
```

以下是 `GameController` 的整体代码实现（有删改）。

```csharp
namespace PriestsAndDevils
{
    public class GameController : MonoBehaviour, ISceneController, IUserAction
    {
        void Awake()
        {
            // Set the current scene controller.
            Director director = Director.GetInstance();
            director.CurrentSceneController = this;
            // Add GUI.
            gui = gameObject.AddComponent<GameGUI>() as GameGUI;
            // Load the resources.
            LoadResources();
        }

        // Load the resources.
        public void LoadResources()
        {
            GenGameObjects();
        }

        // It is called when player clicks the boat.
        public void ClickBoat()
        {
            // ......
        }

        // It is called when player clicks a character.
        public void ClickCharacter(CharacterController character)
        {
            // ......
        }

        // It is called when player resets the game.
        public void Reset()
        {
            // ......
        }
    }
}
```

我们注意到，在**场景**被加载（`awake`）时，它也会自动注入**导演**，设置当前场景。

#### MVC

MVC 是界面人机交互程序设计的一种架构模式，它把程序分为三个部分：

- 模型（`Model`）
  - 管理游戏对象、空间关系。
- 控制器（`Controller`）
  - 一个场景一个主控制器。
  - 至少实现与玩家交互的接口（`IUserAction`），接受用户事件，控制 `Model` 的变化。
  - 实现、管理 `GameObject` 的运动。
- 界面（`View`）
  - 渲染视图，接收并转发用户事件（点击等）至 `Controller` 处理。

##### Model

在实现中，我设计并实现了这些 `Model` ：`Character` 、`Boat` 和 `Coast` 。

在 `Character` 中，我们维护牧师和魔鬼的 `name` 、 `Location` 和 `isOnboard` 。

```csharp
// 用于描述游戏对象的位置：位于左岸、位于右岸。
public enum Location { Left, Right };
```

在 `Boat` 和 `Coast` 中，我们除了维护其 `name` 和 `Location` 信息，还维护了**空位**和**乘客**信息，具体请参考代码实现。

##### Controller

针对每一个 `Model` ，我实现了对应的 `Controller` ，用于控制对应游戏对象的运动，值得注意的是，它们都继承于 `Moveable` 类。

- `Moveable`
  - `SetDestination`：使对应游戏对象运动至指定位置。
  - `Reset`：在重置游戏时使用。
- `CharacterController`
  - `GoAboard`：当玩家点击牧师或魔鬼，使之上船时，被调用。
  - `GoAshore`：当玩家点击牧师或魔鬼，使之上岸时，被调用。
- `BoatController`
  - `Move`：当玩家点击船只，使之运动时，被调用。
  - `GoAboard`：配合 `CharacterController` 的 `GoAboard` 使用。
  - `GoAshore`：配合 `CharacterController` 的 `GoAshore` 使用。
- `CoastController`
  - `GoAboard`：配合 `CharacterController` 的 `GoAboard` 使用。
  - `GoAshore`：配合 `CharacterController` 的 `GoAshore` 使用。

在 `GameController` 中，我们通过调用子控制器，实现对游戏对象的运动控制。在 `GameController` 中，我使用了**集合数据类型**来管理游戏对象。

```csharp
public class GameController : MonoBehaviour, ISceneController, IUserAction
{
    public CoastController leftCoast;
    public CoastController rightCoast;
    public BoatController boat;
    // 使用 集合数据类型 管理游戏对象。
    public  List<CharacterController> characters = new List<CharacterController>(6);
    private GameGUI gui;
    // ......
}
```

为了更好地表示和管理游戏状态，我们引入了 `Result` 的枚举数据结构。

```csharp
// It represents the result of the game.
public enum Result
{
    NOT_FINISHED,
    WINNER,
    LOSER,
}
```

在 `GameController` 中，我们对用户的点击事件，作出响应，最终对游戏进行胜负判断，以下是 `CheckWinner` 的代码实现。

```csharp
// It determines whether the player wins the game.
private Result CheckWinner()
{
    Result result = Result.NOT_FINISHED;
    // Calculate the amount.
    int leftPriests = leftCoast.model.GetCharacterAmount()[0];
    int leftDevils = leftCoast.model.GetCharacterAmount()[1];
    int rightPriests = rightCoast.model.GetCharacterAmount()[0];
    int rightDevils = rightCoast.model.GetCharacterAmount()[1];

    // When all the characters has gone across the river.
    if (leftPriests + leftDevils == 6)
    {
        result = Result.WINNER;
    }
    // When the boat is on the right side.
    if (boat.location == Location.Right)
    {
        rightPriests += boat.model.GetCharacterAmount()[0];
        rightDevils += boat.model.GetCharacterAmount()[1];
    }
    else // When the boat is on the left side.
    {
        leftPriests += boat.model.GetCharacterAmount()[0];
        leftDevils += boat.model.GetCharacterAmount()[1];
    }
    // In this case, player lose the game.
    if ((rightPriests < rightDevils && rightPriests > 0) ||
        (leftPriests < leftDevils && leftPriests > 0))
    {
        result = Result.LOSER;
    }
    return result;
}
```

##### View

我们使用 `GameGUI` 作为 `View` 。在 `Start` 函数中，我们通过 `Director` 的全局单例，获取到了当前的视图控制器，即 `GameController` 。在 `OnGUI` 函数中，我们渲染视图；在 `OnUpdate` 函数中，我们监听捕捉用户的鼠标左键（`Fire1`）的点击事件，最后转发至 `Controller` 处理事件。

```csharp
namespace PriestsAndDevils
{
    public class GameGUI : MonoBehaviour
    {
        public Result result;
        private IUserAction action;

        // Use this for initialization
        void Start()
        {
            result = Result.NOT_FINISHED;
            action = Director.GetInstance().CurrentSceneController as IUserAction;
        }

        void OnGUI()
        {
            // ......
        }

        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    var todo = hit.collider;
                    var character = todo.GetComponent<CharacterController>();
                    if (character)
                    {
                        action.ClickCharacter(character);
                    }
                    else if (todo.transform.name == "Boat")
                    {
                        action.ClickBoat();
                    }
                }
            }
        }
    }
}
```

为了获取用户所点击的 `GameObject` ，我们使用了**射线捕捉**技术：连接摄像机和用户在 `ViewPort` 所点击的点，构造一条射线，然后射线所照射的对象，即为我们想要获取的游戏对象。

#### GenGameObjects

我们在 `GameController` （与课件的 `FirstController` 对应）实现了 `ISceneController` 接口，实现了 `LoadResources` 方法。

```csharp
// Load the resources.
public void LoadResources()
{
    GenGameObjects();
}
```

在该方法中，我们调用了 `GenGameObjects` 方法，来生成各种 `GameObject` ，并为其添加对应的 `MonoBehaviour` 。

```csharp
// It generates the GameObjects.
private void GenGameObjects()
{
    // Generate River.
    {
        GameObject temp = Utils.Instantiate("Prefabs/Water", new Vector3(0, 0.5f, 0));
        temp.name = "River";
    }
    // Generate LeftCoast.
    {
        GameObject temp = Utils.Instantiate("Prefabs/Stone", Coast.departure);
        leftCoast = temp.AddComponent<CoastController>();
        temp.name = leftCoast.name = "LeftCoast";
        leftCoast.location = Location.Left;
    }
    //  Generate RightCoast.
    {
        GameObject temp = Utils.Instantiate("Prefabs/Stone", Coast.destination);
        rightCoast = temp.AddComponent<CoastController>();
        temp.name = rightCoast.name = "RightCoast";
        rightCoast.location = Location.Right;
    }
    // Generate Boat.
    {
        GameObject temp = Utils.Instantiate("Prefabs/Boat", Boat.departure);
        boat = temp.AddComponent<BoatController>();
        temp.name = boat.name = "Boat";
    }
    // Generate Priests.
    for (int i = 0; i < 3; ++i)
    {
        GameObject temp = Utils.Instantiate("Prefabs/Priest", Coast.destination);
        characters[i] = temp.AddComponent<CharacterController>();
        temp.name = characters[i].name = "Priest" + i;
        characters[i].GoAshore(rightCoast);
    }
    // Generate Devils.
    for (int i = 0; i < 3; ++i)
    {
        GameObject temp = Utils.Instantiate("Prefabs/Devil", Coast.destination);
        characters[i + 3] = temp.AddComponent<CharacterController>();
        temp.name = characters[i + 3].name = "Devil" + i;
        characters[i + 3].GoAshore(rightCoast);
    }
}
```

#### 演示

**在线演示**：[demo.jiahonzheng.cn/Priests-and-Devils](https://demo.jiahonzheng.cn/Priests-and-Devils/) 。

**演示视频**：[Unity 牧师与魔鬼](https://www.bilibili.com/video/av68507262/) 。

以下是游戏初始页面。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%A9%BA%E9%97%B4%E4%B8%8E%E8%BF%90%E5%8A%A8_4.png)

以下是玩家胜利的页面。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%A9%BA%E9%97%B4%E4%B8%8E%E8%BF%90%E5%8A%A8_5.png)

以下是玩家失败的页面。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%A9%BA%E9%97%B4%E4%B8%8E%E8%BF%90%E5%8A%A8_6.png)

### Question 3.1

> 使用向量与变换，实现并扩展 Tranform 提供的方法，如 Rotate、RotateAround 等

为实现 `Rotate` 方法，我们先生成围绕 `axis` 旋转 `angle` 度的四元数，随后将其与 `t.position` 和 `t.rotation` 点乘。

```csharp
// Let t rotate with specific axis and angle.
void Rotate(Transform t, Vector3 axis, float angle)
{
	var rot = Quaternion.AngleAxis(angle, axis);
    t.position = rot * t.position;
    t.rotation *= rot;
}
```

为实现 `RotateAround` 方法，我们先获取 `t.position` 和 `center` 的差，然后对其进行 `Rotate` 变换，最后在 `center` 上加上变换后的差，即可获得 `t` 变换后的位置。当然我们还需要对 `t.rotation` 进行 `Rotate` 变换。

```csharp
// Let t rotate around center with specific axis and angle.
void RotateAround(Transform t, Vector3 center, Vector3 axis, float angle)
{
	var position = t.position;
    var rot = Quaternion.AngleAxis(angle, axis);
    var direction = position - center;
    direction = rot * direction;
    t.position = center + direction;
    t.rotation *= rot;
}
```
