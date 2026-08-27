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
    public static event Action<TableSlot> OnSlotClicked;
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
        if(dealtCard == null) {Debug.LogError("dealt card is null"); return;}
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
        Debug.Log("clicked");
        OnSlotClicked?.Invoke(this);
    }

}
