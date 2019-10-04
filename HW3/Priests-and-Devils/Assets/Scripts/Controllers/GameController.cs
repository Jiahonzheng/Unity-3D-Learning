using UnityEngine;
using System.Collections.Generic;

namespace PriestsAndDevils
{
    // It represents the result of the game.
    public enum Result
    {
        NOT_FINISHED,
        WINNER,
        LOSER,
    }

    public class GameController : MonoBehaviour, ISceneController, IUserAction
    {
        private Game game;
        private GameActionManager actionManager;
        public CoastController leftCoast;
        public CoastController rightCoast;
        public BoatController boat;
        public List<CharacterController> characters = new List<CharacterController>(6);
        private GameGUI gui;

        void Awake()
        {
            // Set the current scene controller and load resources.
            Director.GetInstance().OnSceneWake(this);
            // Add GUI.
            gui = gameObject.AddComponent<GameGUI>() as GameGUI;
            // Initialize the action manager.
            actionManager = gameObject.AddComponent<GameActionManager>();
            // Initialize the game model.
            game = new Game(boat.model, leftCoast.model, rightCoast.model);
            game.onChange += delegate
            {
                gui.result = game.result;
            };
        }

        // Load the resources.
        public void LoadResources()
        {
            GenGameObjects();
        }

        // It generates the GameObjects.
        private void GenGameObjects()
        {
            // Load LeftCoast.
            {
                GameObject temp = Utils.Instantiate("Prefabs/Stone", Coast.departure);
                leftCoast = temp.AddComponent<CoastController>();
                temp.name = leftCoast.name = "LeftCoast";
                leftCoast.location = Location.Left;
            }
            //  Load RightCoast.
            {
                GameObject temp = Utils.Instantiate("Prefabs/Stone", Coast.destination);
                rightCoast = temp.AddComponent<CoastController>();
                temp.name = rightCoast.name = "RightCoast";
                rightCoast.location = Location.Right;
            }
            // Load Boat.
            {
                GameObject temp = Utils.Instantiate("Prefabs/Boat", Boat.departure);
                boat = temp.AddComponent<BoatController>();
                temp.name = boat.name = "Boat";
            }
            // Load Priests.
            for (int i = 0; i < 3; ++i)
            {
                GameObject temp = Utils.Instantiate("Prefabs/Priest", Coast.destination);
                characters[i] = temp.AddComponent<CharacterController>();
                temp.name = characters[i].name = "Priest" + i;
                characters[i].SetPosition(rightCoast.model.GetEmptyPosition());
                characters[i].GoAshore(rightCoast);
            }
            // Load Devils.
            for (int i = 0; i < 3; ++i)
            {
                GameObject temp = Utils.Instantiate("Prefabs/Devil", Coast.destination);
                characters[i + 3] = temp.AddComponent<CharacterController>();
                temp.name = characters[i + 3].name = "Devil" + i;
                characters[i + 3].SetPosition(rightCoast.model.GetEmptyPosition());
                characters[i + 3].GoAshore(rightCoast);
            }
        }

        // It is called when player clicks the boat.
        public void ClickBoat()
        {
            if (boat.model.IsEmpty())
            {
                return;
            }
            // Update the model.
            boat.Move();
            // Update the view.
            actionManager.MoveBoat(boat);
            game.CheckWinner();
        }

        // It is called when player clicks a character.
        public void ClickCharacter(CharacterController character)
        {
            // When the character is onboard, it should go ashore.
            if (character.model.isOnboard)
            {
                CoastController temp = (boat.location == Location.Right ? rightCoast : leftCoast);
                boat.GoAshore(character);
                character.GoAshore(temp);
                actionManager.MoveCharacter(character);
            }
            else // When the character is onshore, it should go aboard.
            {
                // When the character and the boat are not on the same side.
                if (character.location != boat.location)
                {
                    return;
                }
                // When the boat is full.
                if (boat.model.GetEmptyIndex() == -1)
                {
                    return;
                }
                // Go aboard.
                CoastController temp = (character.location == Location.Right ? rightCoast : leftCoast); ;
                temp.GoAboard(character);
                character.GoAboard(boat);
                actionManager.MoveCharacter(character);
            }
            game.CheckWinner();
        }

        // It is called when player resets the game.
        public void Reset()
        {
            // Reset the GUI.
            gui.result = Result.NOT_FINISHED;
            // Reset the boat.
            boat.Reset();
            // Reset the Coasts.
            leftCoast.Reset();
            rightCoast.Reset();
            // Reset the characters.
            for (int i = 0; i < 6; ++i)
            {
                characters[i].Reset();
            }
        }
    }
}