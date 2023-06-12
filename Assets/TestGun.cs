using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform bones;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(fire());                 
        }
        

    }

    IEnumerator fire()
    {

        bones.position.Set(-100f, -100f, -100f);

        yield return new WaitForSeconds(1f);

        bones.position.Set(100f, 1000f, 100f);
    }
}
