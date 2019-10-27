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

        public void AddScore(int delta)
        {
            score += delta;
        }
    }
}