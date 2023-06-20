using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBodyScript : MonoBehaviour
{

    public Transform tracking;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = tracking.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = tracking.position;
        transform.Translate(new Vector3(0f, 0f, 0.03f));
        transform.rotation.Set(tracking.rotation.x + 90, tracking.rotation.z, tracking.rotation.y, 0f);
    }
}
