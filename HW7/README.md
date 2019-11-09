# 粒子系统

## 粒子系统本质

**粒子系统**是模拟一些不确定、流动现象的技术，它采用许多形状简单且赋予生命的微小粒子作为基本元素来表示物体(一般由点或很小的多边形通过纹理贴图表示)，表达物体的总体形态和特征的动态变化。人们经常使用粒子系统模拟的现象有火、爆炸、烟、水流、火花、落叶、云、雾、雪、尘、流星尾迹或者象发光轨迹这样的抽象视觉效果等等。

 作为粒子系统，每个粒子运动一般具有简单的数学模型和它们之间具有自形似的运动过程。通过引入特定的随机分布作用于粒子，使得系统整体呈现复杂的现象，这是粒子系统的本质。 

## 汽车尾气模拟

### 设置粒子效果

首先，我们导入 Standard Assets 素材包，实例化其中的 Car 预制，效果如下图。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%B2%92%E5%AD%90%E7%B3%BB%E7%BB%9F_3.png)

我们复制其自带的 `ParticleBurnoutSmoke` 为 `SmokeLeft` 和 `SmokeRight` ，作为左右排气管的尾气粒子系统。在右侧的 Inspector 菜单，我们设置 `Force over Lifetime` 和 `Size over Lifetime` 属性，分别设置粒子的运动方向和粒子群的大小变化情况，具体设置如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%B2%92%E5%AD%90%E7%B3%BB%E7%BB%9F_2.png)

另外，为了模拟汽车在故障情况下的尾气效果，我们需要替换粒子的 `Material` 为 `ParicleSmokeBlack` ，否则粒子一直呈现白色，难以模拟故障效果。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%B2%92%E5%AD%90%E7%B3%BB%E7%BB%9F_1.png)

粒子释放效果如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%B2%92%E5%AD%90%E7%B3%BB%E7%BB%9F_4.png)

### 构建地图

官方素材提供的 Car 的运动是需要较大的运动空间，并且我们需要模拟车辆与障碍物的碰撞，在这里，我们采用了代码生成地图场景的方案，具体实现位于 `MainController` 类。在该类的 `Awake` 方法中，我们调用了 `LoadResources` 方法，用于生成地图场景和车辆，其具体代码如下。

```csharp
// 装载地图资源。
private void LoadResources()
{
    // 加载地图平面。
    var plane = Instantiate(Resources.Load<GameObject>("Prefabs/Plane"));
    plane.name = "Plane";

    // 加载车辆资源。
    car = Instantiate(Resources.Load<GameObject>("Prefabs/Car"));
    car.name = "Car";

    // 加载边界预制。
    var wallPrefab = Resources.Load<GameObject>("Prefabs/Wall");
    // 绘制地图边界。
    {
        GameObject wall = Instantiate(wallPrefab);
        wall.transform.position = new Vector3(-25, 2, 0);
        wall.transform.localScale = new Vector3(1, 4, 100);
    }
    {
        GameObject wall = Instantiate(wallPrefab);
        wall.transform.position = new Vector3(25, 2, 0);
        wall.transform.localScale = new Vector3(1, 4, 100);
    }
    {
        GameObject wall = Instantiate(wallPrefab);
        wall.transform.position = new Vector3(0, 2, 50);
        wall.transform.localScale = new Vector3(50, 4, 1);
    }
    {
        GameObject wall = Instantiate(wallPrefab);
        wall.transform.position = new Vector3(0, 2, -50);
        wall.transform.localScale = new Vector3(50, 4, 1);
    }
}
```

该函数构建的地图场景如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%B2%92%E5%AD%90%E7%B3%BB%E7%BB%9F_5.png)

### 粒子速度

我们现在需要实现这个一个需求：当引擎转速越高，尾气粒子的运动速度也越快。首先，我们如何获取车辆的运行速度？游戏对象 Car 绑定了 `CarController` 脚本，其含有 `Revs` 值，即引擎转速值。根据此信息，我们实现了 `SetEmissionRate` 函数，用于根据引擎转速设置粒子速度。

