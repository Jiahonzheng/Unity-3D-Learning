using System;
using UnityEngine;

namespace PriestsAndDevils
{
    // It represents the result of the game.
    public enum Result
    {
        NOT_FINISHED,
        WINNER,
        LOSER,
    }

    // It represents the state of the game.
    public class State
    {
        public int leftPriests;
        public int leftDevils;
        public int rightPriests;
        public int rightDevils;
        public bool location;

        public State(int leftPriests, int leftDevils, int rightPriests, int rightDevils, bool location)
        {
            this.leftPriests = leftPriests;
            this.leftDevils = leftDevils;
            this.rightPriests = rightPriests;
            this.rightDevils = rightDevils;
            this.location = location;
        }
    }

    public class Game
    {
        // 表示游戏结果。
        public Result result = Result.NOT_FINISHED;
        private Boat boat;
        private Coast leftCoast;
        private Coast rightCoast;
        private State state;
        // 用于通知场景控制器游戏的胜负。
        public event EventHandler onChange;
        // 根据传入的控制器生成裁判类。
        public Game(Boat boat, Coast leftCoast, Coast rightCoast)
        {
            this.boat = boat;
            this.leftCoast = leftCoast;
            this.rightCoast = rightCoast;
            this.state = new State(0, 0, 3, 3, false);
        }

        // It returns the current state.
        public State GetState()
        {
            return state;
        }

        // It determines whether the player wins the game.
        public void CheckWinner()
        {
            result = Result.NOT_FINISHED;
            // Calculate the amount.
            int leftPriests = leftCoast.GetCharacterAmount()[0];
            int leftDevils = leftCoast.GetCharacterAmount()[1];
            int rightPriests = rightCoast.GetCharacterAmount()[0];
            int rightDevils = rightCoast.GetCharacterAmount()[1];
            // When all the characters has gone across the river.
            if (leftPriests + leftDevils == 6)
            {
                result = Result.WINNER;
            }
            // When the boat is on the right side.
            if (boat.location == Location.Right)
            {
                rightPriests += boat.GetCharacterAmount()[0];
                rightDevils += boat.GetCharacterAmount()[1];
            }
            else // When the boat is on the left side.
            {
                leftPriests += boat.GetCharacterAmount()[0];
                leftDevils += boat.GetCharacterAmount()[1];
            }
            // In this case, player lose the game.
            if ((rightPriests < rightDevils && rightPriests > 0) ||
                (leftPriests < leftDevils && leftPriests > 0))
            {
                result = Result.LOSER;
            }
            // Update the state.
            state.leftPriests = leftPriests;
            state.leftDevils = leftDevils;
            state.rightPriests = rightPriests;
            state.rightDevils = rightDevils;
            state.location = boat.location == Location.Left;
            // 通知场景控制器。
            onChange?.Invoke(this, EventArgs.Empty);
        }
    }
}