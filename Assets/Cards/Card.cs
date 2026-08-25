using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card{
    public Suites suite;
    public int number;
    public Card(int value, Suites face){
        number = value;
        suite = face;
    }
}