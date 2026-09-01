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

    // The hand references are often left unassigned in the inspector, so fall back
    // to the dealer's wired hands (and then the scene) before giving up.
    playerHand ResolvedPlayerHand
    {
        get
        {
            if (playerHand == null && dealer != null)
                playerHand = dealer.PlayerHand;
            if (playerHand == null)
                playerHand = FindObjectOfType<playerHand>();
            if (playerHand == null)
                Debug.LogWarning("[TableController] Could not find a playerHand; peek targeting will fail.");
            return playerHand;
        }
    }

    enemyHand ResolvedEnemyHand
    {
        get
        {
            if (enemyHand == null && dealer != null)
                enemyHand = dealer.EnemyHand;
            if (enemyHand == null)
                enemyHand = FindObjectOfType<enemyHand>();
            if (enemyHand == null)
                Debug.LogWarning("[TableController] Could not find an enemyHand; peek targeting will fail.");
            return enemyHand;
        }
    }

    void HandleSlotClicked(TableSlot clickedSlot, Card drawn, bool finishPlayerTurn)
    {
        Debug.Log($"[TableController] Slot clicked: {SlotName(clickedSlot)}, drawnArg={(drawn == null ? "null" : $"{drawn.number} of {drawn.suite}")}, finishPlayerTurn={finishPlayerTurn}");

        if (drawn == null && drawnCard != null)
            drawn = drawnCard.Card;
        Debug.Log($"[TableController] Resolved drawn card: {(drawn != null ? $"{drawn.number} of {drawn.suite} (ability={drawn.ability})" : "null")}");

        // Nothing can be played without a drawn card in hand, so ignore the click entirely.
        if (drawn == null)
        {
            Debug.Log("[TableController] Ignoring click: no card has been drawn.");
            return;
        }

        if (clickedSlot == discardSlot)
        {
            Debug.Log($"[TableController] Clicked slot is the discard pile. Discarding drawn card {drawn.number} of {drawn.suite}.");
            discardSlot.Deal(drawn);
            discardSlot.Show(false);
            ClearDrawnCard();
            if (finishPlayerTurn)
                FSM.Instance.FinishedTurn(GameStates.playerTurn);
            return;
        }

        if (drawn.ability == CardAbility.BlindSwap || drawn.ability == CardAbility.PeekSwap)
        {
            Debug.Log($"[TableController] Swap-ability card in play ({drawn.ability}). pendingSwapFirst={SlotName(pendingSwapFirst)}");
            if (pendingSwapFirst == null)
            {
                pendingSwapFirst = clickedSlot;
                Debug.Log($"[TableController] First swap target set to {clickedSlot.name}. Select a second card to swap.");
                return;
            }

            if (pendingSwapFirst == clickedSlot)
            {
                pendingSwapFirst = null;
                Debug.Log("[TableController] Same slot clicked twice. Selection cleared.");
                return;
            }

            var ctx = new GameContext
            {
                PlayerHand = ResolvedPlayerHand,
                EnemyHand = ResolvedEnemyHand,
                Deck = dealer != null ? dealer.Deck : null,
                State = FSM.Instance != null ? FSM.Instance.State : GameStates.playerTurn,
                Table = this,
                FirstTarget = pendingSwapFirst,
                SecondTarget = clickedSlot
            };

            Debug.Log($"[TableController] Resolving {drawn.ability} between {pendingSwapFirst.name} and {clickedSlot.name} (state={ctx.State}).");
            bool swapResolved = AbilityResolver.TryResolve(drawn, pendingSwapFirst, clickedSlot, ctx);
            Debug.Log($"[TableController] {drawn.ability} resolution result: {swapResolved}");

            if (swapResolved)
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

        var singleCtx = new GameContext
        {
            PlayerHand = ResolvedPlayerHand,
            EnemyHand = ResolvedEnemyHand,
            Deck = dealer != null ? dealer.Deck : null,
            State = FSM.Instance != null ? FSM.Instance.State : GameStates.playerTurn,
            Table = this,
            TargetSlot = clickedSlot
        };

        Debug.Log($"[TableController] Resolving {drawn.ability} on slot {clickedSlot.name} (state={singleCtx.State}).");
        bool resolved = AbilityResolver.TryResolve(drawn, clickedSlot, singleCtx);
        Debug.Log($"[TableController] {drawn.ability} resolution result: {resolved}");

        if (resolved)
        {
            Debug.Log("ability was resolved: " + drawn.ability.ToString());
            discardSlot.Deal(drawn);
            discardSlot.Show(false);
            ClearDrawnCard();
            if (finishPlayerTurn)
                FSM.Instance.FinishedTurn(GameStates.playerTurn);
            return;
        }

        if (drawn.ability == CardAbility.PeekSelf || drawn.ability == CardAbility.PeekOpponent)
        {
            Debug.Log(drawn.ability == CardAbility.PeekSelf
                ? "[TableController] Rejected: you can only peek your own cards."
                : "[TableController] Rejected: you can only peek your opponent's cards.");
            return;
        }

        // A drawn card can only replace one of the active player's own cards.
        if (!singleCtx.IsOwnSlot(clickedSlot))
        {
            Debug.Log($"[TableController] Rejected: {clickedSlot.name} is not your card, you can only swap the drawn card into your own hand.");
            return;
        }

        Debug.Log($"[TableController] Falling back to normal placement on {clickedSlot.name}.");
        Card tableCard = clickedSlot.Card;
        clickedSlot.Deal(drawn);
        ClearDrawnCard();
        discardSlot.Deal(tableCard);
        discardSlot.Show(false);
        if (finishPlayerTurn)
            FSM.Instance.FinishedTurn(GameStates.playerTurn);
    }

    // Lets the peek-swap show both faces before the cards actually change places.
    public void SwapAfterReveal(TableSlot first, TableSlot second)
    {
        StartCoroutine(SwapAfterRevealRoutine(first, second));
    }

    IEnumerator SwapAfterRevealRoutine(TableSlot first, TableSlot second)
    {
        float wait = first == null ? 0f : first.RevealDuration;
        yield return new WaitForSeconds(wait);

        Debug.Log($"[TableController] Reveal finished, swapping {SlotName(first)} and {SlotName(second)}.");
        GameContext.SwapSlots(first, second);
    }

    static string SlotName(TableSlot slot) => slot == null ? "null" : slot.name;

    void ClearDrawnCard()
    {
        if (drawnCard == null)
            return;

        drawnCard.Deal(null);
        drawnCard.Hide();
    }
}
