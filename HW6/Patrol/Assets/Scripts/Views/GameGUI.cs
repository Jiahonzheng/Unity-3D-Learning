using UnityEngine;

namespace Patrol
{
    public class GameGUI : MonoBehaviour
    {
        // 当前游戏状态。
        public GameState state;
        // 当前分数。
        public int score;

        private IUserAction userAction;

        void Awake()
        {
            userAction = Director.GetInstance().currentSceneController as IUserAction;
        }

        void OnGUI()
        {
            GUI.Label(new Rect(160, 30, 200, 100), "Score: " + score, new GUIStyle() { fontSize = 40, });
            if (state != GameState.RUNNING)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), state == GameState.INITIAL ? "Start" : "Restart", new GUIStyle("button")
                {
                    fontSize = 30
                }))
                {
                    (Director.GetInstance().currentSceneController as IUserAction).Restart();
                }
                if (state == GameState.LOSE)
                {
                    GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You Lose!", new GUIStyle() { fontSize = 40, alignment = TextAnchor.MiddleCenter });
                }
            }
        }
    }
}