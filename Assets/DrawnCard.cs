using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawnCard : CardRenderer
{
    Card card;
    public Card Card => card;
    [SerializeField]Image image;
    void Start()
    {
        image.enabled = false;
    }
    public override void Deal(Card drawn)
    {
        card = drawn;
        SetCard(MakeMyFace(drawn.number, drawn.suite));
    }

    public override void SetCard(Sprite sprite)
    {
     image.sprite = sprite;   
    image.enabled = true;
    }
    public void Hide()
    {
        image.enabled = false;
    }
}
