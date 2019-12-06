using UnityEngine;

namespace PriestsAndDevils
{
    public class Coast
    {
        // It stores the name of the GameObject.
        public string name { get; set; }
        // It stores the location of the coast: Left or Right.
        public Location location { get; set; }

        public static readonly Vector3 departure = new Vector3(7, 1, 0);
        public static readonly Vector3 destination = new Vector3(-7, 1, 0);
        // It stores the empty positions.
        public static readonly Vector3[] positions = {
                new Vector3(4.5f, 2.25f, 0),
                new Vector3(5.5f, 2.25f, 0),
                new Vector3(6.5f, 2.25f, 0),
                new Vector3(7.5f, 2.25f, 0),
                new Vector3(8.5f, 2.25f, 0),
                new Vector3(9.5f, 2.25f, 0),};
        // It stores the characters.
        public Character[] characters = new Character[6];

        // It returns the empty index of the boat.
        public int GetEmptyIndex()
        {
            for (int i = 0; i < characters.Length; ++i)
            {
                if (characters[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        // It returns the empty position of the boat.
        public Vector3 GetEmptyPosition()
        {
            Vector3 position = positions[GetEmptyIndex()];
            // Be careful!
            position.x *= (location == Location.Right ? 1 : -1);
            return position;
        }

        // It returns the amount of the Priests and the Devils.
        public int[] GetCharacterAmount()
        {
            // amount[0]: the amount of the Priests.
            // amount[1]: the amount of the Devils.
            int[] amount = { 0, 0 };
            for (int i = 0; i < characters.Length; ++i)
            {
                if (characters[i] != null)
                {
                    // When the character is a Priest.
                    if (characters[i].name.Contains("Priest"))
                    {
                        amount[0]++;
                    }
                    else // When the character is a Devil.
                    {
                        amount[1]++;
                    }
                }
            }
            return amount;
        }
    }
}