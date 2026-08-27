using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHand : MonoBehaviour, Hand
{
    [SerializeField]List<TableSlot> Hand = new();
    [SerializeField] NPC npc;
    public void Deal(Card card, int i)
    {
        Hand[i].Deal(card);
    }

    public void showStartingCards()
    {
        int i = 0;
        while(i < 2)
        {
            npc.Show(Hand[i].Card, i);
            i++;
        }
    }
    public void hideStartingCards()
    {
       /*   int i = 0;
        while(i < 2)
        {
            npc.Hide(Hand[i].Card, i);
            i++;
        } */
    }
}
