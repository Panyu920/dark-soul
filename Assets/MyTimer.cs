using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    public enum State
    {
        IDLE,
        RUN,
        FINISHED
    }

    public State state;

    public float duration;
    private float elapsedTime;

    public void Tick(float dt)
    {
        elapsedTime += dt;

        switch (state)
        {
            case State.IDLE:
                break;
            case State.RUN:
                if (elapsedTime < duration)
                {
                    state = State.RUN;
                }
                else
                {
                    state = State.FINISHED;
                }

                break;
            case State.FINISHED:
                break;
            default:
                Debug.Log("error");
                break;
        }
    }

    public void go()
    {
        state = State.RUN;
        elapsedTime = 0;
    } 
}
