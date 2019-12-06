# UI 系统：公告牌

博客链接：[Unity 公告牌](https://blog.jiahonzheng.cn/2019/11/21/Unity 实现公告牌/)

## 设计要求

+ 使用 UGUI 技术实现**公告牌**。

## ScrollView

首先，我们在 Scene 场景中，添加 ScrollView 游戏对象，并设置其 Inspector 属性，具体设置如下。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/Unity%20%E5%AE%9E%E7%8E%B0%E5%85%AC%E5%91%8A%E7%89%8C_2.png)

在上述设置中，我们**取消了滚动视图的水平滚动**，同时**设置竖直滚动条永远可见**。此时，对象层次结构如下图所示。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/Unity%20%E5%AE%9E%E7%8E%B0%E5%85%AC%E5%91%8A%E7%89%8C_4.png)



其中的 `Content` 存放着列表内容，我们需要对其设置，具体设置如下。

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/Unity%20%E5%AE%9E%E7%8E%B0%E5%85%AC%E5%91%8A%E7%89%8C_3.png)

我们为 `Content` 添加了 `Verticle Layout Group` 和 `Content Size Fitter` 组件，前者用于**构建垂直列表视图**，后者用于**实现列表高度的自适应**。注意到，我们在 `Verticle Layout Group` 中设置了 `Child Controls Size` 和 `Child Force Expand` 属性，这使得我们为列表添加内容时，无需手动为内容进行布局设定。

## 预制

构建完垂直列表视图后，我们需要制作**消息标题**和**消息主体**的预制，用于列表项的显示。

我们使用 Button 和 Text 分别构建消息标题和消息主体，由于前面我们设置了 `Child Controls Size` 属性，因此我们只需设置 Button 和 Text 的高度，即可完成对应预制的制作。

## 列表展开和关闭

我们为 `Content` 绑定 `Content.cs` 脚本，脚本中的 `LoadResources` 函数实现了列表项的渲染：实例化 Button 和 Text 预制，将其追加至 Content 的子节点中，实现列表项（**消息标题**和**消息主体**）的添加与渲染。

```csharp
// 加载列表资源。
void LoadResources()
{
    for (int i = 0; i < num; ++i)
    {
        // 设置消息标题。
        Button title = Instantiate(Resources.Load<Button>("Prefabs/Button"));
        title.name = "Message Title " + (i + 1);
        title.GetComponentInChildren<Text>().text = "Message Title " + (i + 1);
        // 添加 Button 至消息列表。
        title.transform.SetParent(transform, false);

        // 设置消息主体。
        Text body = Instantiate(Resources.Load<Text>("Prefabs/Text"));
        body.name = "Message Body " + (i + 1);
        body.text = "Hello, this is Message " + (i + 1) + ".";
        // 添加 Text 至消息列表。
        body.transform.SetParent(transform, false);
        body.gameObject.SetActive(false);

        // 设置 Button 响应点击事件。
        ButtonHandler handler = title.gameObject.AddComponent<ButtonHandler>();
        handler.text = body;
    }
}
```

在添加列表项时，我们为 Button 消息标题添加了 `ButtonHandler` 脚本，用于**响应点击事件**，**实现列表项的展开与关闭**，具体代码如下。

```csharp
public class ButtonHandler : MonoBehaviour
{
    // 消息主体。
    public Text text;

    void Start()
    {
        // 绑定响应点击事件。
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // 响应点击事件。
    void OnClick()
    {
        // 当消息主体已被展开。
        if (text.gameObject.activeSelf)
        {
            text.gameObject.SetActive(false);
        }
        else // 当消息主体未被展开。
        {
            text.gameObject.SetActive(true);
        }
    }
}
```

在 `OnClick` 函数中，我们根据当前 Text 消息主体的 `IsActive` 属性，来实现列表项的展开或关闭效果。

## 添加动画

