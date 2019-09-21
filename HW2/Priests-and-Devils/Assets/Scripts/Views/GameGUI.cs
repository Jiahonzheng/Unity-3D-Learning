using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriestsAndDevils
{
    public class GameGUI : MonoBehaviour
    {
        public Result result;
        private IUserAction action;

        // Use this for initialization
        void Start()
        {
            result = Result.NOT_FINISHED;
            action = Director.GetInstance().CurrentSceneController as IUserAction;
        }

        void OnGUI()
        {
            var textStyle = new GUIStyle()
            {
                fontSize = 40,
                alignment = TextAnchor.MiddleCenter
            };
            var buttonStyle = new GUIStyle("button")
            {
                fontSize = 30
            };
            // Show the title.
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 300, 100, 50), "Priests and Devils", textStyle);
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 220, 100, 50), "Jiahonzheng", textStyle);
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