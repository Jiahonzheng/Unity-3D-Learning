using System;
using UnityEngine;

namespace Archery
{
    public enum SceneState
    {
        // 等待获取箭
        WaitToGetArrow,
        // 等待射箭
        WaitToShootArrow,
        // 箭已射出
        Shooting,
    }

    public class GameModelChangedEvent : EventArgs
    {
        public int score;
        public int delta;

        public GameModelChangedEvent(int score, int delta)
        {
            this.score = score;
            this.delta = delta;
        }
    }

    public class GameModel
    {
        public SceneState scene = SceneState.WaitToGetArrow;
        public int score = 0;
        public EventHandler<GameModelChangedEvent> onGameModelChanged;
        public Wind currentWind = new GameModel.Wind(Vector3.zero, 0, "");

        public class Wind
        {
            public Vector3 direction;
            public int strength;
            public string text;

            public Wind(Vector3 d, int s, string t)
            {
                direction = d;
                strength = s;
                text = t;
            }
        }

        private Vector3[] winds = new Vector3[8] { new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector3(1, -1, 0), new Vector3(0, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 0, 0), new Vector3(-1, 1, 0) };
        private string[] windsText = new string[8] { "↑", "↗", "→", "↘", "↓", "↙", "←", "↖" };

        public void AddScore(int target)
        {
            score += target;
            onGameModelChanged.Invoke(this, new GameModelChangedEvent(score, target));
        }

        public void AddWind()
        {
            var index = UnityEngine.Random.Range(0, 8);
            var strength = UnityEngine.Random.Range(5, 10);
            currentWind.direction = winds[index] * strength;
            currentWind.strength = strength;
            currentWind.text = windsText[index];
        }
    }
}