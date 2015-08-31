using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinigameManager : MonoBehaviour
{
    [SerializeField]
    private List<string> _minigamesFFA;

    [SerializeField]
    private List<string> _minigames2v2;
    
    [SerializeField]
    private List<string> _minigames1v3;
    
    private void Awake()
    {
        GlobalGameManager.Instance.MiniGameStartEvent += OnStartMinigame;
    }

    private void OnDestroy()
    {
        GlobalGameManager.Instance.MiniGameStartEvent -= OnStartMinigame;
    }

    private void OnStartMinigame()
    {
        if (GlobalGameManager.Instance.GetGameState() != GameState.STATE_BOARD)
            return;

        Debug.Log("Starting minigame!");

        //Determine random gamemode

        //Get a random minigame from that pool
        //Determine the teams
        Application.LoadLevel(_minigamesFFA[0]);

    }
}
