using UnityEngine;
using System.Collections;
using XBOXParty;

namespace XBOXParty
{
    public class SkipMinigame : MonoBehaviour
    {
        private void Start()
        {
            //Bind button
            for (int i = 0; i < 4; ++i)
            {
                InputManager.Instance.BindButton("XBOXParty_Skip_Minigame_One" + i, i, ControllerButtonCode.Back, ButtonState.Pressed);
                InputManager.Instance.BindButton("XBOXParty_Skip_Minigame_Two" + i, i, ControllerButtonCode.Start, ButtonState.Pressed);
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < 4; ++i)
            {
                InputManager.Instance.UnbindButton("XBOXParty_Skip_Minigame_One" + i);
                InputManager.Instance.UnbindButton("XBOXParty_Skip_Minigame_Two" + i);
            }
        }

        private void Update()
        {
            for (int i = 0; i < 4; ++i)
            {
                if (InputManager.Instance.GetButton("XBOXParty_Skip_Minigame_One" + i) &&
                    InputManager.Instance.GetButton("XBOXParty_Skip_Minigame_Two" + i))
                {
                    if (GlobalGameManager.Instance.GameState == GameState.STATE_MINIGAME)
                    {
                        GlobalGameManager.Instance.SubmitGameResults(null);
                    }
                }
            }
        }
    }
}
