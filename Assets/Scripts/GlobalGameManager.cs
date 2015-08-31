using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void VoidDelegate();

public enum GameState
{
    STATE_MAINMENU,
    STATE_BOARD,
    STATE_MINIGAME
}

public class GlobalGameManager : Singleton<GlobalGameManager>
{
    //Datamembers
    [SerializeField]
    private List<Color> _playerColors;

    [SerializeField]
    private List<int> _numStepsAwarded;

    private int _playerCount = 2;
    private GameState _gameState;
    private int _boardLevelID = 1;
    private bool _canStartMinigame = true;

    private List<int> _currentPawnPositions;
    private List<int> _addedPawnPositions;

    //Events
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

    private event VoidDelegate _gameEndEvent;
    public VoidDelegate GameEndEvent
    {
        get { return _gameEndEvent; }
        set { _gameEndEvent = value; }
    }

    //Functions
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
    public GameState GetGameState()
    {
        return _gameState;
    }

    public int GetPlayerCount()
    {
        return _playerCount;
    }

    public Color GetPlayerColor(int playerID)
    {
        if (playerID >= _playerColors.Count)
        {
            Debug.LogWarning("Trying to access the color for player " + playerID + ". But there aren't that many colors");
            return Color.white;
        }

        return _playerColors[playerID];
    }

    public int GetBoardLevelID()
    {
        return _boardLevelID;
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

        InitializePawnPositions(_playerCount);

        if (_gameStartEvent != null)
            _gameStartEvent();

        _gameState = GameState.STATE_BOARD;
    }

    //Used by the board
    public int GetCurrentPawnPosition(int id)
    {
        if (id >= _currentPawnPositions.Count)
            return 0;

        return _currentPawnPositions[id];
    }

    public int GetAddedPawnPosition(int id)
    {
        if (id >= _addedPawnPositions.Count)
            return 0;

        return _addedPawnPositions[id];
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
            
        for (int i = 0; i < results.Count; ++i)
        {
            if (results[i] >= _numStepsAwarded.Count)
            {
                _addedPawnPositions[i] = 1;
            }
            else
            {
                _addedPawnPositions[i] = _numStepsAwarded[results[i]];
                Debug.Log("Player " + i + " got first place and advanced " + _addedPawnPositions[i] + " steps!");
            }
        }

        //Go back to the board
        _gameState = GameState.STATE_BOARD;
        Application.LoadLevel(_boardLevelID);   
    }

    public void OnLevelWasLoaded(int level)
    {
        if (level != _boardLevelID)
            _gameState = GameState.STATE_MINIGAME;
    }
}
