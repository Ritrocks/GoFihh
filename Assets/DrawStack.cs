using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class DrawStack : MonoBehaviour
{
    FSM fsm;
    [SerializeField] Dealer dealer;
    [SerializeField] CardRenderer drawnCard;
    void OnEnable()
    {
        fsm = FSM.Instance;
    }
    void OnMouseDown()
    {
        if(fsm.State != GameStates.playerTurn) return;
        Card card = dealer.Deal();
        drawnCard.Deal(card);
    }
}
