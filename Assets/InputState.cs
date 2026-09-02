using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState 
{
    public bool OnPressing;
    public bool OnPress;
    public bool OnRelease;

    private bool currentState;
    private bool lastState;

    public void Tick(bool inputState)
    {
        currentState = inputState;
        OnPressing = currentState;

        OnPress=false;
        OnRelease = false;
        if(currentState != lastState)
        {
            if(currentState)
            {
                OnPress = true;
            }else
            {
                OnRelease = true;
            }
        }
        lastState = currentState;
    }
}
