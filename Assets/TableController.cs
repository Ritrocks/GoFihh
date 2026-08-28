using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    [SerializeField] DrawnCard drawnCard;
    [SerializeField] TableSlot discardSlot;
    [SerializeField] Dealer dealer;
    [SerializeField] playerHand playerHand;
    [SerializeField] enemyHand enemyHand;

    void OnEnable()  => TableSlot.OnSlotClicked += HandleSlotClicked;
    void OnDisable() => TableSlot.OnSlotClicked -= HandleSlotClicked;

    void HandleSlotClicked(TableSlot clickedSlot)
    {
        Card tableCard = clickedSlot.Card;
        Card drawn = drawnCard.Card;

        if (drawn != null)
        {
            var ctx = new GameContext
            {
                PlayerHand = playerHand,
                EnemyHand = enemyHand,
                Deck = dealer != null ? dealer.Deck : null,
                State = FSM.Instance != null ? FSM.Instance.State : GameStates.playerTurn,
                Table = this,
                TargetSlot = clickedSlot
            };

            if (AbilityResolver.TryResolve(drawn, clickedSlot, ctx))
            {
                drawnCard.Deal(null);
                drawnCard.Hide();
                FSM.Instance.FinishedTurn(GameStates.playerTurn);
                return;
            }
        }

        clickedSlot.Deal(drawn);
        drawnCard.Hide();
        discardSlot.Deal(tableCard); 
        discardSlot.Show();
        FSM.Instance.FinishedTurn(GameStates.playerTurn);
    }
}
