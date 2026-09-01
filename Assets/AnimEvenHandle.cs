using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvenHandle : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>(); 
    }

    public void ResetTrigger(string name)
    {
        anim.ResetTrigger(name);
        print(name);
    }
}
