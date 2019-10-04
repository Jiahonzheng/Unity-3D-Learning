using System.Collections.Generic;
using UnityEngine;

namespace PriestsAndDevils
{
    public class GameActionManager : ActionManager
    {
        public void MoveBoat(BoatController boat)
        {
            MoveToAction action = MoveToAction.GetAction(boat.gameObject, this, boat.GetDestination(), 20);
            AddAction(action);
        }

        public void MoveCharacter(CharacterController character)
        {
            Vector3 destination = character.GetDestination();
            GameObject gameObject = character.gameObject;
            Vector3 currentPosition = character.transform.position;
            Vector3 middlePosition = currentPosition;
            if (destination.y > currentPosition.y)
            {
                middlePosition.y = destination.y;
            }
            else
            {
                middlePosition.x = destination.x;
            }
            Action action1 = MoveToAction.GetAction(gameObject, null, middlePosition, 20);
            Action action2 = MoveToAction.GetAction(gameObject, null, destination, 20);
            SequenceAction action = SequenceAction.GetAction(this, new List<Action> { action1, action2 });
            AddAction(action);
        }
    }
}