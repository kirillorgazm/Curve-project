using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth { get; private set; } = 10;
    public int currentHealth { get; private set; }

    private SpriteRenderer[] spriteRenderers;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int _incomingDamage)
    {
        currentHealth -= _incomingDamage;
        StartCoroutine(animateDamageTaken());

    }

    private IEnumerator animateDamageTaken()
    {
        foreach (SpriteRenderer _sr in spriteRenderers)

            if (_sr.CompareTag("Player"))
                _sr.color = Color.red;


        yield return new WaitForSeconds(1f);

        foreach (SpriteRenderer _sr in spriteRenderers)
            if (_sr.CompareTag("Player"))
                _sr.color = Color.white;
    }

}
