using UnityEngine;

namespace PriestsAndDevils
{
    public class Boat
    {
        // It stores the name of the GameObject.
        public string name { get; set; }
        // It stores the location of the boat: Left or Right.
        public Location location { get; set; }

        public static readonly Vector3 departure = new Vector3(3, 1, 0);
        public static readonly Vector3 destination = new Vector3(-3, 1, 0);
        // It stores the empty positions.
        public static readonly Vector3[] departures = { new Vector3(2.5f, 1.5f, 0), new Vector3(3.5f, 1.5f, 0) };
        // It stores the empty positions.
        public static readonly Vector3[] destinations = { new Vector3(-3.5f, 1.5f, 0), new Vector3(-2.5f, 1.5f, 0) };
        // It stores the passengers.
        public Character[] characters = new Character[2];

        // Temporary destination
        private Vector3 nowDestination;

        // Constructor.
        public Boat()
        {
            // It is on the right side at the beginning.
            location = Location.Right;
        }

        // It returns whether the boat is empty.
        public bool IsEmpty()
        {
            for (int i = 0; i < characters.Length; ++i)
            {
                if (characters[i] != null)
                {
                    return false;
                }
            }
            return true;
        }

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
            Vector3 position;
            int index = GetEmptyIndex();
            if (location == Location.Right)
            {
                position = departures[index];
            }
            else
            {
                position = destinations[index];
            }
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