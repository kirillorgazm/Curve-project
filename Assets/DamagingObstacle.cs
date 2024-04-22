using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamagingObstacle : MonoBehaviour
{

    [SerializeField] public PlayerHealth playerHealth;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private int damage = 1;

    private float damageCooldownCounter;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (damageCooldownCounter > damageCooldown)
            {
                playerHealth.TakeDamage(damage);
                damageCooldownCounter = 0;
            }
            else
            {
                damageCooldownCounter += Time.deltaTime;
            }
        }
    
    }
}
