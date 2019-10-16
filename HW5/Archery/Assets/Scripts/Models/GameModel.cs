using System;

namespace Archery
{
    public enum SceneState
    {
        WaitToGetArrow,
        WaitToShootArrow,
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

