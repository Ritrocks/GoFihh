using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSlot : CardRenderer
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite backside;
    Card card;
    public Card Card => card;
    public static event Action<TableSlot, Card, bool> OnSlotClicked;
   #region visuals
   public override void SetCard(Sprite cardSprite)
    {
        spriteRenderer.sprite = cardSprite;
    }
   void OnEnable()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        SetCard(null);
    }
    public override void Deal(Card dealtCard)
    {
        if(dealtCard == null) return;
        card = dealtCard;
        Hide();
    }

    public void Show()
    {
        SetCard(MakeMyFace(card.number, card.suite));
    }
    public void Hide()
    {
        SetCard(backside);
    }
   
    #endregion

    void OnMouseDown()
    {
        Click();
    }
    public void Click()
    {
        OnSlotClicked?.Invoke(this, null, true);
    }

    public void Click(Card drawn, bool finishPlayerTurn)
    {
        OnSlotClicked?.Invoke(this, drawn, finishPlayerTurn);
    }

}
