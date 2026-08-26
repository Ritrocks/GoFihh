using System.Collections.Generic;
using UnityEngine;

public class Card 
{
    public Suites suite;
    public int number;
    public Sprite face;
    public Sprite Face => face;
     public Card(int value, Suites newSuite){
        number = value;
        suite = newSuite;
    } 
    
}