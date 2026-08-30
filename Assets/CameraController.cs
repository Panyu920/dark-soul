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
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        pi = playerHandle.GetComponent<PlayerInput>();
        model = playerHandle.gameObject.GetComponent<ActorController>().model;
        cameraTransform = Camera.main.transform;
        rigid = playerHandle.GetComponent<Rigidbody>();

        if (null == pi)
        {
            Debug.LogError("pi is null");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 tempModelAngel = model.transform.eulerAngles;
        playerHandle.transform.Rotate(Vector3.up , pi.Jright * hSpeed * Time.fixedDeltaTime); 
        // playerHandle.g
        //  Quaternion deltaRotation = Quaternion.Euler(0, pi.Jright*hSpeed * Time.fixedDeltaTime, 0);
        // rigid.MoveRotation(rigid.rotation * deltaRotation);

        _currentPitch -= pi.Jup * vSpeed*Time.fixedDeltaTime;
        _currentPitch = Mathf.Clamp(_currentPitch,minPitch,maxPitch);

        cameraHandle.transform.localRotation = Quaternion.Euler(_currentPitch, 0, 0);
        model.transform.eulerAngles = tempModelAngel;
    }

    void LateUpdate()
    {
        cameraTransform.eulerAngles = Vector3.Lerp(cameraTransform.eulerAngles,transform.eulerAngles,cameraSpeed);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, transform.position,cameraSpeed);
    }
}
