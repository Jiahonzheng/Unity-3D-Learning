using UnityEngine;

namespace PriestsAndDevils
{
    public class CoastController : MonoBehaviour
    {
        public Coast model = new Coast();
        public new string name { get { return model.name; } set { model.name = value; } }
        public Location location { get { return model.location; } set { model.location = value; } }

        // It is called when a character goes ashore.
        public void GoAboard(CharacterController character)
        {
            var characters = model.characters;
            for (int i = 0; i < characters.Length; ++i)
            {
                if (characters[i] != null &&
                    characters[i].name == character.name)
                {
                    characters[i] = null;
                }
            }
        }

        // It is called when a character go Ashore.
        public void GoAshore(CharacterController character)
        {
            int index = model.GetEmptyIndex();
            model.characters[index] = character.model;
        }

        // It is called when player resets the game.
        public void Reset()
        {
            model.characters = new Character[6];
        }
    }
}
