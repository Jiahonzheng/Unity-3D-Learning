using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TicTacToe.UI
{
    public abstract class GameController : MonoBehaviour
    {
        // It controls the logic of the game.
        protected Mechanics.Basic mechanics;

        // OnGUI is called when rendering and handling GUI events.
        void OnGUI()
        {
            float width = Screen.width * 0.5f - 150;
            float height = Screen.height * 0.5f - 150;
            // Width and Height of the "Back to Home" and "Reset" buttons.
            float buttonWidth = 150;
            float buttonHeight = 100;
            // Render "Back to Home" button.
            float buttonBaseX = width - buttonWidth / 2;
            float buttonBaseY = height + buttonHeight * 2;
            if (GUI.Button(new Rect(buttonBaseX - 100, buttonBaseY - 200, buttonWidth, buttonHeight), "Back to Home"))
            {
                OnPressBackButton();
            }
            // Render "Reset" button.
            if (GUI.Button(new Rect(buttonBaseX - 100, buttonBaseY, buttonWidth, buttonHeight), "Reset"))
            {
                OnPressResetButton();
            }
            // Check if there is a winner.
            var winner = mechanics.CheckWin();
            if (winner != Mechanics.Player.Unfinished)
            {
                string msg = winner == Mechanics.Player.First ? "Player1(X) Wins!" : winner == Mechanics.Player.Second ? "Player2(O) Wins!" : "Tie!";
                GUIStyle messageStyle = new GUIStyle
                {
                    fontSize = 25,
                    fontStyle = FontStyle.Bold,
                };
                messageStyle.normal.textColor = Color.red;
                // Show the results.
                GUI.Label(new Rect(width + 50, height - 75, 100, 100), msg, messageStyle);
                GUI.enabled = false;
            }

            // Render the map.
            int bHeight = 100;
            int bWidth = 100;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    var player = mechanics.GetHistory(i, j);
                    string token;
                    if (player == Mechanics.Player.First)
                    {
                        token = "X";
                    }
                    else if (player == Mechanics.Player.Second)
                    {
                        token = "O";
                    }
                    else
                    {
                        token = "";
                    }
                    // Render the current brick.
                    var isPressed = GUI.Button(new Rect(width + i * bWidth, height + j * bHeight, bWidth, bHeight), token);
                    // Prevent multiple clicks.
                    if (player == Mechanics.Player.Unfinished)
                    {
                        AfterRenderButton(i, j, player, isPressed);
                    }
                }
            }

            GUI.enabled = true;
        }

        public abstract void AfterRenderButton(int i, int j, Mechanics.Player player, bool isPressed);

        // It is called when "Back to Home" button is pressed.
        void OnPressBackButton()
        {
            SceneManager.LoadScene("Home");
        }

        // It is called when "Reset" button is pressed.
        void OnPressResetButton()
        {
            mechanics.Reset();
            GUI.enabled = true;
        }
    }
}