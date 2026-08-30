using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnText : MonoBehaviour
{
    TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    public void updateText(String texttoupdate)
    {
        Debug.Log(texttoupdate);
        text.text = texttoupdate;
    }
}
