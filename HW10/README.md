# AR 技术的简单尝试

项目地址：[github.com/Jiahonzheng/Unity-3D-Learning](https://github.com/Jiahonzheng/Unity-3D-Learning/tree/HW10/HW10)

视频地址：[www.bilibili.com/video/av80020277/](https://www.bilibili.com/video/av80020277/)

课程讲义地址：[https://pmlpml.github.io/unity3d-learning/12-AR-and-MR](https://pmlpml.github.io/unity3d-learning/12-AR-and-MR)

## 环境配置

在完成本次作业时，我在**环境配置**部分耽误了很多时间，以下是我最终的配置：

+ Windows 10 1903
+ Unity 2019.2.17.f1
+ [Vuforia Engine 8.6]()

### 设置 Unity

首先，我们需要开启 Player Settings 选项卡中的 Vuforia AR 支持。

```
File -> Build Settings -> Player Settings -> Player -> Vuforia Argumented Reality Support
```

最终，设置结果如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_1.png)

### SDK 下载与安装

随后，我们需要在[官网](https://developer.vuforia.com/downloads/sdk) 下载 SDK 文件，该文件格式为 Unity Package 。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_2.png)

随后，我们需要在 Unity 导入该 Package 。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_3.png)

注意：**导入 Package 时，最好使用 Proxifier 等全局代理工具！**

## 图片识别与建模

### 创建数据库

根据[课程讲义]()内容，我们需要在 Vuforia 平台上，上传图片，创建相应的数据库。首先，我们创建一个名为 3D_Course 的数据库，类型设置为 Device 。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_5.png)

随后，我们需要在此数据库中上传 dragon.jpg 图片。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_7.jpg)

具体上传设置如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_6.png)

### 导出数据库

在创建完数据库后，我们需要将其导出至 Unity Package 文件，具体操作如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_4.png)

### 导入 License

我们需要在 https://developer.vuforia.com/vui/develop/licenses 页面中获取 License 。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_9.png)

随后，我们在 `VuforiaConfiguration` 中添加所复制的 License 。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_8.png)

### ImageTarget

我们在 Scene 场景中，移除原有的 Main Camera 组件，然后添加 Vuforia Engine 中的 ARCamera 组件和 ImageTarget 组件（即下图菜单中的 Image 组件）。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_10.png)

现在，**我们需要在 Unity 中导入用户自定义的 3D_Course Vuforia 数据库**。然后我们需要在 ImageTarget 组件的 Inspector 栏中，进行以下的设置：

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_11.png)

最后，我们导入 VoxelCharacters 中的 Soldier 预制至 ImageTarget 中，使得 ARCamera 在捕捉到 Dragon 图片时，可显示 Soldier 人物。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_13.png)

## 虚拟按钮

现在，我们需要添加三个虚拟按钮，分别来实现 Soldier 人物在**空闲**、**行走**和**跳舞**三个状态中的切换，通过点击 ImageTarget 的 Inspector 菜单中的 Add Virtual Button 按钮，我们即可创建**虚拟按钮**。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_12.png)

在创建虚拟按钮后，我们需要分别为每个按钮添加 Plane 组件，**否则无法在屏幕中显示**。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_14.png)

同时，我们需要为每个虚拟按钮设置不同的 name 名称，**否则无法按照预期响应按钮事件**。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_15.png)

### 人物动画

我们编写了以下的 Animation Controller ，其中的 Dance 和 Walk 动画来自于 VoxelCharacters 素材包。我们使用两个布尔变量 `isDancing` 和 `isWalking` 来控制人物的不同动画。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_16.png)

### 注册按钮响应事件

为实现虚拟按钮与人物动画之间的控制关系，我们需要为 ImageTarget 添加 VirtualButton 脚本。首先，我们需要在 `Start` 函数中，获取 ImageTarget 组件中所有的虚拟按钮对象，然后依次对每个按钮注册**按钮响应事件**。

```csharp
void Start()
{
    // 获取 ImageTarget 下的所有虚拟按钮对象。
    var vbbs = GetComponentsInChildren<VirtualButtonBehaviour>();
    // 为每个虚拟按钮注册按钮响应事件。
    for (int i = 0; i < vbbs.Length; ++i)
    {
        vbbs[i].RegisterOnButtonPressed(OnButtonPressed);
        vbbs[i].RegisterOnButtonReleased(OnButtonReleased);
    }
}
```

当 `ButtonPressed` 事件被触发时，会调用 `OnButtonPressed` 函数：根据不同的虚拟按钮名称，我们实现了动画的不同控制逻辑。

```csharp
public void OnButtonPressed(VirtualButtonBehaviour vb)
{
    // 根据虚拟按钮的名称，分发不同的动画控制逻辑。
    switch (vb.VirtualButtonName)
    {
        // 实现跳舞动画。
        case "DanceButton":
            animator.SetBool("isWalking", false);
            animator.SetBool("isDancing", true);
            break;
        // 实现行走动画。
        case "WalkButton":
            animator.SetBool("isDancing", false);
            animator.SetBool("isWalking", true);
            break;
        // 实现空闲动画。
        default:
            animator.SetBool("isWalking", false);
            animator.SetBool("isDancing", false);
            break;
    }
}
```

由于我们不需要在 `ButtonReleased` 事件发生时，执行业务逻辑，因此我们的 `OnButtonReleased` 函数为空函数。

以下是 `VirtualButton.cs` 的完整代码。

```csharp
using UnityEngine;
using Vuforia;

public class VirtualButton : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        // 获取 ImageTarget 下的所有虚拟按钮对象。
        var vbbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        // 为每个虚拟按钮注册按钮响应事件。
        for (int i = 0; i < vbbs.Length; ++i)
        {
            vbbs[i].RegisterOnButtonPressed(OnButtonPressed);
            vbbs[i].RegisterOnButtonReleased(OnButtonReleased);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        // 根据虚拟按钮的名称，分发不同的动画控制逻辑。
        switch (vb.VirtualButtonName)
        {
            // 实现跳舞动画。
            case "DanceButton":
                animator.SetBool("isWalking", false);
                animator.SetBool("isDancing", true);
                break;
            // 实现行走动画。
            case "WalkButton":
                animator.SetBool("isDancing", false);
                animator.SetBool("isWalking", true);
                break;
            // 实现空闲动画。
            default:
                animator.SetBool("isWalking", false);
                animator.SetBool("isDancing", false);
                break;
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
    }
}
```

## 界面演示

视频地址：[www.bilibili.com/video/av80020277/](https://www.bilibili.com/video/av80020277/)

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/AR%E6%8A%80%E6%9C%AF%E7%9A%84%E7%AE%80%E5%8D%95%E5%B0%9D%E8%AF%95_17.png)