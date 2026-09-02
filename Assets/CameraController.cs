using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerInput pi;

    private GameObject cameraHandle;
    private GameObject playerHandle;
    private GameObject model;

     public float minPitch = -30f;
    public float maxPitch = 30f;
    private Rigidbody rigid;

    private float _currentPitch = 20.0f;
    private Transform cameraTransform;

    [SerializeField]
    private float hSpeed;

    [SerializeField]
    private float vSpeed;
    [SerializeField]
    private float cameraSpeed;
    private float yaw;
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        pi = playerHandle.GetComponent<PlayerInput>();
        model = playerHandle.gameObject.GetComponent<ActorController>().model;
        cameraTransform = Camera.main.transform;
        rigid = playerHandle.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        if (null == pi)
        {
            Debug.LogError("pi is null");
        }
    }

    // Update is called once per frame
    void Update()
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
        // cameraTransform.LookAt(cameraHandle.transform);
    }

    void LateUpdate()
    {
        cameraTransform.eulerAngles = Vector3.Lerp(cameraTransform.eulerAngles,transform.eulerAngles,cameraSpeed);
        // Quaternion.Lerp(cameraHandle.transform.localRotation,Quaternion.Euler(_currentPitch,0,0),0.1f);
        // cameraTransform.LookAt(cameraHandle.transform);
        // yaw = 0;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, transform.position,cameraSpeed);
    }
}
