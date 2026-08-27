using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    FSM fsm;
    MemoryAI memory;
    // Start is called before the first frame update
    void Start()
    {
        memory = new();
        fsm = FSM.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        memory.Process();
        if(fsm.State == GameStates.enemyTurn)
        {
            fsm.FinishedTurn(GameStates.enemyTurn);
        }
    }
    public void Show(Card card, int slot)
    {
        memory.CommitToMemory(card, slot);
    }
   
}
