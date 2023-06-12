using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    public GameObject player;
    public GameObject cameraCenter;
    public Camera cam;

    public float scrollSensitivity = 5f;
    public float scrollDampening = 6f;
    public float sensitivity = 5f;
    public float collisionSensitivity;

    private RaycastHit _camHit;
    private Vector3 _camDist;
    // Start is called before the first frame update
    void Start()
    {
        _camDist = cam.transform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector3 targetPosition = target.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            var rotation = Quaternion.Euler(0f, cameraCenter.transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * sensitivity, cameraCenter.transform.rotation.eulerAngles.z);

            cameraCenter.transform.rotation = rotation;
        }
    }
}
