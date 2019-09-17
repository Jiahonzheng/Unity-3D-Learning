using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TicTacToe.UI
{
    public class HomeController : MonoBehaviour
    {
        // OnGUI is called when rendering and handling GUI events.
        void OnGUI()
        {
            float height = Screen.height * 0.5f;
            float width = Screen.width * 0.5f;

            // Render the title.
            float titleWidth = 200;
            float titleHeight = 100;
            float titleBaseX = width - titleWidth / 2 - 35;
            float titleBaseY = height - titleHeight * 2;
            GUIStyle titleStyle = new GUIStyle
            {
                fontSize = 50,
                fontStyle = FontStyle.Bold,
            };
            GUI.Label(new Rect(titleBaseX, titleBaseY, titleWidth, titleHeight), "Tic Tac Toe", titleStyle);

            // Render the creator.
            GUI.Label(new Rect(titleBaseX + 40, titleBaseY + 80, titleWidth, titleHeight), "Jiahonzheng", new GUIStyle
            {
                fontSize = 30,
                fontStyle = FontStyle.Bold,
            });

            // Render the buttons.
            float buttonWidth = 150;
            float buttonHeight = 100;
            float buttonBaseX = width - buttonWidth / 2;
            float buttonBaseY = height - buttonHeight / 2;

            // Render "Single Player" button.
            if (GUI.Button(new Rect(buttonBaseX, buttonBaseY, buttonWidth, buttonHeight), "Single Player"))
            {
                SceneManager.LoadScene("SinglePlayerMode");
            }

            // Render "Two Players" button.
            if (GUI.Button(new Rect(buttonBaseX, buttonBaseY + buttonHeight, buttonWidth, buttonHeight), "Two Players"))
            {
                SceneManager.LoadScene("TwoPlayerMode");
            }

            // Render "Quit" button.
            if (GUI.Button(new Rect(buttonBaseX, buttonBaseY + buttonHeight * 2, buttonWidth, buttonHeight), "Quit"))
            {
                Application.Quit();
            }
        }
    }
}