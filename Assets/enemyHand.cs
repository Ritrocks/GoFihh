using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHand : Hand
{
    //[SerializeField]List<TableSlot> hand = new();
    [SerializeField] NPC npc;
    [SerializeField] bool debugEnemyCardsVisible;

    public override string GetIdentity()
    {
        return "Enemy";
    }
    public override void Deal(Card card, int i)
    {
        hand[i].Deal(card);
        if (debugEnemyCardsVisible)
            hand[i].Show();
    }
    
    public override void showStartingCards()
    {
        int i = 0;
        while(i < 2)
        {
            npc.Show(hand[i].Card, i);
        //    Debug.Log("i see " + hand[i].Card.number + " " + hand[i].Card.suite.ToString());
            i++;
        }

        if (debugEnemyCardsVisible)
        {
            for (int j = 0; j < hand.Count; j++)
            {
                if (hand[j] != null && hand[j].Card != null)
                    hand[j].Show();
            }
        }
    }
    public override void hideStartingCards()
    {
      
    }
}
