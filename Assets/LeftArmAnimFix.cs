using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform leftLowerArm;
    Animator anim ;
    public Vector3 offset;
    void Start()
    {
        anim =  GetComponent<Animator>();
    leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm); 
    }

    // Update is called once per frame
    void OnAnimatorIK()
    {
        leftLowerArm.localEulerAngles += offset; 
        anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm,Quaternion.Euler(leftLowerArm.localEulerAngles));
    }
}
