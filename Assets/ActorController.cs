using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
[RequireComponent(typeof(Rigidbody))]

[RequireComponent(typeof(PlayerInput))]
public class ActorController : MonoBehaviour
{
    public GameObject model;
    // Start is called before the first frame update

    [SerializeField]
    private Animator anim;
    private PlayerInput pi;

    [SerializeField]
    private float movingSpeed = 1.4f;

    private Vector3 planarVec;
    private Vector3 verticalVec;
    [SerializeField]
    private float verticalSpeed = 5.0f;
    private bool lockPlanar;

    public Rigidbody rigid;

    public float runMultiplyer = 2.7f;


    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), pi.isRunning ? 2.0f : 1.0f, 0.5f));

        if (pi.jump)
        {
            anim.SetTrigger("jump");
        }
        if (pi.Dmag > 0.1f)
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
        }

        if (false == lockPlanar)
        {
            planarVec = pi.Dmag * model.transform.forward * movingSpeed * (pi.isRunning ? runMultiplyer : 1.0f) ;
        }
    }

    void FixedUpdate()
    {
        // rigid.position += planarVec * Time.fixedDeltaTime * movingSpeed; 
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + verticalVec;
        verticalVec = Vector3.zero;
    }

    void OnJumpEnter()
    {
        // print("jump enter");
        pi.inputEnabled = false;
        lockPlanar = true;
        verticalVec = new Vector3(0,verticalSpeed,0);
    }


    void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
    }

    void OnFallEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
    }

    void OnGround()
    {
        anim.SetBool("onGround",true);
    }

    void NotOnGround()
    {
        anim.SetBool("onGround",false);
    }
}
