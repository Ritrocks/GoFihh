using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardDeck 
{
    List<Card> deck;
    System.Random rng = new();
    public CardDeck(List<Sprite> faceList){
        deck = new List<Card>();
        foreach(Suites suite in Enum.GetValues(typeof(Suites))){
            for(int i = 0; i <13; i++){
                deck.Add(new Card(i, suite, faceList));
            }
        }
    }

    public void shuffle(){
        int n = 52;
        while(n>1){
            n--;
            int k = rng.Next(n+1);
            var value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;    
        }
        Debug.Log("shuffled");
    }

    public void logDeck(){
        foreach (var card in deck)
        {
            Debug.Log(card.number.ToString() + " " + card.suite.ToString());
        }
    }

    public Card Pop()
    {
        Card victim = deck[0];
        int i = 0;
        while(i<deck.Count-1)
        {
            deck[i] = deck [i+1];
            i++;
        }
        return victim;
    }
}

public enum Suites{
    Spades,
    Hearts,
    Clubs,
    Diamonds
}