using UnityEngine;
using System.Collections;

namespace Board
{
    public class InputHandler : MonoBehaviour
    {
        private void Update()
        {
            GameState state = GlobalGameManager.Instance.GameState;

            if (Input.GetButtonUp("Start"))
            {
                switch (state)
                {
                    case GameState.STATE_MAINMENU:
                        GlobalGameManager.Instance.StartGame();
                        break;

                    case GameState.STATE_BOARD:
                        GlobalGameManager.Instance.StartMinigame();
                        break;

                    case GameState.STATE_RESULTMENU:
                        GlobalGameManager.Instance.ResetGame();
                        break;
                }
            }
        }
    }
}
