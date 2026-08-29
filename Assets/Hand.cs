using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hand : MonoBehaviour
{
    [SerializeField] protected List<TableSlot> hand = new();
    public List<TableSlot> Slots => hand;

    public abstract void Deal(Card card, int i);
    public abstract void showStartingCards();
    public abstract void hideStartingCards();
    public void ShowAll()
    {
        foreach (TableSlot slot in hand)
        {
            slot.Show(false);
        }
    }
    public void SwapCards(int firstIndex, int secondIndex)
    {
        if (firstIndex < 0 || secondIndex < 0 || firstIndex >= hand.Count || secondIndex >= hand.Count)
            return;

        Card firstCard = hand[firstIndex].Card;
        Card secondCard = hand[secondIndex].Card;

        hand[firstIndex].Deal(secondCard);
        hand[secondIndex].Deal(firstCard);
    }

    public void ClickSlot(int index)
    {
        if (index < 0 || index >= hand.Count)
            return;

        hand[index].Click();
    }

    public void ClickSlot(int index, Card drawn, bool finishPlayerTurn)
    {
        if (index < 0 || index >= hand.Count)
            return;

        hand[index].Click(drawn, finishPlayerTurn);
    }

    public void SwapWithHand(Hand otherHand, int index)
    {
        if (otherHand == null)
            return;

        if (index < 0 || index >= hand.Count || index >= otherHand.hand.Count)
            return;

        Card myCard = hand[index].Card;
        Card otherCard = otherHand.hand[index].Card;

        hand[index].Deal(otherCard);
        otherHand.hand[index].Deal(myCard);
    }
    public List<Card> CardsInHand()
    {
        List<Card> list = new();
        foreach(TableSlot slot in hand) list.Add(slot.Card);
        return list;
    }
    public abstract String GetIdentity();
}
