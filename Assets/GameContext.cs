using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext 
{
    public Hand PlayerHand;
    public Hand EnemyHand;
    public CardDeck Deck;
    public GameStates State;
    public TableController Table;
    public TableSlot TargetSlot;
    public TableSlot FirstTarget;
    public TableSlot SecondTarget;

    public void PeekSelf()
    {
        if (TargetSlot != null && TargetSlot.Card != null)
        {
            TargetSlot.Show();
        }
    }

    public void PeekOpponent()
    {
        if (TargetSlot != null && TargetSlot.Card != null)
        {
            TargetSlot.Show();
        }
    }

    public void BlindSwap()
    {
        if (FirstTarget == null || SecondTarget == null || FirstTarget == SecondTarget)
            return;

        Card firstCard = FirstTarget.Card;
        Card secondCard = SecondTarget.Card;

        if (firstCard == null || secondCard == null)
            return;

        FirstTarget.Deal(secondCard);
        SecondTarget.Deal(firstCard);
    }

    public void PeekSwap()
    {
        if (FirstTarget == null || SecondTarget == null || FirstTarget == SecondTarget)
            return;

        FirstTarget.Show();
        SecondTarget.Show();
        BlindSwap();
    }
}
