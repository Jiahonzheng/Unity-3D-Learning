using UnityEngine;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    // num 指消息总数。
    public int num = 6;
    // 列表布局。
    private RectTransform content;

    void Start()
    {
        content = GetComponent<RectTransform>();
        // 加载列表资源。
        LoadResources();
    }

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
}
