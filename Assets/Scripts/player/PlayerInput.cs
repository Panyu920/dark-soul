using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public KeyCode KeyRoll = KeyCode.LeftControl;
    public KeyCode KeyJUp = KeyCode.UpArrow;
    public KeyCode KeyJDown = KeyCode.DownArrow;
    public KeyCode KeyJLeft = KeyCode.LeftArrow;
    public KeyCode KeyJRight = KeyCode.RightArrow;
    public KeyCode KeyDefense = KeyCode.F;
    [Header("===== Key Setting =====")]
    public KeyCode KeyAttack = KeyCode.Mouse0;
    [Header("===== Key Setting =====")]
    public bool mouseEnable = true;
    public float mouseSensitivityY;
    public float mouseSensitivityX;


    [Header("===== Out signals =====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public float Jup;
    public float Jright;
    public Vector3 Dvec;
    // 跑步键抬起时的速度
    private float stopRunDmag;
    // pressing signal
    public bool isRunning = false;
    public bool defense = false;
    // trigger once signal
    public bool jump = false;
    private bool lastJump = false;
    public bool roll = false;
    public bool attack = false;

    // double trigger


    [Header("===== Others =====")]
    public bool inputEnabled = true;
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    public InputState runState =new();
    private InputState jumpState =new();
    private InputState rollState =new();
    private InputState defenseState =new();
    private InputState attackState =new();
    private InputState upState =new();





    // Update is called once per frame
    void Update()
    {
        runState.Tick(Input.GetKey(KeyRun),Time.deltaTime);
        jumpState.Tick(Input.GetKey(KeyJump),Time.deltaTime);
        rollState.Tick(Input.GetKey(KeyRoll),Time.deltaTime);
        defenseState.Tick(Input.GetKey(KeyDefense),Time.deltaTime);
        attackState.Tick(Input.GetKey(KeyAttack),Time.deltaTime);
        
        upState.Tick(Input.GetKey(KeyUp),Time.deltaTime);

        if (Input.GetKeyUp(KeyRun))
        {
            stopRunDmag = Dmag;
            print(Dmag);
        }

        targetDup = Input.GetKey(KeyUp) ? 1.0f : 0.0f - (Input.GetKey(KeyDown) ? 1.0f : 0.0f);
        targetDright = Input.GetKey(KeyRight) ? 1.0f : 0.0f - (Input.GetKey(KeyLeft) ? 1.0f : 0.0f);
        if (!inputEnabled)
        {
            targetDright = 0;
            targetDup = 0;
            // return;
        }
        if (mouseEnable)
        {
            Jup = Input.GetAxis("Mouse Y") * mouseSensitivityY;
            Jright = Input.GetAxis("Mouse X") * mouseSensitivityX;
        }else
        {
        Jup = Input.GetKey(KeyJUp) ? 1.0f : 0.0f - (Input.GetKey(KeyJDown) ? 1.0f : 0.0f);
        Jright = Input.GetKey(KeyJRight) ? 1.0f : 0.0f - (Input.GetKey(KeyJLeft) ? 1.0f : 0.0f);
        }


        // isRunning = Input.GetKey(KeyRun);
        // roll = Input.GetKey(KeyRoll);
        // defense = Input.GetKey(KeyDefense);
        // if(defense && Input.GetKeyUp(KeyDefense))
        // {
        //     defense
        // } 

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);
        SquareToCircle(ref Dright, ref Dup);

        Dmag = Mathf.Sqrt(Dup * Dup + Dright * Dright);
        Dvec = Dup * transform.forward + Dright * transform.right;


        isRunning = (runState.OnPressing && !runState.OnDelaying) || runState.OnExtending;
        roll = rollState.OnPress || upState.OnDoubleClick;
        jump = jumpState.OnPress;
        defense = defenseState.OnPressing;
        attack = attackState.OnPress;

        if(runState.OnExtending)
        {
            // print("A:"+Dmag);
            Dmag = Mathf.Lerp(stopRunDmag,Dmag,-0.1f);
            // print("B:"+Dmag);
            stopRunDmag = Dmag;
        }

    }

    private void SquareToCircle(ref float x, ref float y)
    {
        float u = x * Mathf.Sqrt(1 - y * y / 2);
        float v = y * Mathf.Sqrt(1 - x * x / 2);
        x = u;
        y = v;
    }

    private void Jump()
    {
        bool newJump = Input.GetKey(KeyJump);

        if (newJump != lastJump && newJump)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }

        lastJump = jump;
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Right))
        {
            attack = true;
        }
    }
}
