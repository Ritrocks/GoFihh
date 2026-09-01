using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext 
{
    public Hand PlayerHand;
    public Hand EnemyHand;
    public CardDeck Deck;
    public GameStates State;
    public TableController Table;
    public TableSlot TargetSlot;
    public TableSlot FirstTarget;
    public TableSlot SecondTarget;

    Hand SelfHand => State == GameStates.godTurn ? EnemyHand : PlayerHand;
    Hand OpponentHand => State == GameStates.godTurn ? PlayerHand : EnemyHand;

    static bool HandContainsSlot(Hand hand, TableSlot slot)
    {
        return hand != null && slot != null && hand.Slots.Contains(slot);
    }

    // True when the slot belongs to whoever is currently taking their turn.
    public bool IsOwnSlot(TableSlot slot) => HandContainsSlot(SelfHand, slot);

    static string SlotName(TableSlot slot) => slot == null ? "null" : slot.name;
    static string HandName(Hand hand) => hand == null ? "null" : hand.GetIdentity();

    static string HandContents(Hand hand)
    {
        if (hand == null)
            return "null hand";

        List<string> names = new();
        foreach (TableSlot slot in hand.Slots)
            names.Add(SlotName(slot));
        return $"{hand.GetIdentity()} slots [{string.Join(", ", names)}]";
    }

    public bool PeekSelf()
    {
        Debug.Log($"[GameContext] PeekSelf on {SlotName(TargetSlot)} (state={State}, selfHand={HandName(SelfHand)})");

        if (TargetSlot == null || TargetSlot.Card == null)
        {
            Debug.Log("[GameContext] PeekSelf failed: target slot is null or empty.");
            return false;
        }

        if (!HandContainsSlot(SelfHand, TargetSlot))
        {
            Debug.Log($"[GameContext] PeekSelf failed: {TargetSlot.name} is not in the active player's own hand. Own hand = {HandContents(SelfHand)}");
            return false;
        }

        TargetSlot.Show(true, State != GameStates.godTurn);
        Debug.Log($"[GameContext] PeekSelf revealed {TargetSlot.Card.number} of {TargetSlot.Card.suite}.");
        return true;
    }

    public bool PeekOpponent()
    {
        Debug.Log($"[GameContext] PeekOpponent on {SlotName(TargetSlot)} (state={State}, opponentHand={HandName(OpponentHand)})");

        if (TargetSlot == null || TargetSlot.Card == null)
        {
            Debug.Log("[GameContext] PeekOpponent failed: target slot is null or empty.");
            return false;
        }

        if (!HandContainsSlot(OpponentHand, TargetSlot))
        {
            Debug.Log($"[GameContext] PeekOpponent failed: {TargetSlot.name} is not in the opponent's hand. Opponent hand = {HandContents(OpponentHand)}");
            return false;
        }

        TargetSlot.Show(true, State != GameStates.godTurn);
        Debug.Log($"[GameContext] PeekOpponent revealed {TargetSlot.Card.number} of {TargetSlot.Card.suite}.");
        return true;
    }

    public void BlindSwap()
    {
        Debug.Log($"[GameContext] BlindSwap between {SlotName(FirstTarget)} and {SlotName(SecondTarget)}");

        if (FirstTarget == null || SecondTarget == null || FirstTarget == SecondTarget)
        {
            Debug.Log("[GameContext] BlindSwap aborted: invalid target pair.");
            return;
        }

        Card firstCard = FirstTarget.Card;
        Card secondCard = SecondTarget.Card;

        if (firstCard == null || secondCard == null)
        {
            Debug.Log("[GameContext] BlindSwap aborted: one of the slots is empty.");
            return;
        }

        SwapSlots(FirstTarget, SecondTarget);
        Debug.Log($"[GameContext] BlindSwap done: {firstCard.number}{firstCard.suite} <-> {secondCard.number}{secondCard.suite}");
    }

    // Exchanges the cards held by two slots. Dealing hides both, so anything that
    // wants the faces visible must reveal them and swap afterwards.
    public static void SwapSlots(TableSlot first, TableSlot second)
    {
        if (first == null || second == null || first == second)
            return;

        Card firstCard = first.Card;
        Card secondCard = second.Card;

        if (firstCard == null || secondCard == null)
            return;

        first.Deal(secondCard);
        second.Deal(firstCard);
    }

    public void PeekSwap()
    {
        Debug.Log($"[GameContext] PeekSwap between {SlotName(FirstTarget)} and {SlotName(SecondTarget)}");

        if (FirstTarget == null || SecondTarget == null || FirstTarget == SecondTarget)
        {
            Debug.Log("[GameContext] PeekSwap aborted: invalid target pair.");
            return;
        }

        bool revealFace = State != GameStates.godTurn;
        FirstTarget.Show(true, revealFace);
        SecondTarget.Show(true, revealFace);

        // Swapping deals into both slots, which hides them again. Wait for the reveal
        // to finish first, otherwise the flip animation plays over a face-down card.
        if (revealFace && Table != null)
        {
            Debug.Log("[GameContext] PeekSwap revealing both cards, swap deferred until the reveal ends.");
            Table.SwapAfterReveal(FirstTarget, SecondTarget);
            return;
        }

        BlindSwap();
    }
}
