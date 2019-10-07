using System;
using UnityEngine;

namespace HitUFO
{
    public class GameGUI : MonoBehaviour
    {
        public GameState state;
        public int round = 0;
        public int trial = 0;
        public int score = 0;

        public EventHandler onPressRestartButton;
        public EventHandler onPressNextRoundButton;
        public EventHandler onPressNextTrialButton;

        void OnGUI()
        {
            var textStyle = new GUIStyle()
            {
                fontSize = 20
            };
            GUI.Label(new Rect(10, Screen.height / 2 - 250, 200, 100), "Round: " + round, textStyle);
            GUI.Label(new Rect(10, Screen.height / 2 - 220, 200, 100), "Trial: " + trial, textStyle);
            GUI.Label(new Rect(10, Screen.height / 2 - 190, 200, 100), "Score: " + score, textStyle);
            if (state != GameState.Running)
            {
                var text = state == GameState.Win ? "You Win!" : "You Lose!";
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 50), text, new GUIStyle() { fontSize = 40, alignment = TextAnchor.MiddleCenter });
                if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", new GUIStyle("button") { fontSize = 30 }))
                {
                    onPressRestartButton.Invoke(this, EventArgs.Empty);
                }
            }
            if (GUI.Button(new Rect(Screen.width -125, Screen.height - 180, 120, 70), "NextRound", new GUIStyle("button") { fontSize = 20,alignment = TextAnchor.MiddleCenter }))
            {
                onPressNextRoundButton.Invoke(this, EventArgs.Empty);
            }
            if (GUI.Button(new Rect(Screen.width - 125, Screen.height - 100, 120, 70), "NextTrial", new GUIStyle("button") { fontSize = 20, alignment = TextAnchor.MiddleCenter }))
            {
                onPressNextTrialButton.Invoke(this, EventArgs.Empty);
            }
        }
    }
}