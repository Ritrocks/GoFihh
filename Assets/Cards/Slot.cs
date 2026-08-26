using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField]List<Sprite> faceList;
    [SerializeField] Sprite Backside;
    Card card;
    public Card Card => card;
   #region visuals
   void OnEnable()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
    }
    public void Deal(Card dealtCard)
    {
        if(dealtCard == null) {Debug.LogError("dealt card is null"); return;}
        card = dealtCard;
        Hide();
    }

    public void Show()
    {
        if (card == null) { Debug.LogError("Show called with no card set", this); return; }

        spriteRenderer.sprite = MakeMyFace(card.number, card.suite);
    }
    public void Hide()
    {
        spriteRenderer.sprite = Backside;
    }
   
    Sprite MakeMyFace(int number, Suites suite)
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
        Debug.Log(i);
        return faceList[i];
    }
    #endregion

    void OnMouseDown()
    {
        Debug.Log("clicked");
    }
}
