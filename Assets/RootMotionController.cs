using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>(); 
    }

    void OnAnimatorMove()
    {
        SendMessageUpwards("OnAnimatorRM",anim.deltaPosition); 
    }
}
