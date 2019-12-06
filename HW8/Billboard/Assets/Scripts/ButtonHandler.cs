using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    // 消息主体。
    public Text text;
    // 动画帧率。
    public int frame = 5;
    // 消息主体 Text 的高度。
    private float textHeight;
    // 消息主体 Text 的宽度。
    private float textWidth;

    void Start()
    {
        // 绑定响应点击事件。
        GetComponent<Button>().onClick.AddListener(OnClick);
        // 记录消息主体 Text 的高度。
        textHeight = text.rectTransform.sizeDelta.y;
        // 记录消息主体 Text 的宽度。
        textWidth = text.rectTransform.sizeDelta.x;
    }

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
}
