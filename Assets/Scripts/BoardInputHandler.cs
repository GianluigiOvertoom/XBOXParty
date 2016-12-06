using UnityEngine;
using System.Collections;
using XBOXParty;

namespace Board
{
    public class BoardInputHandler : MonoBehaviour
    {
        private void Start()
        {
            //Bind button
            InputManager.Instance.BindButton("Board_Submit", 0, ControllerButtonCode.A, ButtonState.OnPress);
            InputManager.Instance.BindButton("Board_Reset", 0, ControllerButtonCode.Back, ButtonState.OnPress);
        }

        private void OnDestroy()
        {
            InputManager.Instance.UnbindButton("Board_Submit");
            InputManager.Instance.UnbindButton("Board_Reset");
        }

        private void Update()
        {
            GameState state = GlobalGameManager.Instance.GameState;

            if (InputManager.Instance.GetButton("Board_Submit"))
            {
                switch (state)
                {
                    //case GameState.STATE_MAINMENU:
                    //    GlobalGameManager.Instance.StartGame();
                    //    break;

                    case GameState.STATE_BOARD:
                        GlobalGameManager.Instance.StartMinigame();
                        break;

                    case GameState.STATE_RESULTMENU:
                        GlobalGameManager.Instance.ResetGame();
                        break;

                    default:
                        break;
                }
            }

            if (InputManager.Instance.GetButton("Board_Reset"))
            {
                GlobalGameManager.Instance.HardResetGame();
            }
        }
    }
}
