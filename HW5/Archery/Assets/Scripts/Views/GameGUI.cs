using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Archery {
    public class GameGUI : MonoBehaviour
    {
        private GameObject canvasObject;
        private GameObject scoreObject;
        private GameObject tipsObject;
        private GameObject windObject;

        void Awake()
        {
            canvasObject = Instantiate(Resources.Load<GameObject>("Prefabs/Canvas"));
            scoreObject = Instantiate(Resources.Load<GameObject>("Prefabs/Score"), canvasObject.transform);
            tipsObject = Instantiate(Resources.Load<GameObject>("Prefabs/Tips"), canvasObject.transform);
            windObject = Instantiate(Resources.Load<GameObject>("Prefabs/Wind"), canvasObject.transform);
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

        public void ShowWind(GameModel.Wind wind)
        {
            windObject.GetComponent<Text>().text = "Wind: " + wind.text + " " + wind.strength;
        }
    }
}