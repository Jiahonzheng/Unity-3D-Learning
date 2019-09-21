using UnityEngine;

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
        public CoastController leftCoast;
        public CoastController rightCoast;
        public BoatController boat;
        public CharacterController[] characters = new CharacterController[6];
        private GameGUI gui;

        void Awake()
        {
            // Set the current scene controller.
            Director director = Director.GetInstance();
            director.CurrentSceneController = this;
            // Add GUI.
            gui = gameObject.AddComponent<GameGUI>() as GameGUI;
            // Load the resources.
            LoadResources();
        }

        // Load the resources.
        public void LoadResources()
        {
            // Load River.
            {
                GameObject temp = Utils.Instantiate("Prefabs/Water", new Vector3(0, 0.5f, 0));
                temp.name = "River";
            }
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
                characters[i].GoAshore(rightCoast);
            }
            // Load Devils.
            for (int i = 0; i < 3; ++i)
            {
                GameObject temp = Utils.Instantiate("Prefabs/Devil", Coast.destination);
                characters[i + 3] = temp.AddComponent<CharacterController>();
                temp.name = characters[i + 3].name = "Devil" + i;
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
            boat.Move();
            gui.result = CheckWinner();
        }

        // It is called when player clicks a character.
        public void ClickCharacter(CharacterController character)
        {
            // When the character is onboard, it should go ashore.
            if (character.model.isOnboard)
            {
                boat.GoAshore(character);
                character.GoAshore((boat.location == Location.Right ? rightCoast : leftCoast));
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
            }
            // Update the GUI.
            gui.result = CheckWinner();
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

        // It determines whether the player wins the game.
        private Result CheckWinner()
        {
            Result result = Result.NOT_FINISHED;
            // Calculate the amount.
            int leftPriests = leftCoast.model.GetCharacterAmount()[0];
            int leftDevils = leftCoast.model.GetCharacterAmount()[1];
            int rightPriests = rightCoast.model.GetCharacterAmount()[0];
            int rightDevils = rightCoast.model.GetCharacterAmount()[1];

            // When all the characters has gone across the river.
            if (leftPriests + leftDevils == 6)
            {
                result = Result.WINNER;
            }
            // When the boat is on the right side.
            if (boat.location == Location.Right)
            {
                rightPriests += boat.model.GetCharacterAmount()[0];
                rightDevils += boat.model.GetCharacterAmount()[1];
            }
            else // When the boat is on the left side.
            {
                leftPriests += boat.model.GetCharacterAmount()[0];
                leftDevils += boat.model.GetCharacterAmount()[1];
            }
            // In this case, player lose the game.
            if ((rightPriests < rightDevils && rightPriests > 0) ||
                (leftPriests < leftDevils && leftPriests > 0))
            {
                result = Result.LOSER;
            }
            return result;
        }
    }
}