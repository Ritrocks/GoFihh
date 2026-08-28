using System.Collections.Generic;
using UnityEngine;

public class Card 
{
    public Suites suite;
    public int number;
    public Sprite face;
    public Sprite Face => face;
    public CardAbility ability;
     public Card(int value, Suites newSuite){
        number = value;
        suite = newSuite;
        WhatsMyAbility();
    } 
    public void WhatsMyAbility()
    {
        switch (number)
        {
            case 6: case 7:
            ability = CardAbility.PeekSelf;
            break;
            case 8: case 9:
            ability = CardAbility.PeekOpponent;
            break;
            case 10:
            ability = CardAbility.BlindSwap;
            break;
            case 11:
            ability = CardAbility.PeekSwap;
            break;
            default:
            ability = CardAbility.None;
            break;
        }
    }
    
}
public enum CardAbility
{
    None,
    PeekSwap,
    BlindSwap,
    PeekSelf,
    PeekOpponent,
}
