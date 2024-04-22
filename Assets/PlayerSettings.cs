using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    [Header("Common settings")]
    [Tooltip("Set this to adjust run speed")]
    [SerializeField] public float runSpeed = 10f;
    [SerializeField] public float jumpForce = 20f;
    [SerializeField] public float coyoteTime = 0.2f;
    [SerializeField] public float dashSpeed = 70f;
    [SerializeField] public float dashTime = 0.2f;
    [SerializeField] public float dashCooldown = 2f;
    [SerializeField] public float wallSlidingSpeed = 2f;
    [SerializeField] public float bashTime = 0.2f;
    [SerializeField] public float bashCooldown = 5f;
}
