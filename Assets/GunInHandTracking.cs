using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInHandTracking : MonoBehaviour
{
    public Transform tracking;
    public Vector3 trackingDisplacement = new Vector3(0f, 0.03f, 0.3f);

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
        transform.Rotate(new Vector3(-75f, 0f, 90f));

    }
}
