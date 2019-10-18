using System;

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

        public void AddScore(int target)
        {
            score += target;
            onGameModelChanged.Invoke(this, new GameModelChangedEvent(score, target));
        }
    }
}

