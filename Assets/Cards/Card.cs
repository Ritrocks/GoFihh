using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

[System.Serializable]
public class Card{
    public Suites suite;
    public int number;
    public Sprite face;
    public Sprite Face => face;
    [SerializeField] List<Sprite> faceList;
    public Card(int value, Suites newSuite, List<Sprite> sprites){
        number = value;
        suite = newSuite;
        faceList = sprites;
        MakeMyFace();
    }

    void MakeMyFace()
    {
        int i = 0;
        switch (suite)
        {
            case Suites.Hearts:
            break;
            case Suites.Spades:
            i+=12;
            break;
            case Suites.Clubs:
            i+= 24;
            break;
            case Suites.Diamonds:
            i+=36;
            break;
            default:
            break;
        }
        i+=number;
        face = faceList[i];
    }
}