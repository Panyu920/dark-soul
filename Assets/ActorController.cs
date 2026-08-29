using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private Vector3 movingVec;

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
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), pi.isRunning?2.0f:1.0f,0.5f));

        if (pi.jump)
        {
            anim.SetTrigger("jump");
        }
        if (pi.Dmag > 0.1f)
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward,pi.Dvec,0.3f);
        }
        movingVec = pi.Dmag * model.transform.forward * movingSpeed * (pi.isRunning?runMultiplyer:1.0f);
    }

    void FixedUpdate()
    {
        // rigid.position += movingVec * Time.fixedDeltaTime * movingSpeed; 
        rigid.velocity = new Vector3(movingVec.x, rigid.velocity.y, movingVec.z);
    }
}
