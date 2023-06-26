using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float maxHealth = 100f;
    public float remainingHealth;
    // Start is called before the first frame update
    void Start()
    {
        remainingHealth = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(float damage)
    {
        Debug.Log("TAKING DAMAGE");
        remainingHealth -= damage;
        if (remainingHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
