using UnityEngine;

namespace PriestsAndDevils
{
    public class CharacterController : Moveable
    {
        public Character model = new Character();
        public new string name { get { return model.name; } set { model.name = value; } }
        public Location location { get { return model.location; } set { model.location = value; } }

        // It is called when a character goes aboard.
        public void GoAboard(BoatController boat)
        {
            location = boat.location;
            model.isOnboard = true;
            transform.parent = boat.transform;
            SetDestination(boat.model.GetEmptyPosition());
            boat.GoAboard(this);
        }

        // It is called when a character goes ashore.
        public void GoAshore(CoastController coast)
        {
            model.location = coast.location;
            model.isOnboard = false;
            transform.parent = null;
            SetDestination(coast.model.GetEmptyPosition());
            coast.GoAshore(this);
        }

        // It is called when player resets the game.
        public new void Reset()
        {
            base.Reset();
            GoAshore((Director.GetInstance().currentSceneController as GameController).rightCoast);
        }
    }
}
