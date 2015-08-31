using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
	private void Update ()
    {
        GameState state = GlobalGameManager.Instance.GetGameState();

	    if (Input.GetButtonUp("Player0_A"))
        {
            switch (state)
            {
                case GameState.STATE_MAINMENU:
                    GlobalGameManager.Instance.StartGame();
                    break;

                case GameState.STATE_BOARD:
                    GlobalGameManager.Instance.StartMinigame();
                    break;
            }
        }
	}
}
