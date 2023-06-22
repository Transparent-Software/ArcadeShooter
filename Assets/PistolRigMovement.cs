using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolRigMovement : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(new Vector3(0f, 0f, 0f), Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.Translate(new Vector3(0f, 0f, 0.03f), Space.Self);
        }

        if (Input.GetMouseButtonUp(0))
        {
            transform.Translate(new Vector3(0f, 0f, -0.03f), Space.Self);
        }
    }
}
