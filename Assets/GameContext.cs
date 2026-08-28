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

    public void PeekSelf()
    {
        if (TargetSlot != null)
        {
            TargetSlot.Show();
        }
    }

    public void PeekOpponent()
    {
        if (TargetSlot != null)
        {
            TargetSlot.Show();
        }
    }

    public void BlindSwap()
    {
        if (TargetSlot == null || PlayerHand == null || EnemyHand == null)
            return;

        int indexToSwap = 0;
        PlayerHand.SwapWithHand(EnemyHand, indexToSwap);
    }

    public void PeekSwap()
    {
        PeekSelf();
        PeekOpponent();
        BlindSwap();
    }
}
