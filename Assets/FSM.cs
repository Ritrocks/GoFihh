using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class FSM : MonoBehaviour
{
   public static FSM Instance {get; private set;}
   GameStates state;
   [SerializeField]TurnText turnText;
   public GameStates State => state;
   bool cambio = false;
   int turnCounter = 0;
   public static event Action OnEndGame;
   
    void Awake()
    {
        // 2. Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            // Destroy this duplicate object immediately
            Destroy(gameObject);
            return;
        }

        // 3. Set the definitive instance
        Instance = this;

        // 4. Optional: Keep this object alive when switching scenes
        DontDestroyOnLoad(gameObject);
        state = GameStates.dealing;
    }
    public void FinishedTurn(GameStates finishedState)
    {
        if(cambio) turnCounter++; 
        if(turnCounter>0) {OnEndGame?.Invoke(); return;}
        state = finishedState == GameStates.enemyTurn
        ? GameStates.playerTurn
        : GameStates.enemyTurn;
        turnText.updateText(state.ToString());
    }
    public void Cambio()
    {
        cambio = true;
    }
}

public enum GameStates
{
    playerTurn,
    enemyTurn,
    dealing
}
