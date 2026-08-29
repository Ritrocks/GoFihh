using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public static FSM Instance { get; private set; }
    GameStates state;
    [SerializeField] private TurnText turnText;
    public GameStates State => state;
    bool cambio = false;
    int turnCounter = 0;
    public static event Action OnEndGame;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        FindTurnText();
        DontDestroyOnLoad(gameObject);
        state = GameStates.dealing;
    }

    private void FindTurnText()
    {
        if (turnText == null)
            turnText = FindObjectOfType<TurnText>();
    }

    public void FinishedTurn(GameStates finishedState)
    {
        if (cambio) turnCounter++;
        if (turnCounter > 0)
        {
            OnEndGame?.Invoke();
            return;
        }

        state = finishedState == GameStates.enemyTurn
            ? GameStates.playerTurn
            : GameStates.enemyTurn;

        if (turnText != null)
            turnText.updateText(state.ToString());
    }

    public void Cambio()
    {
        FindTurnText();
        if (turnText != null)
            turnText.updateText("Cambio.");

        Debug.Log("cambio");
        PlayCambioMusic();
        cambio = true;
    }

    void PlayCambioMusic()
    {
        if (TryGetComponent<AudioSource>(out var audioSource))
            audioSource.Play();
    }
}

public enum GameStates
{
    playerTurn,
    enemyTurn,
    dealing
}
