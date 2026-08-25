using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    CardDeck deck;
    // Start is called before the first frame update
    void Start()
    {
     deck = new CardDeck();
     deck.logDeck();
     deck.shuffle();
     deck.logDeck();   
    }
    
    // Update is called once per frame
    void Update()
    {
        }
}