为了实现列表展开和关闭的动画效果，我们需要在 `Start` 函数中获取 Text 的宽度和高度。

```csharp
void Start()
{
    // 绑定响应点击事件。
    GetComponent<Button>().onClick.AddListener(OnClick);
    // 记录消息主体 Text 的高度。
    textHeight = text.rectTransform.sizeDelta.y;
    // 记录消息主体 Text 的宽度。
    textWidth = text.rectTransform.sizeDelta.x;
}
```

我们使用**协程**实现动画效果，在 `CloseText` 函数中实现列表项的关闭动画，在 `OpenText` 函数中实现列表项的展开动画。

在 `CloseText` 函数中，我们先计算出 Text 高度的缩放速度，随后使用 `for` 循环实现动画的**帧绘制**逻辑。在每一帧中，我们更新当前旋转角度和 Text 高度，最后在动画结束时，我们设置 Text 对象为**不可用**状态。

```csharp
// 播放关闭消息主体动画。
private IEnumerator CloseText()
{
    // 设置旋转角度的初始值和旋转速度。
    float angleX = 0;
    float angleSpeed = 90f / frame;
    // 设置 Text 初始高度和高度缩放速度。
    float height = textHeight;
    float heightSpeed = textHeight / frame;

    // 执行动画。
    for (int i = 0; i < frame; ++i)
    {
        // 更新旋转角度。
        angleX -= angleSpeed;
        // 更新 Text 高度。
        height -= heightSpeed;
        // 应用新的旋转角度和高度。
        text.transform.rotation = Quaternion.Euler(angleX, 0, 0);
        text.rectTransform.sizeDelta = new Vector2(textWidth, height);
        // 结束动画。
        if (i == frame - 1)
        {
            text.gameObject.SetActive(false);
        }
        yield return null;
    }
}
```

函数 `OpenText` 的执行逻辑与函数 `CloseText` 逻辑相似，这里不做过多阐述，其具体代码如下。

```csharp
// 播放打开消息主体动画。
private IEnumerator OpenText()
{
    // 设置旋转的初始值和旋转速度。
    float angleX = -90f;
    float angleSpeed = 90f / frame;
    // 设置 Text 初始高度和高度缩放速度。
    float height = 0;
    float heightSpeed = textHeight / frame;

    // 执行动画。
    for (int i = 0; i < frame; ++i)
    {
        // 更新旋转角度。
        angleX += angleSpeed;
        // 更新 Text 高度。
        height += heightSpeed;
        // 应用新的旋转角度和高度。
        text.transform.rotation = Quaternion.Euler(angleX, 0, 0);
        text.rectTransform.sizeDelta = new Vector2(textWidth, height);
        // 结束动画。
        if (i == 0)
        {
            text.gameObject.SetActive(true);
        }
        yield return null;
    }
}
```

最后，我们需要更新 `OnClick` 函数的执行逻辑。

```csharp
// 响应点击事件。
void OnClick()
{
    // 当消息主体已被展开，播放关闭动画。
    if (text.gameObject.activeSelf)
    {
        StartCoroutine("CloseText");
    }
    else // 当消息主体未被展开，播放展开动画。
    {
        StartCoroutine("OpenText");
    }
}
```

## 实现代码

由于篇幅限制，具体实现代码可参照 GitHub 链接：[github.com/Jiahonzheng/Unity-3D-Learning](https://github.com/Jiahonzheng/Unity-3D-Learning/tree/HW8/HW8) 。

## 游戏演示

在线演示：[demo.jiahonzheng.cn/Billboard](https://demo.jiahonzheng.cn/Billboard/)

演示视频：[www.bilibili.com/video/av76431719](https://www.bilibili.com/video/av76431719/)

![](https://jiahonzheng-blog.oss-cn-shenzhen.aliyuncs.com/Unity%20%E5%AE%9E%E7%8E%B0%E5%85%AC%E5%91%8A%E7%89%8C_1.png)