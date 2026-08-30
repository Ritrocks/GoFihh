using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class DrawStack : MonoBehaviour
{
    FSM fsm;
    [SerializeField] Dealer dealer;
    [SerializeField] CardRenderer drawnCard;
    AudioSource audio;
    void OnEnable()
    {
        audio = GetComponent<AudioSource>();
        fsm = FSM.Instance;
    }
    void OnMouseDown()
    {
        if(fsm.State != GameStates.playerTurn) return;
        Card card = dealer.Deal();
        drawnCard.Deal(card);
        audio.Play();
    }
    public Card Click()
    {
        Card card = dealer.Deal();
        return card;
    }
}
