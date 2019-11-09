using System;

namespace Patrol
{
    public enum GameState
    {
        INITIAL,
        RUNNING,
        LOSE
    }

    public class GameModel
    {
        public GameState state = GameState.INITIAL;
        public int score;
        public EventHandler onRefresh;

        // 更新分数。
        public void AddScore(int delta)
        {
            score += delta;
            onRefresh?.Invoke(this, null);
        }

        // 重置。
        public void Reset(GameState s = GameState.RUNNING)
        {
            state = s;
            score = 0;
            onRefresh?.Invoke(this, null);
        }
    }
}