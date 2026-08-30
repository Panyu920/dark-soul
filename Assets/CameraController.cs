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

        if (null == pi)
        {
            Debug.LogError("pi is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempModelAngel = model.transform.eulerAngles;
        playerHandle.transform.Rotate(Vector3.up , pi.Jright * hSpeed * Time.deltaTime); 

        _currentPitch -= pi.Jup * vSpeed*Time.deltaTime;
        _currentPitch = Mathf.Clamp(_currentPitch,minPitch,maxPitch);

        cameraHandle.transform.localRotation = Quaternion.Euler(_currentPitch, 0, 0);
        model.transform.eulerAngles = tempModelAngel;
    }

    void LateUpdate()
    {
        cameraTransform.eulerAngles = transform.eulerAngles;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, transform.position,cameraSpeed);
    }
}
