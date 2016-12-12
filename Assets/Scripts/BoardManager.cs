using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using XBOXParty;

namespace Board
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField]
        private Image m_LogoImage;

        [SerializeField]
        private SpriteRenderer m_BoardImage;

        [SerializeField]
        private List<Node> m_Nodes;

        // Use this for initialization
        private void Awake()
        {
            MinigameData lastMinigame = GlobalGameManager.Instance.GetCurrentMinigame();

            if (lastMinigame != null)
            {
                m_LogoImage.sprite = lastMinigame.Logo;
                m_BoardImage.sprite = lastMinigame.Background;
            }
        }

        public Node GetNode(int id)
        {
            if (id >= m_Nodes.Count)
                return null;

            return m_Nodes[id];
        }

        public int GetNodeId(Node node)
        {
            for (int i = 0; i < m_Nodes.Count; ++i)
            {
                if (m_Nodes[i] == node)
                    return i;
            }

            return -1;
        }
    }
}
