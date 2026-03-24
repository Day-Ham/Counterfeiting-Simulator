using System;
using UnityEngine;

public static class GameState
{
    public static bool GameFinished = false;

    public static event Action OnGameStarted;
    public static event Action OnGameFinished;

    public static void GameStart()
    {
        GameFinished = false;
        OnGameStarted?.Invoke(); //notify all systems
    }

    public static void FinishGame()
    {
        if (GameFinished) return; //prevent multiple invocations
        
        GameFinished = true;
        OnGameFinished?.Invoke(); //notify all systems
    }
}
