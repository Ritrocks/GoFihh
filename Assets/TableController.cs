using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    [SerializeField] DrawnCard drawnCard;
    [SerializeField] TableSlot discardSlot;

    void OnEnable()  => TableSlot.OnSlotClicked += HandleSlotClicked;
    void OnDisable() => TableSlot.OnSlotClicked -= HandleSlotClicked;

    void HandleSlotClicked(TableSlot clickedSlot)
    {
        Card tableCard = clickedSlot.Card;
        Card drawn = drawnCard.Card;

        clickedSlot.Deal(drawn);
        drawnCard.Hide();
        discardSlot.Deal(tableCard); 
        discardSlot.Show();
    }
}
