using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healhbar : MonoBehaviour
{

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image totalHPBar;
    [SerializeField] private Image currentHPBar;

    // Start is called before the first frame update
    void Start()
    {
        totalHPBar.fillAmount = (float)playerHealth.maxHealth / playerHealth.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHPBar.fillAmount = (float)playerHealth.currentHealth / playerHealth.maxHealth;
    }
}
