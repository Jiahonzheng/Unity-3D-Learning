using System;
using UnityEngine;

namespace PriestsAndDevils
{
    public class Game
    {
        public Result result = Result.NOT_FINISHED;
        private Boat boat;
        private Coast leftCoast;
        private Coast rightCoast;

        public event EventHandler onChange;

        public Game(Boat boat, Coast leftCoast, Coast rightCoast)
        {
            this.boat = boat;
            this.leftCoast = leftCoast;
            this.rightCoast = rightCoast;
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
            onChange?.Invoke(this, EventArgs.Empty);
        }
    }
}