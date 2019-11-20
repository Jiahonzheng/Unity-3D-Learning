using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
