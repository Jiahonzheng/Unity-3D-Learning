using UnityEngine;
using Vuforia;

public class VirtualButton : MonoBehaviour
{
    public VirtualButtonBehaviour[] vbbs;
    public Animator animator;

    void Start()
    {
        vbbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbbs.Length; ++i)
        {
            vbbs[i].RegisterOnButtonPressed(OnButtonPressed);
            vbbs[i].RegisterOnButtonReleased(OnButtonReleased);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        switch (vb.VirtualButtonName)
        {
            case "DanceButton":
                animator.SetBool("isWalking", false);
                animator.SetBool("isDancing", true);
                break;
            case "WalkButton":
                animator.SetBool("isDancing", false);
                animator.SetBool("isWalking", true);
                break;
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