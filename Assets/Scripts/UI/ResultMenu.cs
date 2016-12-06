using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using XBOXParty;

namespace Board
{
    public class ResultMenu : MonoBehaviour
    {
        [SerializeField]
        private List<Text> _resultLabels;

        private void Start()
        {
            GlobalGameManager.Instance.GameEndEvent += OnGameEnd;
            GlobalGameManager.Instance.ResetGameEvent += OnResetGame;

            if (GlobalGameManager.Instance.GameState != GameState.STATE_RESULTMENU)
            {
                Hide();
            }
        }

        private void OnDestroy()
        {
            if (GlobalGameManager.Instance != null)
            {
                GlobalGameManager.Instance.GameEndEvent -= OnGameEnd;
                GlobalGameManager.Instance.ResetGameEvent -= OnResetGame;
            }
        }

        private void OnGameEnd(List<int> results)
        {
            Show();

            //Sort the list
            List<KeyValuePair<int, int>> sortedList = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < results.Count; ++i)
            {
                sortedList.Add(new KeyValuePair<int, int>(i, results[i]));
            }
            sortedList.Sort((x, y) => x.Value.CompareTo(y.Value));

            //Set the text
            for (int i = 0; i < results.Count; ++i)
            {
                _resultLabels[i].text = (sortedList[i].Value + 1) + ": Player " + (sortedList[i].Key + 1);
                _resultLabels[i].color = GlobalGameManager.Instance.GetPlayerColor(sortedList[i].Key);
            }

            for (int i = results.Count; i < _resultLabels.Count; ++i)
            {
                _resultLabels[i].gameObject.SetActive(false);
            }
        }

        private void OnResetGame()
        {
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ResetGame()
        {
            GlobalGameManager.Instance.ResetGame();
        }
    }
}