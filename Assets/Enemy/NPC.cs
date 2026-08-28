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
    [SerializeField] enemyHand hand;
    [SerializeField] playerHand playerHand;
    [SerializeField] DrawStack drawStack;
    [SerializeField] TableSlot discardPile;
    [SerializeField] bool debugEnemyDrawnCardVisible;
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

        DrawCard();
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f,2f));
        MakeDecision();
        fsm.FinishedTurn(GameStates.enemyTurn);
        playing = false;
    }
    void MakeDecision()
    {
        if (drawnCard != null && drawnCard.ability != CardAbility.None)
        {
            UseSpecialCard();
            return;
        }

        Card highestCard = memory.HighestCard();
        if (drawnCard.number < highestCard.number)
        {
            DiscardAndSwap(highestCard);
        }
        else Discard(highestCard);
        if(memory.SumAllCards()<12){fsm.Cambio(); Debug.Log("Cambio!");}
    }

    int PickUnknownSelfSlot()
    {
        return memory.FindUnknownSelfSlot();
    }

    int PickUnknownOpponentSlot()
    {
        return memory.FindUnknownEnemySlot();
    }

    int PickBestHighestSelfSlot()
    {
        int bestIndex = -1;
        Card bestCard = null;

        if (hand == null)
            return bestIndex;

        for (int i = 0; i < hand.Slots.Count; i++)
        {
            Card card = hand.Slots[i].Card;
            if (card == null)
                continue;

            if (bestCard == null || card.number > bestCard.number)
            {
                bestCard = card;
                bestIndex = i;
            }
        }

        return bestIndex;
    }

    int PickBestOpponentTargetForSwap(int highestNpcValue)
    {
        if (playerHand == null)
            return -1;

        int bestIndex = memory.LowestKnownEnemyCardIndex(highestNpcValue);
        if (bestIndex >= 0)
            return bestIndex;

        if (highestNpcValue > 6)
        {
            List<int> validIndexes = new List<int>();
            for (int i = 0; i < playerHand.Slots.Count; i++)
            {
                if (playerHand.Slots[i].Card != null)
                    validIndexes.Add(i);
            }

            if (validIndexes.Count > 0)
                return validIndexes[UnityEngine.Random.Range(0, validIndexes.Count)];
        }

        return -1;
    }

    void UseSpecialCard()
    {
        Debug.Log("ai using special card");
        switch (drawnCard.ability)
        {
            case CardAbility.PeekSelf:
                {
                    int targetIndex = PickUnknownSelfSlot();
                    if (targetIndex < 0)
                        targetIndex = 0;

                    if (hand != null)
                        hand.ClickSlot(targetIndex, drawnCard, false);
                    break;
                }
            case CardAbility.PeekOpponent:
                {
                    int targetIndex = PickUnknownOpponentSlot();
                    if (targetIndex < 0)
                    {
                        List<int> validIndexes = new List<int>();
                        for (int i = 0; i < playerHand.Slots.Count; i++)
                        {
                            if (playerHand.Slots[i].Card != null)
                                validIndexes.Add(i);
                        }

                        if (validIndexes.Count > 0)
                            targetIndex = validIndexes[UnityEngine.Random.Range(0, validIndexes.Count)];
                    }

                    if (targetIndex >= 0 && playerHand != null)
                        playerHand.ClickSlot(targetIndex, drawnCard, false);
                    break;
                }
            case CardAbility.BlindSwap:
            case CardAbility.PeekSwap:
                {
                    int npcHighestIndex = PickBestHighestSelfSlot();
                    if (npcHighestIndex < 0)
                        break;

                    int targetIndex = PickBestOpponentTargetForSwap(GetCardValueAtIndex(hand, npcHighestIndex));
                    if (targetIndex < 0)
                        break;

                    if (hand != null)
                        hand.ClickSlot(npcHighestIndex, drawnCard, false);
                    if (playerHand != null)
                        playerHand.ClickSlot(targetIndex, drawnCard, false);
                    break;
                }
        }
        Discard(drawnCard);
        drawnCard = null;
    }

    int GetCardValueAtIndex(Hand targetHand, int index)
    {
        if (targetHand == null || index < 0 || index >= targetHand.Slots.Count)
            return -1;

        Card card = targetHand.Slots[index].Card;
        return card == null ? -1 : card.number;
    }
    
    void DiscardAndSwap(Card card)
    {
        int targetIndex = memory.indexOf(card);
        if (targetIndex >= 0 && targetIndex < hand.Slots.Count)
            hand.ClickSlot(targetIndex, drawnCard, false);
        drawnCard = null;
    }
    void Discard(Card card)
    {
        discardPile.Click(drawnCard, false);
    }
    void DrawCard()
    {
        drawnCard = drawStack.Click();

        if (debugEnemyDrawnCardVisible)
        {
            Debug.Log("NPC drew: " + drawnCard.number + " of " + drawnCard.suite.ToString());
        }
    }
    public void Show(Card card, int slot)
    {
        memory.CommitToMemory(card, slot);
    }
   
}
