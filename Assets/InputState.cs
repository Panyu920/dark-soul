using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState 
{
    public bool OnPressing;
    public bool OnPress;
    public bool OnRelease;
    public bool OnDoubleClick;
    public bool OnExtending;
    public bool OnDelaying;
    public float doubleDuration = 0.2f;
    public float extendingDuration = 0.5f;
    public float delayingDuration = 0.5f;

    private bool currentState;
    private bool lastState;

    // private float lastTime;

    private MyTimer doubleClickTimer = new();
    private MyTimer extendingTimer = new();
    private MyTimer delayingTimer = new();
    
    

    public void Tick(bool inputState,float dt)
    {
        currentState = inputState;
        OnPressing = currentState;

        OnPress=false;
        OnRelease = false;
        OnDoubleClick = false;
        OnDelaying = false;
        OnExtending = false;

        doubleClickTimer.Tick(dt);
        delayingTimer.Tick(dt);
        extendingTimer.Tick(dt);
        if(currentState != lastState)
        {
            if(currentState)
            {
                OnPress = true;
                // if( lastTime < doubleDuration)
                // {
                //     OnDoubleClick = true;
                //     lastTime = 0;
                // }
                // else
                // {
                //     lastTime = 0;
                // }
                StartTimer(delayingTimer,delayingDuration);

                if (doubleClickTimer.state != MyTimer.State.RUN)
                {
                    StartTimer(doubleClickTimer,doubleDuration);
                }else
                {
                    OnDoubleClick = doubleClickTimer.state == MyTimer.State.RUN;
                }
            }else
            {
                OnRelease = true;
                StartTimer(extendingTimer,extendingDuration);
            }
        }

        if(delayingTimer.state == MyTimer.State.RUN)
        {
            OnDelaying = true;
        }

        if(extendingTimer.state == MyTimer.State.RUN)
        {
            OnExtending = true;
            // Debug.Log(1111);
        }

        lastState = currentState;
    }

    private void StartTimer(MyTimer timer, float duration)
    {
        timer.duration = duration;
        timer.go();
    }
}
