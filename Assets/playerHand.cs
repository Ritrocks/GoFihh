using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;

public class playerHand : MonoBehaviour, Hand
{
    [SerializeField]List<TableSlot> Hand = new();
    public void Deal(Card card, int i)
    {
        Hand[i].Deal(card);
    }

    public void showStartingCards()
    {
        int i = 0;
        while(i < 2)
        {
            Hand[i].Show();
            i++;
        }
    }
    public void hideStartingCards()
    {
         int i = 0;
        while(i < 2)
        {
            Hand[i].Hide();
            i++;
        }
    }
}
