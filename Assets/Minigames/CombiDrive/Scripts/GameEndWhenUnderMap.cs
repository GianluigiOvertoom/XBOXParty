using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XBOXParty;

namespace CombiDrive
{
    public class GameEndWhenUnderMap : MonoBehaviour
    {
        GlobalGameManager instance;
        private GameController m_Controller;

        private List<int> m_FailedList = new List<int>();

        private void Start()
        {
            instance = GlobalGameManager.Instance;

            m_Controller = GameObject.Find("CombiDriveGameController").GetComponent<GameController>();

        }
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.name == "Race car 1" || other.name == "Race car 2")
            {
                m_FailedList.Add(0);
                m_FailedList.Add(0);
                m_FailedList.Add(0);
                m_FailedList.Add(0);
                instance.SubmitGameResults(m_FailedList);
            }

            if (other.name == "Hiroshima bomb")
            {
                Destroy(other.gameObject);
            }
        }
    }
}
