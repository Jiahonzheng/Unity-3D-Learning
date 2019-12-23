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