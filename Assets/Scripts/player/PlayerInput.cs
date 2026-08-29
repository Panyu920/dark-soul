using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("===== Key Setting =====")]
    public KeyCode KeyUp = KeyCode.W;
    public KeyCode KeyDown = KeyCode.S;
    public KeyCode KeyLeft = KeyCode.A;
    public KeyCode KeyRight = KeyCode.D;
    public KeyCode KeyRun = KeyCode.LeftShift;
    public KeyCode KeyJump = KeyCode.Space;


    [Header("===== Out signals =====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    // pressing signal
    public bool isRunning = false;
    // trigger once signal
    public bool jump = false;
    private bool lastJump = false;
    // double trigger


    [Header("===== Others =====")]
    public bool inputEnabled = true;
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;



    void Start()

    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!inputEnabled)
        {
            targetDright = 0;
            targetDup = 0;
            return;
        }
        Dup = Input.GetKey(KeyUp) ? 1.0f : 0.0f - (Input.GetKey(KeyDown) ? 1.0f : 0.0f);
        Dright = Input.GetKey(KeyRight) ? 1.0f : 0.0f - (Input.GetKey(KeyLeft) ? 1.0f : 0.0f);


        isRunning = Input.GetKey(KeyRun);

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);
        SquareToCircle(ref Dright,ref Dup);

        Dmag = Mathf.Sqrt(Dup*Dup + Dright * Dright);
        Dvec = Dup * transform.forward + Dright * transform.right;

        Jump();
    }

    private void SquareToCircle(ref float x, ref float y)
    {
        float u = x* Mathf.Sqrt(1- y  *  y /2);
        float v = y* Mathf.Sqrt(1- x  *  x /2);
        x = u;
        y = v;
    }

    private void Jump()
    {
        bool newJump = Input.GetKey(KeyJump); 

        if(newJump != lastJump && newJump)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }

        lastJump = jump;
    }
}
