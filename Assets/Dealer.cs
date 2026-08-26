using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    CardDeck deck;
    List<Card> river = new();
    [SerializeField]RiverSlots riverSlots;
    [SerializeField] playerHand playerHand;
    [SerializeField]List<Sprite> faceList = new();
    [SerializeField] GameObject cardPrefab;
    Moves playerMove;
    Moves enemyMove;
    // Start is called before the first frame update
    void Start()
    {
     deck = new CardDeck(faceList);
     deck.shuffle();
     DealToPlayers();
     playerHand.showStartingCards();
    }
    
    #region delete this
   /*  void DealFirstThree()
    {
        //this should all go into the river class
        for(int i = 0; i<3; i++)
        
        {
            GameObject card = Instantiate(cardPrefab);
            riverSlots.SetSlot(i, card);
            river.Add(deck.Pop());
            card.GetComponent<SpriteRenderer>().sprite = river[i].Face;
        }
    } */
     #endregion
    void DealToPlayers()
    {
        for(int i = 0; i<4; i++)
        {
            Card card = deck.Pop();
            playerHand.Deal(card, i);
        }
    }

}
public enum Moves{
    check,
    raise,
    match,
    fold
}
