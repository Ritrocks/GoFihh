using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NPC : MonoBehaviour
{
    FSM fsm;
    MemoryAI memory;
    bool playing;
    Card drawnCard;
    [SerializeField]enemyHand hand;
    [SerializeField]CardDeck deck;
    [SerializeField] TableSlot discardPile;
    // Start is called before the first frame update
    void Start()
    {
        memory = new();
        fsm = FSM.Instance;
        playing = false;
    }
    

    // Update is called once per frame
    void Update()
    {
        memory.Process();
        if (fsm.State == GameStates.enemyTurn && !playing)
    {
        playing = true;
        StartCoroutine(Play());
    }
    }
    IEnumerator Play()
    {
//        Debug.Log("playing");
        DrawCard();
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f,2f));
        MakeDecision();
        fsm.FinishedTurn(GameStates.enemyTurn);
        playing = false;
    }
    void MakeDecision()
    {
        Card highestCard = memory.HighestCard();
        if (drawnCard.number<highestCard.number)
        {
            DiscardAndSwap(highestCard);
        }
        else Discard(highestCard);
        if(memory.SumAllCards()<12){fsm.Cambio(); Debug.Log("Cambio!");}
    }
    
    void DiscardAndSwap(Card card)
    {
       // Debug.Log("swapping out:" + card.number + " " + card.suite.ToString());
        hand.Deal(drawnCard, memory.indexOf(card));
        memory.CommitToMemory(drawnCard, memory.indexOf(card));
        drawnCard = null;
        discardPile.Deal(card);
        discardPile.Show();
    }
    void Discard(Card card)
    {
       // Debug.Log("discarding: "+ drawnCard.number + " " + drawnCard.suite.ToString());
        discardPile.Deal(drawnCard);
        discardPile.Show();
    }
    void DrawCard()
    {
     drawnCard = deck.Pop();   
    }
    public void Show(Card card, int slot)
    {
        memory.CommitToMemory(card, slot);
    }
   
}
