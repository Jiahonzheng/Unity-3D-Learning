using System;

namespace HitUFO
{
    public class GameModel
    {
        // 表示当前游戏状态（正在进行、玩家胜利、玩家失败）。
        public GameState game = GameState.Running;
        // 表示当前场景状态（等待用户按下空格键、等待用户点击鼠标左键）。
        public SceneState scene = SceneState.Waiting;
        // 通知场景控制器更新游戏画面。
        public EventHandler onRefresh;
        // 通知场景控制器更新 Ruler 。
        public EventHandler onEnterNextRound;
        // 表示最大 Round 次数。
        public readonly int maxRound = 10;
        // 表示单个 Round 中最大 Trial 次数。
        public readonly int maxTrial = 10;
        // 表示当前是第几个 Round 。
        public int currentRound { get; private set; } = 0;
        // 表示当前是第几个 Trial 。
        public int currentTrial { get; private set; } = 0;
        // 维护当前的玩家分数。
        public int score { get; private set; } = 0;

        // 重置裁判类。
        public void Reset(GameState _game = GameState.Running)
        {
            game = _game;
            scene = SceneState.Waiting;
            currentRound = 0;
            currentTrial = 0;
            score = 0;
            // 通知场景控制器需要重置游戏画面。
            onRefresh.Invoke(this, EventArgs.Empty);
            // 通知场景控制器需要重置 Ruler 。
            onEnterNextRound.Invoke(this, EventArgs.Empty);
        }

        // 进入下一 Round（作为调试用）。
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

        // 进入下一 Trial 。
        public void NextTrial()
        {
            ++currentTrial;
            if (currentTrial == maxTrial)
            {
                currentTrial = 0;
                ++currentRound;
                // 检测玩家是否胜利。
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

        // 增加玩家分数。
        public void AddScore(int score)
        {
            this.score += score;
            // 通知场景控制器需要更新游戏画面。
            onRefresh.Invoke(this, EventArgs.Empty);
        }

        // 扣除玩家分数。
        public void SubScore()
        {
            this.score -= (currentRound + 1) * 10;
            // 检测玩家是否失败。
            if (score < 0)
            {
                Reset(GameState.Lose);
            }
            onRefresh.Invoke(this, EventArgs.Empty);
        }
    }

    public enum GameState
    {
        Running, // 正在进行
        Lose, // 玩家失败
        Win // 玩家胜利
    }

    public enum SceneState
    {
        Waiting, // 等待用户按下空格键
        Shooting // 等待用户点击鼠标左键
    }
}