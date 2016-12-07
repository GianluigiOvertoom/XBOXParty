using UnityEngine;
using System.Collections;
using RoboRush;
using XBOXParty;

namespace RoboRush
{
    public class GameController : MonoBehaviour
    {
        GlobalGameManager m_gm;

        void Awake()
        {
            m_gm = GlobalGameManager.Instance;
        }

        void Update()
        {

        }
    }

}