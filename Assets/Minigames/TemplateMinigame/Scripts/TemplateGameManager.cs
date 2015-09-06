using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Board;

namespace TemplateGame
{
    public class TemplateGameManager : MonoBehaviour
    {
        private List<int> _positions;

        private void Awake()
        {
            _positions = new List<int>();

            //If we don't change this player 1 will always be first, player 2 2nd and so on...
            int playerCount = GlobalGameManager.Instance.PlayerCount;
            for (int i = 0; i < playerCount; ++i)
            {
                _positions.Add(i);
            }
        }

        public void Submit()
        {
            GlobalGameManager.Instance.SubmitGameResults(_positions);
        }
    }
}
