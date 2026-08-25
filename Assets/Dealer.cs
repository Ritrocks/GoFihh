using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    CardDeck deck;
    [SerializeField]List<Sprite> faceList = new();
    [SerializeField] GameObject cardPrefab;
    // Start is called before the first frame update
    void Start()
    {
     deck = new CardDeck(faceList);
     deck.logDeck();
     deck.shuffle();
     deck.logDeck();   
     Deal();
    }
    
    void Deal()
    {
        GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
        card.GetComponent<SpriteRenderer>().sprite = deck.Pop().Face;
    }
}
