using UnityEngine;

namespace PriestsAndDevils
{
    public class BoatController : Moveable
    {
        public Boat model = new Boat();
        public Location location { get { return model.location; } set { model.location = value; } }

        // It is called when boat is clicked to move.
        public void Move()
        {
            if (model.location == Location.Left)
            {
                model.location = Location.Right;
                SetDestination(Boat.departure);
            }
            else
            {
                model.location = Location.Left;
                SetDestination(Boat.destination);
            }
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
        public new void Reset()
        {
            base.Reset();
            if (location == Location.Left)
            {
                Move();
            }
            model.characters = new Character[2];
        }
    }
}
