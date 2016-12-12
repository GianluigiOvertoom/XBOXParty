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
            for (int i = 0; i < 4; ++i)
            {
                InputManager.Instance.BindButton("Board_Submit_" + i, i, ControllerButtonCode.A, ButtonState.OnPress);
                InputManager.Instance.BindButton("Board_Reset_" + i, i, ControllerButtonCode.Back, ButtonState.OnPress);
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < 4; ++i)
            {
                InputManager.Instance.UnbindButton("Board_Submit_" + i);
                InputManager.Instance.UnbindButton("Board_Reset_" + i);
            }
        }

        private void Update()
        {
            GameState state = GlobalGameManager.Instance.GameState;

            bool pressedSubmit = false;
            bool pressedReset = false;

            for (int i = 0; i < 4; ++i)
            {
                if (InputManager.Instance.GetButton("Board_Submit_" + i)) { pressedSubmit = true; }
                if (InputManager.Instance.GetButton("Board_Reset_" + i))  { pressedReset = true; }

            }

            if (pressedSubmit)
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

            if (pressedReset)
            {
                switch (state)
                {
                    case GameState.STATE_BOARD:
                        GlobalGameManager.Instance.HardResetGame();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
