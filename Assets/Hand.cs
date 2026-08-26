using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Hand
{
    public void Deal(Card card, int i);
    public void showStartingCards();
    public void hideStartingCards();
}
