using System;

namespace HitUFO
{
    public class GameModel
    {
        public GameState game = GameState.Running;
        public SceneState scene = SceneState.Waiting;
        public EventHandler onRefresh;
        public EventHandler onEnterNextRound;
        public readonly int maxRound = 10;
        public readonly int maxTrial = 10;
        public int currentRound { get; private set; } = 0;
        public int currentTrial { get; private set; } = 0;
        public int score { get; private set; } = 0;

        public void Reset(GameState _game = GameState.Running)
        {
            game = _game;
            scene = SceneState.Waiting;
            currentRound = 0;
            currentTrial = 0;
            score = 0;
            onRefresh.Invoke(this, EventArgs.Empty);
        }

        public void NextRound()
        {
            ++currentRound;
            if (currentRound == maxRound)
            {
                Reset(GameState.Win);
            }
            else
            {
                onRefresh.Invoke(this, EventArgs.Empty);
                onEnterNextRound.Invoke(this, EventArgs.Empty);
            }
        }

        public void NextTrial()
        {
            ++currentTrial;
            if (currentTrial == maxTrial)
            {
                currentTrial = 0;
                ++currentRound;
                if (currentRound == maxRound)
                {
                    Reset(GameState.Win);
                }
                else
                {
                    onEnterNextRound.Invoke(this, EventArgs.Empty);
                }
            }
            onRefresh.Invoke(this, EventArgs.Empty);
        }

        public void AddScore(int score)
        {
            this.score += score;
            onRefresh.Invoke(this, EventArgs.Empty);
        }

        public void SubScore()
        {
            this.score -= (currentRound + 1) * 10;
            if (this.score < 0)
            {
                this.score = 0;
                this.game = GameState.Lose;
            }
            onRefresh.Invoke(this, EventArgs.Empty);
        }
    }

    public enum GameState
    {
        Running,
        Lose,
        Win
    }

    public enum SceneState
    {
        Waiting,
        Shooting
    }
}