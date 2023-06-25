using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInHandTracking : MonoBehaviour
{
    public Transform tracking;
    public Vector3 trackingDisplacement = new Vector3(-0.02f, 0.03f, 0.29f);
    public Vector3 rotationDisplacement = new Vector3(330.9f, 475.2f, 71.2f);

    // Start is called before the first frame update
    void Start()
    {
        transform.position = tracking.position;
        transform.rotation = tracking.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = tracking.position;
        transform.Translate(trackingDisplacement);

        transform.rotation = tracking.rotation;
        transform.Rotate(rotationDisplacement, Space.Self);

    }
}
