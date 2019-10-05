using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriestsAndDevils
{
    public class GameGUI : MonoBehaviour
    {
        public Result result;
        private IUserAction action;

        private const string title = "Priests and Devils";
        private const string author = "Jiahonzheng";
        private const string rule =
@"Cube: Periest    Sphere: Devil
3 priests and 3 devils want to go across the river.
The boat can only carries two persons each time.
One person must steer the boat.
Click on person or boat to make it move.
Priests get killed when less than devils on either side.
You should keep all priests alive!";

        // Use this for initialization
        void Start()
        {
            result = Result.NOT_FINISHED;
            action = Director.GetInstance().currentSceneController as IUserAction;
        }

        void OnGUI()
        {
            var textStyle = new GUIStyle()
            {
                fontSize = 40,
                alignment = TextAnchor.MiddleCenter
            };
            var ruleStyle = new GUIStyle
            {
                fontSize = 20,
                fontStyle = FontStyle.Normal
            };
            var buttonStyle = new GUIStyle("button")
            {
                fontSize = 30
            };
            // Show the title and the author.
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 300, 100, 50), title, textStyle);
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 250, 100, 50), author, textStyle);
            // Show the rules.
            GUI.Label(new Rect(Screen.width / 2 - 450, Screen.height / 2 - 220, 100, 50), rule, ruleStyle);
            // Show the result.
            if (result != Result.NOT_FINISHED)
            {
                var text = result == Result.WINNER ? "You Win!" : "You Lose!";
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), text, textStyle);
                // When user presses the Restart button.
                if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
                {
                    action.Reset();
                }
            }
        }

        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    var todo = hit.collider;
                    var character = todo.GetComponent<CharacterController>();
                    if (character)
                    {
                        action.ClickCharacter(character);
                    }
                    else if (todo.transform.name == "Boat")
                    {
                        action.ClickBoat();
                    }
                }
            }
        }
    }
}