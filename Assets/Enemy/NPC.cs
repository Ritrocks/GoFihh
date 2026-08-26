using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    MemoryAI memory;
    // Start is called before the first frame update
    void Start()
    {
        memory = new();
    }

    // Update is called once per frame
    void Update()
    {
        
        memory.Process();
    }
    public void Show(Card card, int slot)
    {
        memory.CommitToMemory(card, slot);
    }
   
}
