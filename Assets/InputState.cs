using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState 
{
    public bool OnPressing;
    public bool OnPress;
    public bool OnRelease;
    public bool OnDoubleClick;
    public float doubleDuration = 0.2f;

    private bool currentState;
    private bool lastState;

    // private float lastTime;

    private MyTimer doubleClickTimer = new();
    
    

    public void Tick(bool inputState,float dt)
    {
        currentState = inputState;
        OnPressing = currentState;

        OnPress=false;
        OnRelease = false;
        OnDoubleClick = false;

        doubleClickTimer.Tick(dt);
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
            }
        }
        lastState = currentState;
    }

    private void StartTimer(MyTimer timer, float duration)
    {
        timer.duration = duration;
        timer.go();
    }
}
