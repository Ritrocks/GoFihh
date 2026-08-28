using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityResolver
{
    public static bool TryResolve(Card playedCard, TableSlot targetSlot, GameContext ctx)
    {
        if (playedCard == null || ctx == null)
            return false;

        ctx.TargetSlot = targetSlot;

        switch (playedCard.ability)
        {
            case CardAbility.PeekSelf:
                ctx.PeekSelf();
                return true;
            case CardAbility.PeekOpponent:
                ctx.PeekOpponent();
                return true;
            case CardAbility.BlindSwap:
                ctx.BlindSwap();
                return true;
            case CardAbility.PeekSwap:
                ctx.PeekSwap();
                return true;
            default:
                return false;
        }
    }

    public static bool TryResolve(Card playedCard, GameContext ctx)
    {
        return TryResolve(playedCard, ctx?.TargetSlot, ctx);
    }
}

