using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform target;

    public float movementSpeed = 5f;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * Time.deltaTime * movementSpeed);
        }
    }
}
