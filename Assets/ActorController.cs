using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
[RequireComponent(typeof(Rigidbody))]

[RequireComponent(typeof(PlayerInput))]
public class ActorController : MonoBehaviour
{
    public GameObject model;
    private CapsuleCollider col;
    // Start is called before the first frame update

    [Space(10)]
    [Header("===== friction setting")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    [SerializeField]
    private Animator anim;
    private PlayerInput pi;

    [SerializeField]
    private float movingSpeed = 1.4f;

    private Vector3 planarVec;
    private Vector3 thrustVec;
    [SerializeField]
    private float verticalSpeed = 5.0f;
    [SerializeField]
    private float rollSpeed = 2.0f;
    private bool lockPlanar;

    public Rigidbody rigid;

    public float runMultiplyer = 2.7f;
    private float lerpWeight = 0.0f;


    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), pi.isRunning ? 2.0f : 1.0f, 0.5f));

        if (pi.jump)
        {
            anim.SetTrigger("jump");
        }

        if (pi.roll)
        {
            anim.SetTrigger("roll");
        }

        if (pi.attack && anim.GetBool("onGround"))
        {
            anim.SetTrigger("attack");
        }
        if (pi.Dmag > 0.1f)
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
        }

        if (false == lockPlanar)
        {
            planarVec = pi.Dmag * model.transform.forward * movingSpeed * (pi.isRunning ? runMultiplyer : 1.0f);
        }

        if (rigid.velocity.magnitude > 5.0f)
        {
            anim.SetTrigger("roll");
        }
    }

    void FixedUpdate()
    {
        // rigid.position += planarVec * Time.fixedDeltaTime * movingSpeed; 
        if (true == pi.roll && rigid.velocity == Vector3.zero)
        {
            rigid.velocity = model.transform.forward * 3;
        }else {
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        }
    }

    void OnJumpEnter()
    {
        // print("jump enter");
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, verticalSpeed, 0);
    }


    void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        pi.roll = false;
        col.material = frictionOne;
    }

    void OnGroundExit()
    {
        col.material = frictionZero;
    }
    void OnFallEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
    }

    void OnGround()
    {
        anim.SetBool("onGround", true);
    }

    void NotOnGround()
    {
        anim.SetBool("onGround", false);
    }

    void OnRollEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        if (rigid.velocity == Vector3.zero)
        {
            rigid.velocity = model.transform.forward * 1;
        }
    }

    void OnRollUpdate()
    {
        // if (rigid.velocity == Vector3.zero)
        // {
        // // print(rigid.velocity);
        //     rigid.velocity = model.transform.forward * 10;
        // // print(rigid.velocity);
        // }
        thrustVec = model.transform.forward * anim.GetFloat("rollVelocity");

        // rigid.velocity *= anim.GetFloat("rollVelocity");
        // print(anim.GetFloat("rollVelocity"));
    }

    void OnAttack1hAEnter()
    {
        // pi.inputEnabled = false;
        lerpWeight = 1.0f;
            // anim.ResetTrigger("attack");
    }
    void OnAttack1hAUpdate()
    {
        int layerIndex = anim.GetLayerIndex("Attack");
        float currentWeight = anim.GetLayerWeight(layerIndex);
        currentWeight = Mathf.Lerp(currentWeight,lerpWeight,0.1f);
        anim.SetLayerWeight(layerIndex,currentWeight);
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
    }
    void OnAttack1hAExit()
    {
        // anim.SetLayerWeight(anim.GetLayerIndex("Attack"),1.0f);
        // pi.inputEnabled = false;
            pi.attack = false;
            // print(111);
    }
    
    void OnAttackIdle()
    {
        pi.inputEnabled = true;
        // anim.SetLayerWeight(anim.GetLayerIndex("Attack"),0.0f);
            lerpWeight = 0;
    }

    void OnAttackIdleUpdate()
    {
        int layerIndex = anim.GetLayerIndex("Attack");
        float currentWeight = anim.GetLayerWeight(layerIndex);
        currentWeight = Mathf.Lerp(currentWeight,lerpWeight,0.1f);
        anim.SetLayerWeight(layerIndex,currentWeight);
    }
}
