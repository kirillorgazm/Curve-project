using UnityEngine;
using UnityEngine.UI;

public class BashCDBar : MonoBehaviour
{
    [SerializeField] private PlayerCooldownStats playerCooldownStats;
    [SerializeField] private Image totalBashCD;
    [SerializeField] private Image currentBashCD;

    // Start is called before the first frame update
    void Start()
    {
        totalBashCD.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        currentBashCD.fillAmount = (float)playerCooldownStats.currentBashCD / playerCooldownStats.totalBashCD;
    }
}
