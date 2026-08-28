using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardRenderer :MonoBehaviour
{
    [SerializeField]List<Sprite> faceList;

    public abstract void SetCard(Sprite sprite);
    public abstract void Deal(Card card);
    protected Sprite MakeMyFace(int number, Suites suite)
    {
        int i = 0;
        switch (suite)
        {
            case Suites.Hearts:
            break;
            case Suites.Spades:
            i+=13;
            break;
            case Suites.Clubs:
            i+= 26;
            break;
            case Suites.Diamonds:
            i+=39;
            break;
            default:
            break;
        }
        i+=number;
        return faceList[i];
    }
}