```csharp
// 根据引擎转速设置粒子释放速率。
private void SetEmissionRate()
{
    // 比例系数。
    var K = 5000;
    // 注意：若使用 exhaust.emission.rateOverTime = K * carController.Revs; 会返回语法错误。
    var emission = exhaust.emission;
    emission.rateOverTime = K * carController.Revs;
}
```

### 碰撞检测

根据实验要求，我们需要模拟汽车在故障后的尾气效果，我们可以为 Car 添加 Collider 脚本，用于在车辆碰撞时，增加其损坏值，该值控制粒子颜色的深浅，即损坏值越大，尾气粒子颜色越深。

我们编写了 `CarCollider` 脚本，在 `Car` 游戏对象添加该脚本，即可实现对车辆损坏值的记录和更新。

```csharp
public class CarCollider : MonoBehaviour
{
    // 记录车辆损坏情况。
    private float damage = 0;

    // 返回车辆损坏情况。
    public float GetDamage()
    {
        return damage;
    }

    // 设置车辆损坏情况。
    public void SetDamage(float d)
    {
        damage = d;
    }

    // 当车辆与墙体碰撞时，增加其损坏系数。
    private void OnCollisionEnter(Collision collision)
    {
        damage += 4.5f * gameObject.GetComponent<Rigidbody>().velocity.magnitude;
    }
}
```

在获取到车辆损坏值后，我们需要根据该值设置尾气粒子的颜色深浅，具体实现位于 `SetColor` 函数中。我们使用 `Gradient` 类实现损坏值和尾气颜色的**正相关**关系：损坏程度越高，尾气颜色越深。

```csharp
// 根据车辆损坏程度设置粒子颜色。
private void SetColor()
{
    // 获取粒子颜色句柄。
    var color = exhaust.colorOverLifetime;
    // 获取车辆损坏情况。
    var damage = car.GetComponent<CarCollider>().GetDamage();
    // 根据损坏情况设置颜色深浅，损坏越严重，颜色越深。
    var gradient = new Gradient();
    var colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(new Color(214, 189, 151), 0.079f), new GradientColorKey(Color.white, 1.0f) };
    var alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(damage / 255f + 10f / 255f, 0.061f), new GradientAlphaKey(0.0f, 1.0f) };
    gradient.SetKeys(colorKeys, alphaKeys);
    color.color = gradient;
}
```

### 辅助功能

经过上述流程，我们实现了尾气粒子运动速度随引擎转速的提高而增大，实现了尾气颜色随损坏程度的提高而变得深色（黑色）。为了便于测试，我在游戏实现中添加了**辅助按钮**，用于快速设置车辆的损坏情况。

```csharp
// 显示损坏设置按钮。
void OnGUI()
{
    var labelStyle = new GUIStyle() { fontSize = 40 };
    var buttonStyle = new GUIStyle("button") { fontSize = 30 };
    GUI.Label(new Rect(160, 30, 200, 100), "Damage", labelStyle);
    // 设置为 0 损坏。
    if (GUI.Button(new Rect(160, 80, 100, 50), "0%", buttonStyle))
    {
        car.GetComponent<CarCollider>().SetDamage(0);
    }
    // 设置为 50% 损坏。
    if (GUI.Button(new Rect(160, 130, 100, 50), "50%", buttonStyle))
    {
        car.GetComponent<CarCollider>().SetDamage(100);
    }
    // 设置为 100% 损坏。
    if (GUI.Button(new Rect(160, 180, 100, 50), "100%", buttonStyle))
    {
        car.GetComponent<CarCollider>().SetDamage(200);
    }
}
```

辅助功能的显示效果如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%B2%92%E5%AD%90%E7%B3%BB%E7%BB%9F_6.png)

### 实现代码

由于篇幅限制，具体实现代码参照 GitHub 链接：[github.com/Jiahonzheng/Unity-3D-Learning]( https://github.com/Jiahonzheng/Unity-3D-Learning/tree/HW7/HW7 )

### 游戏演示

在线演示：[demo.jiahonzheng.cn/Smoke](https://demo.jiahonzheng.cn/Smoke)

演示视频：[www.bilibili.com/video/av75085474/]( https://www.bilibili.com/video/av75085474/ )

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/%E7%B2%92%E5%AD%90%E7%B3%BB%E7%BB%9F_6.png)