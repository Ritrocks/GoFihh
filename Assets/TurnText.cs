using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnText : MonoBehaviour
{
    TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    public void updateText(String texttoupdate)
    {
        text.text = texttoupdate;
    }
}
