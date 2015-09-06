using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Board
{
    public delegate void VoidDelegate();
    public delegate void IntListDelegate(List<int> intList);

    public enum GameState
    {
        STATE_MAINMENU,
        STATE_BOARD,
        STATE_MINIGAME,
        STATE_RESULTMENU
    }

    public class GlobalGameManager : Singleton<GlobalGameManager>
    {
        //------------------
        // Datamembers
        //------------------
        [SerializeField]
        private List<Color> _playerColors;

        [SerializeField]
        private List<int> _numStepsAwarded;

        private GameState _gameState;
        public GameState GameState
        {
            get { return _gameState; }
        }

        private int _playerCount = 0;
        public int PlayerCount
        {
            get { return _playerCount; }
        }

        private int _boardLevelID = 1;
        public int BoardLevelID
        {
            get { return _boardLevelID; }
        }

        private bool _canStartMinigame = true;

        private List<int> _currentPawnPositions;
        private List<int> _addedPawnPositions;

        //------------------
        // Events
        //------------------
        private event VoidDelegate _resetGameEvent;
        public VoidDelegate ResetGameEvent
        {
            get { return _resetGameEvent; }
            set { _resetGameEvent = value; }
        }

        private event VoidDelegate _gameStartEvent;
        public VoidDelegate GameStartEvent
        {
            get { return _gameStartEvent; }
            set { _gameStartEvent = value; }
        }

        private event VoidDelegate _minigameStartEvent;
        public VoidDelegate MiniGameStartEvent
        {
            get { return _minigameStartEvent; }
            set { _minigameStartEvent = value; }
        }

        private event IntListDelegate _gameEndEvent;
        public IntListDelegate GameEndEvent
        {
            get { return _gameEndEvent; }
            set { _gameEndEvent = value; }
        }

        //------------------
        // Functions
        //------------------
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            _gameState = GameState.STATE_MAINMENU;
        }

        private void InitializePawnPositions(int playerCount)
        {
            if (_currentPawnPositions != null) _currentPawnPositions.Clear();
            else _currentPawnPositions = new List<int>();

            for (int i = 0; i < playerCount; ++i)
                _currentPawnPositions.Add(0);

            if (_addedPawnPositions != null) _addedPawnPositions.Clear();
            else _addedPawnPositions = new List<int>();

            for (int i = 0; i < playerCount; ++i)
                _addedPawnPositions.Add(0);
        }

        //Used by everyone
        public Color GetPlayerColor(int playerID)
        {
            if (playerID >= _playerColors.Count)
            {
                Debug.LogWarning("Trying to access the color for player " + playerID + ". But there aren't that many colors");
                return Color.white;
            }

            return _playerColors[playerID];
        }

        //Used by the main menu
        public void SetPlayerCount(int playerCount)
        {
            if (_gameState != GameState.STATE_MAINMENU)
                return;

            if (playerCount < 2)
            {
                Debug.Log("Do you like having parties on your own?");
                return;
            }

            _playerCount = playerCount;
        }

        public void StartGame()
        {
            if (_gameState != GameState.STATE_MAINMENU)
                return;

            if (_playerCount < 2)
                return;

            InitializePawnPositions(_playerCount);

            if (_gameStartEvent != null)
                _gameStartEvent();

            _gameState = GameState.STATE_BOARD;
            _canStartMinigame = true;
        }

        //Used from the end menu
        public void ResetGame()
        {
            if (_gameState != GameState.STATE_RESULTMENU)
                return;

            _gameState = GameState.STATE_MAINMENU;

            if (_resetGameEvent != null)
                _resetGameEvent();
        }

        //Used by the board
        public int GetCurrentPawnPosition(int playerID)
        {
            if (playerID >= _currentPawnPositions.Count)
                return 0;

            return _currentPawnPositions[playerID];
        }

        public int GetAddedPawnPosition(int playerID)
        {
            if (playerID >= _addedPawnPositions.Count)
                return 0;

            return _addedPawnPositions[playerID];
        }

        public void OnAllPawnsMoved()
        {
            if (_gameState != GameState.STATE_BOARD)
                return;

            //Increase the player positions
            for (int i = 0; i < _currentPawnPositions.Count; ++i)
            {
                _currentPawnPositions[i] += _addedPawnPositions[i];
                _addedPawnPositions[i] = 0;
            }

            //Allow starting a minigame!
            _canStartMinigame = true;
        }

        public void PlayerFinished(List<int> results)
        {
            if (_gameState != GameState.STATE_BOARD)
                return;

            _gameState = GameState.STATE_RESULTMENU;

            //1 or more players finished this turn meaning that the game is over!
            if (_gameEndEvent != null)
                _gameEndEvent(results);
        }

        public void StartMinigame()
        {
            if (!_canStartMinigame)
                return;

            _canStartMinigame = false;

            if (_minigameStartEvent != null)
                _minigameStartEvent();
        }

        //Used by the minigames
        public void SubmitGameResults(List<int> results)
        {
            if (_gameState != GameState.STATE_MINIGAME)
                return;

            //Copy results
            if (_addedPawnPositions.Count < results.Count)
            {
                Debug.Log("Received more results than active players!");
                return;
            }

            if (_addedPawnPositions.Count > results.Count)
            {
                Debug.Log("Received less results than active players!");
                return;
            }

            for (int i = 0; i < results.Count; ++i)
            {
                if (results[i] >= _numStepsAwarded.Count)
                {
                    _addedPawnPositions[i] = 1;
                }
                else
                {
                    _addedPawnPositions[i] = _numStepsAwarded[results[i]];
                    Debug.Log("Player " + i + " advanced " + _addedPawnPositions[i] + " step(s)!");
                }
            }

            //Go back to the board
            _gameState = GameState.STATE_BOARD;
            Application.LoadLevel(_boardLevelID);
        }

        public int GetPlayerTeamID(int playerID, MinigameMode minigameMode)
        {
            //Determine the player positions
            List<int> sortedPlayerList = GetSortedPlayerList();

            switch (minigameMode)
            {
                case MinigameMode.MODE_FFA:
                    {
                        return playerID;
                    }

                case MinigameMode.MODE_2V2:
                    {
                        //If you're the first or the last, you're in the same team.
                        if (playerID == sortedPlayerList[0] || playerID == sortedPlayerList[sortedPlayerList.Count - 1])
                            return 0;
                        else
                            return 1;
                    }

                case MinigameMode.MODE_1V3:
                    {
                        if (playerID == sortedPlayerList[0])
                            return 0;
                        else
                            return 1;
                    }

                default:
                    {
                        return 0;
                    }
            }
        }

        private List<int> GetSortedPlayerList()
        {
            List<KeyValuePair<int, int>> tempList = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < _playerCount; ++i)
            {
                tempList.Add(new KeyValuePair<int, int>(i, _currentPawnPositions[i]));
            }
            tempList.Sort((x, y) => -1 * x.Value.CompareTo(y.Value));

            List<int> sortedList = new List<int>();
            for (int i = 0; i < tempList.Count; ++i)
            {
                sortedList.Add(tempList[i].Key);
            }

            return sortedList;
        }

        public void OnLevelWasLoaded(int level)
        {
            if (level != _boardLevelID)
                _gameState = GameState.STATE_MINIGAME;
        }
    }
}