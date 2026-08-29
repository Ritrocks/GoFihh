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

    private TableSlot pendingSwapFirst;

    void OnEnable()  => TableSlot.OnSlotClicked += HandleSlotClicked;
    void OnDisable() => TableSlot.OnSlotClicked -= HandleSlotClicked;

    void HandleSlotClicked(TableSlot clickedSlot, Card drawn, bool finishPlayerTurn)
    {
        if (drawn == null && drawnCard != null)
            drawn = drawnCard.Card;
        if (clickedSlot == discardSlot)
        {
            if (drawn != null)
            {
                discardSlot.Deal(drawn);
                discardSlot.Show(false);
                ClearDrawnCard();
                if (finishPlayerTurn)
                    FSM.Instance.FinishedTurn(GameStates.playerTurn);
            }
            return;
        }

        if (drawn != null && (drawn.ability == CardAbility.BlindSwap || drawn.ability == CardAbility.PeekSwap))
        {
            if (pendingSwapFirst == null)
            {
                pendingSwapFirst = clickedSlot;
                Debug.Log("Select a second card to swap.");
                return;
            }

            if (pendingSwapFirst == clickedSlot)
            {
                pendingSwapFirst = null;
                Debug.Log("Selection cleared.");
                return;
            }

            var ctx = new GameContext
            {
                PlayerHand = playerHand,
                EnemyHand = enemyHand,
                Deck = dealer != null ? dealer.Deck : null,
                State = FSM.Instance != null ? FSM.Instance.State : GameStates.playerTurn,
                Table = this,
                FirstTarget = pendingSwapFirst,
                SecondTarget = clickedSlot
            };

            if (AbilityResolver.TryResolve(drawn, pendingSwapFirst, clickedSlot, ctx))
            {
                discardSlot.Deal(drawn);
                discardSlot.Show(false);
                ClearDrawnCard();
                pendingSwapFirst = null;
                if (finishPlayerTurn)
                    FSM.Instance.FinishedTurn(GameStates.playerTurn);
                return;
            }

            pendingSwapFirst = null;
        }

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
                Debug.Log("ability was resolved: " + drawn.ability.ToString());
                discardSlot.Deal(drawn);
                discardSlot.Show(false);
                ClearDrawnCard();
                if (finishPlayerTurn)
                    FSM.Instance.FinishedTurn(GameStates.playerTurn);
                return;
            }
        }

        Card tableCard = clickedSlot.Card;
        clickedSlot.Deal(drawn);
        ClearDrawnCard();
        discardSlot.Deal(tableCard); 
        discardSlot.Show(false);
        if (finishPlayerTurn)
            FSM.Instance.FinishedTurn(GameStates.playerTurn);
    }

    void ClearDrawnCard()
    {
        if (drawnCard == null)
            return;

        drawnCard.Deal(null);
        drawnCard.Hide();
    }
}
