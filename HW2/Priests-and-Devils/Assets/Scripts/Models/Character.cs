using UnityEngine;

namespace PriestsAndDevils
{
    public class Character
    {
        // It stores the name of the GameObject.
        public string name { get; set; }
        // It stores the location of the character: Left or Right.
        public Location location { get; set; }
        // It returns whether the character is on board.
        public bool isOnboard { get; set; }
    }
}