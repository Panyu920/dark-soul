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

    private float lastTime;
    
    

    public void Tick(bool inputState,float dt)
    {
        currentState = inputState;
        OnPressing = currentState;

        OnPress=false;
        OnRelease = false;
        lastTime += dt;
        OnDoubleClick = false;
        if(currentState != lastState)
        {
            if(currentState)
            {
                OnPress = true;
                if( lastTime < doubleDuration)
                {
                    OnDoubleClick = true;
                    lastTime = 0;
                }
                else
                {
                    lastTime = 0;
                }
            }else
            {
                OnRelease = true;
            }
        }
        lastState = currentState;
    }
}
