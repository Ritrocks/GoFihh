using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dealer : MonoBehaviour
{
    CardDeck deck;
    [SerializeField] playerHand playerHand;
    [SerializeField] enemyHand enemyHand;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Text text;
    Moves playerMove;
    Moves enemyMove;
    List<Hand> hands;
    FSM fsm;
    // Start is called before the first frame update
    void Start()
    {
     deck = new CardDeck();
    hands = new(){playerHand, enemyHand};

     deck.shuffle();
     DealToPlayers();
     StartCoroutine(StartingSequence());
     fsm = FSM.Instance;
     fsm.FinishedTurn(GameStates.dealing);
    }
    IEnumerator StartingSequence()
    {
        foreach(Hand hand in hands) hand.showStartingCards();
        yield return new WaitForSeconds(3f);
        foreach(Hand hand in hands) hand.hideStartingCards();
    }
    
    void DealToPlayers()
    {
        for(int i = 0; i<4; i++)
        {
            Card card = deck.Pop();
            
           // if(card == null){Debug.LogError("dealing null");}
            playerHand.Deal(card, i);
            card = deck.Pop();
            enemyHand.Deal(card, i);
        }
    }
    public Card Deal()
    {
        return deck.Pop();
    }

}
public enum Moves{
    check,
    raise,
    match,
    fold
}
