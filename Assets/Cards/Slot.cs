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

    // How long a Show() reveal stays face-up before flipping back.
    public float RevealDuration => showingAnimation != null ? showingAnimation.length : 0f;

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
        Debug.Log($"[TableSlot] Mouse down on {name} (holds {(card != null ? $"{card.number} of {card.suite}" : "nothing")})");
        Click();
    }
    public void Click()
    {
        Debug.Log($"[TableSlot] Click() on {name}, no drawn card passed, finishPlayerTurn=true");
        OnSlotClicked?.Invoke(this, null, true);
    }

    public void Click(Card drawn, bool finishPlayerTurn)
    {
        Debug.Log($"[TableSlot] Click() on {name}, drawn={(drawn != null ? $"{drawn.number} of {drawn.suite}" : "null")}, finishPlayerTurn={finishPlayerTurn}");
        OnSlotClicked?.Invoke(this, drawn, finishPlayerTurn);
    }

}
