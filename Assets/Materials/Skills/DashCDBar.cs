using UnityEngine;
using UnityEngine.UI;

public class DashCDBar : MonoBehaviour
{
    [SerializeField] private PlayerCooldownStats playerCooldownStats;
    [SerializeField] private Image totalDashCD;
    [SerializeField] private Image currentDashCD;

    // Start is called before the first frame update
    void Start()
    {
        totalDashCD.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        currentDashCD.fillAmount = (float)playerCooldownStats.currentDashCD / playerCooldownStats.totalDashCD;
    }
}
