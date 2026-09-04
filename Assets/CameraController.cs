using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerInput pi;

    private GameObject cameraHandle;
    private GameObject playerHandle;
    private GameObject model;
    public Image targetImage;

     public float minPitch = -30f;
    public float maxPitch = 30f;
    private Rigidbody rigid;

    private float _currentPitch = 20.0f;
    private Transform cameraTransform;
    [SerializeField]
    private LockTarget lockTarget;

    [SerializeField]
    private float hSpeed;

    [SerializeField]
    private float vSpeed;
    [SerializeField]
    private float cameraSpeed;
    public bool isLock;
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        pi = playerHandle.GetComponent<PlayerInput>();
        model = playerHandle.gameObject.GetComponent<ActorController>().model;
        cameraTransform = Camera.main.transform;
        rigid = playerHandle.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        targetImage.enabled = false;

        if (null == pi)
        {
            Debug.LogError("pi is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lockTarget == null)
        {
        Vector3 tempModelAngel = model.transform.eulerAngles;
        
        // Quaternion targetAngel = playerHandle.transform.rotation; 
        playerHandle.transform.Rotate(Vector3.up , pi.Jright * hSpeed * Time.deltaTime); 
        // playerHandle.g
        //  Quaternion deltaRotation = Quaternion.Euler(0, pi.Jright*hSpeed * Time.fixedDeltaTime, 0);
        // rigid.MoveRotation(rigid.rotation * deltaRotation);

        // 累积角度增量
    // yaw += pi.Jright * hSpeed * Time.deltaTime;

    // // 目标旋转：只绕Y轴
    // Quaternion targetRot = Quaternion.Euler(0, yaw, 0);

    // // 平滑插值
    // playerHandle.transform.rotation = Quaternion.Lerp(
    //     playerHandle.transform.rotation,
    //     targetRot,
    //     0.1f 
    // );

        _currentPitch -= pi.Jup * vSpeed*Time.deltaTime;
        _currentPitch = Mathf.Clamp(_currentPitch,minPitch,maxPitch);

        cameraHandle.transform.localRotation = Quaternion.Euler(_currentPitch, 0, 0);
        // Quaternion.Lerp(cameraHandle.transform.localRotation,Quaternion.Euler(_currentPitch,0,0),0.1f);

        model.transform.eulerAngles = tempModelAngel;

        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
            
            targetImage.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0,lockTarget.halfHeight,0));
            // 超出距离取消锁定
            if(Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 10.0f)
            {
                lockTarget = null;
                targetImage.enabled = false;
                isLock = false;
            }
        }
        // cameraTransform.LookAt(cameraHandle.transform);
    }

    void LateUpdate()
    {
        // cameraTransform.eulerAngles = Vector3.Lerp(cameraTransform.eulerAngles,transform.eulerAngles,cameraSpeed);
        // Quaternion.Lerp(cameraHandle.transform.localRotation,Quaternion.Euler(_currentPitch,0,0),0.1f);
        cameraTransform.LookAt(cameraHandle.transform);
        // yaw = 0;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, transform.position,cameraSpeed);
    }

    public void LockUnlock()
    {
        // print(111);
        Vector3 pos1 = model.transform.position;
        Vector3 pos2 = pos1+ Vector3.up;
        Vector3 boxCenter = pos2 + model.transform.forward *4.0f;

        Collider[] colliders = Physics.OverlapBox(boxCenter, new Vector3(1.5f,1.5f,1.5f),model.transform.rotation,LayerMask.GetMask("Enemy"));

        if(colliders.Length >0)
        {
            foreach (var col in colliders)
            {
                if(lockTarget != null &&col.gameObject == lockTarget.obj)
                {
                    lockTarget = null;
                    // print(222);
                    targetImage.enabled = false;
                    isLock  = false;
                    break;
                } 
                lockTarget = new LockTarget(col.gameObject,col.bounds.extents.y);
                // print(lockTarget.halfHeight);
                    targetImage.enabled = true;
                    isLock = true;
                break;
            }

        }else
        {
            lockTarget = null;
                    targetImage.enabled = false;
                    isLock  = false;
        }
    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;

        public LockTarget(GameObject obj , float halfHeight)
        {
            this.obj = obj;
            this.halfHeight = halfHeight;
        }
    }
}
