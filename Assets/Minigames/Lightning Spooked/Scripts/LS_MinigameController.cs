using UnityEngine;
using System.Collections;
using XBOXParty;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LightningSpooked
{
    public class LS_MinigameController : MonoBehaviour
    {
        private List<int> LS_playerScore = new List<int>(4);

        [SerializeField]
        private LS_PlayerController[] LS_players;

        [SerializeField]
        private LS_Electricity LS_electricity;

        [SerializeField]
        private RawImage LS_introScreen;

        [SerializeField]
        private RawImage LS_tutorialScreen;

        [SerializeField]
        private RawImage LS_introStart;

        [SerializeField]
        private RawImage LS_tutStart;

        private int LS_playerDeaths;


        //Bind some buttons.
        private void Awake()
        {
            //Set all the players on false so the players won't have to see them.
            for (int i = 0; i < LS_players.Length; i++)
            {
                LS_players[i].enabled = false;
            }


            //Set the electricity in false so the game won't start.
            LS_electricity.enabled = false;


            //The host (player1) can press B to go to the next screen.
            InputManager.Instance.BindButton("nextScreen", 0,ControllerButtonCode.Start, ButtonState.OnRelease);
        }


        //Make sure to check for input before the minigame has started.
        private void Update()
        {
            bool nextScreen = InputManager.Instance.GetButton("nextScreen");
            //bool nextScreen = true;


            //Check for input to get rid of the tutorial screen.
            if ((nextScreen) && (LS_tutorialScreen.IsActive()) && (!LS_introScreen.enabled))
            {
                LS_tutorialScreen.enabled = false;
                //LS_tutStart.enabled = false;
                StartMinigame();
            }


            //Check for input to get rid of the tutorial screen.
            if ((nextScreen) && (LS_introScreen.IsActive()))
            {
                LS_introScreen.enabled = false;
                //LS_introStart.enabled = false;
            }
        }


        //Start the minigame after the player has seen the logo and the tutorial screen.
        private void StartMinigame()
        {
            //Set the playerID's and give them to the right color.
            for (int i = 0; i < LS_players.Length; i++)
            {
                print(i);
                LS_players[i].enabled = true;
                LS_players[i].getSetLS_playerID = i;
            }


            //Fill the score list
            for (int i = 0; i < LS_playerScore.Capacity; i++)
            {
                LS_playerScore.Add(i);
            }


            //Set the electricity back to true when the game starts.
            LS_electricity.enabled = true;


            //Get the players and create vessels for them in the LS_playerScore list.
            int playerCount = GlobalGameManager.Instance.PlayerCount;
        }


        //Submit the score into the system.
        public void SubmitScore()
        {
            print("Player1: " + LS_playerScore[0]);
            print("Player2: " + LS_playerScore[1]);
            print("Player3: " + LS_playerScore[2]);
            print("Player4: " + LS_playerScore[3]);

            //This is the important function!     
            GlobalGameManager.Instance.SubmitGameResults(LS_playerScore); 
        }


        //Set the player death.
        public void SetPlayerDeathsInListAndSubmitScore(int playerID)
        {
            LS_PlayerController overigePlayer;

            //Check how many players have died. (in case of 0: the player is at the last place);
            switch(LS_playerDeaths)
            {
                case 0:
                    LS_playerScore[playerID] = 3;
                    break;
                case 1:
                    LS_playerScore[playerID] = 2;
                    break;
                case 2:
                    LS_playerScore[playerID] = 1;
                    overigePlayer = FindObjectOfType<LS_PlayerController>();
                    LS_playerScore[overigePlayer.getSetLS_playerID] = 0;
                    break;
            }


            //Update the amount of players that had died.
            LS_playerDeaths++;


            //check if all the players have died.
            if (LS_playerDeaths >= 3)
            {
                SubmitScore();
            }
        }
    }
}