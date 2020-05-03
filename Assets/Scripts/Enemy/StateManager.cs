using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : AI
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public override void SwitchState(State state)
    {
        switch(state)
        {
            case State.NONE:
                break;
            case State.IDLE:
                break;
            case State.ATTACK:
                break;
            case State.PATROL:
                break;
            case State.ALERT:
                break;
           

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
