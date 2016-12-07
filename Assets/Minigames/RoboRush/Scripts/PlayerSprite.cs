using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using XBOXParty;

namespace RoboRush
{
    public class PlayerSprite : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] m_PlayerSprites;

        [SerializeField]
        private Image[] m_PlayerImages;

        public void SetPlayerSprites(List<int> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                m_PlayerImages[i].sprite = m_PlayerSprites[players[i]];
                m_PlayerImages[i].color = GlobalGameManager.Instance.GetPlayerColor(players[i]);
            }
        }
    }
}