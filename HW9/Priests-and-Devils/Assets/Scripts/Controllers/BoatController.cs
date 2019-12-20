using UnityEngine;

namespace PriestsAndDevils
{
    public class BoatController : MonoBehaviour
    {
        public Boat model = new Boat();
        public Location location { get { return model.location; } set { model.location = value; } }

        private Vector3 nowDestination;

        // It is called when boat is clicked to move.
        public void Move()
        {
            if (location == Location.Left)
            {
                location = Location.Right;
                nowDestination = Boat.departure;
            }
            else
            {
                location = Location.Left;
                nowDestination = Boat.destination;
            }
        }

        // It returns which side the boat should go.
        public Vector3 GetDestination()
        {
            return nowDestination;
        }

        // It is called when a character go aboard.
        public void GoAboard(CharacterController character)
        {
            int index = model.GetEmptyIndex();
            model.characters[index] = character.model;
        }

        // It is called when a character goes ashore.
        public void GoAshore(CharacterController character)
        {
            var characters = model.characters;
            for (int i = 0; i < characters.Length; ++i)
            {
                if (characters[i] != null && characters[i].name == character.name)
                {
                    characters[i] = null;
                }
            }
        }

        // It is called when player resets the game.
        public void Reset()
        {
            if (location == Location.Left)
            {
                Move();
                transform.position = nowDestination;
            }
            model.characters = new Character[2];
        }
    }
}
