using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverSlots : MonoBehaviour
{
    public List<GameObject> slots = new();

    public void SetSlot(int i, GameObject card)
    {
        card.transform.SetParent(slots[i].transform);
        slots[i] = card;
        card.transform.localPosition = Vector3.zero;
        card.transform.localRotation = Quaternion.identity;
    }
}
