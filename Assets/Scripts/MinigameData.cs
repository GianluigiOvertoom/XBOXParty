using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XBOXParty
{
    public enum MinigameMode
    {
        MODE_FFA = 0,
        MODE_2V2 = 1,
        MODE_1V3 = 2
    }

    public class MinigameData : ScriptableObject
    {
        [SerializeField]
        private string m_Naam; //m_Name gives errors?
        public string Name
        {
            get { return m_Naam; }
        }

        [SerializeField]
        private MinigameMode m_GameMode;
        public MinigameMode GameMode
        {
            get { return m_GameMode; }
        }

        [SerializeField]
        private string m_RootScene;
        public string RootScene
        {
            get { return m_RootScene; }
        }

        [SerializeField]
        private Sprite m_Logo;
        public Sprite Logo
        {
            get { return m_Logo; }
        }

        [SerializeField]
        private Sprite m_Background;
        public Sprite Background
        {
            get { return m_Background; }
        }
    }
}
