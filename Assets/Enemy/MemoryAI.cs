using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Analytics;

public class MemoryAI 
{
    public List<Card> myCards;
    List<Card> enemyCards;
   public Card HighestCard()
    {
        Card highest = null;
        foreach (Card card in myCards)
        {
            if (card == null) continue;
            if (highest == null || card.number > highest.number)
            {
                highest = card;
            }
        }
        return highest;
    }
    public int indexOf(Card card)
    {
        return myCards.IndexOf(card);
    }
    public MemoryAI()
    {
        myCards  = new List<Card>(new Card[4]);
        enemyCards = new List<Card>(new Card[4]);
    }
    public void CommitToMemory(Card card, int i)
    {
        Debug.Log("remembering: " + card.number + "of " + card.suite.ToString());
        myCards[i] = card;
    }
    public int SumAllCards()
    {
        int score = 0;
        foreach(Card card in myCards)
        {
            if(card == null){score+=5; continue;}
            score+= card.number +1;
        }
        return score;
    }
    public void Process()
    {
       /*  for(int i = 0; i<myCards.Count; i++)
        {
            if(myCards[i] == null)continue;
            if (Random.Range(0, 100) < 2)
            {
                myCards[i] = null;
            }
        }
        for(int i = 0; i<enemyCards.Count; i++)
        {
            if(enemyCards[i]==null) continue;
            if (Random.Range(0, 100) < 4)
            {
                enemyCards[i] = null;
            }
        } */
    }
}
