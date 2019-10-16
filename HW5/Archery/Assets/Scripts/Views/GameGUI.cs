using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour
{
    private GameObject canvasObject;
    private GameObject scoreObject;
    private GameObject tipsObject;

    void Awake()
    {
        canvasObject = Instantiate(Resources.Load<GameObject>("Prefabs/Canvas"));
        scoreObject = Instantiate(Resources.Load<GameObject>("Prefabs/Score"), canvasObject.transform);
        tipsObject = Instantiate(Resources.Load<GameObject>("Prefabs/Tips"), canvasObject.transform);
        // 显示分数。
        ShowScore(0);
        // 隐藏提示。
        tipsObject.SetActive(false);
    }

    // 显示分数。
    public void ShowScore(int score)
    {
        scoreObject.GetComponent<Text>().text = "Score: " + score;
    }

    // 显示命中环数。
    public void ShowTips(int point)
    {
        var tips = point == 0 ? "Try Again!" : point + " Points!";
        tipsObject.GetComponent<Text>().text = tips;
        tipsObject.SetActive(true);
        StartCoroutine(WaitForTipsDisappear());
    }

    private IEnumerator WaitForTipsDisappear()
    {
        yield return new WaitForSeconds(0.5f);
        tipsObject.SetActive(false);
    }
}
