using UnityEngine;

namespace PriestsAndDevils
{
    public class CharacterController : MonoBehaviour
    {
        public Character model = new Character();
        public new string name { get { return model.name; } set { model.name = value; } }
        public Location location { get { return model.location; } set { model.location = value; } }

        private Vector3 nowDestination;

        // It is called when a character goes aboard.
        public void GoAboard(BoatController boat)
        {
            location = boat.location;
            model.isOnboard = true;
            transform.parent = boat.transform;
            nowDestination = boat.model.GetEmptyPosition();
            boat.GoAboard(this);
        }

        // It is called when a character goes ashore.
        public void GoAshore(CoastController coast)
        {
            model.location = coast.location;
            model.isOnboard = false;
            transform.parent = null;
            nowDestination = coast.model.GetEmptyPosition();
            coast.GoAshore(this);
        }

        // It returns where the character goes.
        public Vector3 GetDestination()
        {
            return nowDestination;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        // It is called when player resets the game.
        public void Reset()
        {
            CoastController coast = (Director.GetInstance().currentSceneController as GameController).rightCoast;
            GoAshore(coast);
            transform.position = nowDestination;
        }
    }
}
