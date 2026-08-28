using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hand : MonoBehaviour
{
    [SerializeField] protected List<TableSlot> hand = new();
    public abstract void Deal(Card card, int i);
    public abstract void showStartingCards();
    public abstract void hideStartingCards();
    public void ShowAll()
    {
        foreach (TableSlot slot in hand)
        {
            slot.Show();
        }
    }
    public List<Card> CardsInHand()
    {
        List<Card> list = new();
        foreach(TableSlot slot in hand) list.Add(slot.Card);
        return list;
    }
    public abstract String GetIdentity();
}
