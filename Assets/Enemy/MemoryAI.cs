using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Analytics;

public class MemoryAI 
{
    List<Card> myCards;
    List<Card> enemyCards;
    public MemoryAI()
    {
        myCards = new();
        enemyCards = new();
    }
    public void CommitToMemory(Card card, int i)
    {
        myCards.Add(card);
    }
    public void Process()
    {
        for(int i = 0; i<myCards.Count; i++)
        {
            if(myCards[i] == null)continue;
            if (Random.Range(0, 10) < 2)
            {
                myCards[i] = null;
            }
        }
        for(int i = 0; i<enemyCards.Count; i++)
        {
            if(enemyCards[i]==null) continue;
            if (Random.Range(0, 10) < 4)
            {
                enemyCards[i] = null;
            }
        }
    }
}
