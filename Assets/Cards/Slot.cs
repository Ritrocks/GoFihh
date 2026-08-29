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
    Animator animator;
    public static event Action<TableSlot, Card, bool> OnSlotClicked;

    private Quaternion restRotation;
    private Coroutine turnRoutine;
    [SerializeField]AnimationClip showingAnimation;
    Camera targetCamera; 
    Transform myTransform;
   #region visuals
   public override void SetCard(Sprite cardSprite)
    {
        spriteRenderer.sprite = cardSprite;
    }
   void OnEnable()
    {
        targetCamera = Camera.main;
        animator = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        SetCard(null);
    }
    public override void Deal(Card dealtCard)
    {
        if(dealtCard == null) return;
        card = dealtCard;
        Hide();
    }

    public void Show(bool animate)
    {
        Show(animate, true);
    }

    public void Show(bool animate, bool revealFace)
    {
        if (revealFace)
            SetCard(MakeMyFace(card.number, card.suite));
        if(!animate)return;
        animator.SetTrigger("Play");    
        if (turnRoutine != null) StopCoroutine(turnRoutine);
        turnRoutine = StartCoroutine(TurnTowardCamera(showingAnimation.length));
    }
   
    IEnumerator TurnTowardCamera(float duration){
        yield return new WaitForSeconds(duration);
        SetCard(backside);
      
    turnRoutine = null;
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
