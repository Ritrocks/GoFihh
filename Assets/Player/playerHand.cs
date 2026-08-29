using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;

public class playerHand : Hand
{
    //[SerializeField]List<TableSlot> hand = new();
    public override string GetIdentity()
    {
        return "Player";
    }
    public override void Deal(Card card, int i)
    {
        hand[i].Deal(card);
    }

    public override void showStartingCards()
    {
        int i = 0;
        while(i < 2)
        {
            hand[i].Show(true);
            i++;
        }
    }
    public override void hideStartingCards()
    {
         int i = 0;
        while(i < 2)
        {
            hand[i].Hide();
            i++;
        }
    }

}
