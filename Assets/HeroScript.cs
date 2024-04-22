
using System.Collections;
using UnityEngine;

public class HeroScript : MonoBehaviour
{

    [Header("Common settings")]
    [SerializeField] private PlayerSettings _stats;
    [SerializeField] private Vector2 mousePos;
    [SerializeField] private Vector3 mousePosWorld;

    [Header("Jump&Air settings")]
    [SerializeField] private GameObject groundRayObj;
    [SerializeField] private float groundDistance;

    [Header("Dash settings")]
    [SerializeField] private GameObject dashEffect;

    [Header("Wall settings")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    [Header("Damaging settings")]
    [SerializeField] private Transform damagingObstacleCheck;
    [SerializeField] private LayerMask damagingObstacleLayer;

    [Header("Bash settings")]
    [SerializeField] private GameObject bashArrow;

    private Rigidbody2D heroRB;
    private PlayerStateList pState;
    private Animator anim;
    private PlayerCooldownStats playerCooldownStats;

    private bool canDashByCooldown = true;
    private bool doubleJump = true;
    private bool dashed;
    private float heroGravity;  //to reserve gravity for further use
    private float xAxis;
    private float coyoteTimeCounter;

    private Camera mainCam;

    private float bashTimeReset;

    private bool isChoosingBashDirection;
    private bool isBashing;
    [SerializeField] private Vector3 bashDirection;

    void Awake()
    {
        heroRB = GetComponent<Rigidbody2D>();
        pState = GetComponent<PlayerStateList>();
        _stats = GetComponent<PlayerSettings>();
        // anim = GetComponentInChildren<Animator>();
        playerCooldownStats = GetComponent<PlayerCooldownStats>();
        playerCooldownStats.currentDashCD = 0f;
        playerCooldownStats.totalDashCD = _stats.dashCooldown;

        playerCooldownStats.currentBashCD = 0f;
        playerCooldownStats.totalBashCD = _stats.bashCooldown;
    }

    // Start is called before the first frame update
    void Start()
    {
        heroGravity = heroRB.gravityScale;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        bashTimeReset = _stats.bashTime;
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        mousePos = Input.mousePosition;
    }

    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDashByCooldown && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }

        if (IsGrounded())
        {
            //canDash = true;
            dashed = false;
        }

        if (playerCooldownStats.currentDashCD < playerCooldownStats.totalDashCD)
        {
            playerCooldownStats.currentDashCD += Time.deltaTime;
        }
    }

    IEnumerator Dash()
    {
        canDashByCooldown = false;
        playerCooldownStats.currentDashCD = 0f;
        pState.dashing = true;
        heroRB.gravityScale = 0;
        heroRB.velocity = new Vector2(transform.localScale.x * _stats.dashSpeed, 0);

        dashEffect.GetComponent<SpriteRenderer>().color = IsGrounded() ? Color.black : Color.white;
        Instantiate(dashEffect, transform);

        yield return new WaitForSeconds(_stats.dashTime);
        heroRB.gravityScale = heroGravity;
        pState.dashing = false;

        yield return new WaitForSeconds(_stats.dashCooldown);
        canDashByCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        UpdateJumpVariables();
        Debug.Log("dash cd: " + playerCooldownStats.currentDashCD);
        // SurfaceAlignment();
        if (pState.dashing) return;
        Jump();
        Run();
        Flip();
        StartDash();
        WallSlide();
        Bash();

        if (IsDead()) { gameObject.SetActive(false); }
    }

    void FixedUpdate()
    {
        // SurfaceAlignment();
    }

    private void Bash()
    {
        if (playerCooldownStats.currentBashCD < playerCooldownStats.totalBashCD)
        {
            playerCooldownStats.currentBashCD += Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Time.timeScale = 0.1f;
                bashArrow.SetActive(true);
                isChoosingBashDirection = true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1) && isChoosingBashDirection)
            {
                Time.timeScale = 1;
                isChoosingBashDirection = false;
                isBashing = true;

                bashDirection = mainCam.ScreenToWorldPoint(mousePos) - transform.position;
                bashDirection.z = 0;
                bashDirection = bashDirection.normalized;

                // heroRB.AddForce(-bashDirection * 50, ForceMode2D.Impulse);

                bashArrow.SetActive(false);
                playerCooldownStats.currentBashCD = 0f;
            }
        }

        if (isBashing)
        {
            if (_stats.bashTime > 0)
            {
                _stats.bashTime -= Time.deltaTime;
                heroRB.velocity = bashDirection * 15000 * Time.deltaTime;
                Debug.Log("hero velo " + heroRB.velocity);
            }
            else
            {
                isBashing = false;
                _stats.bashTime = bashTimeReset;
                heroRB.velocity = new Vector2(heroRB.velocity.x, 0);
                Debug.Log("hero velo " + heroRB.velocity);
            }
        }



    }

    private void Run()
    {
        // Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        // transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        heroRB.velocity = new Vector2(xAxis * _stats.runSpeed, heroRB.velocity.y);
        // anim.SetBool("Walk", true);

    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteTimeCounter > 0f || doubleJump) // doubleJump should be false by default, representing a state of double jump
            {
                doubleJump = !doubleJump;
                heroRB.velocity = new Vector3(heroRB.velocity.x, _stats.jumpForce);
                pState.jumping = true;
                coyoteTimeCounter = 0;  //set to zero as we are jumping now
            }

        }

    }

    void Flip()
    {
        // transform.localScale = new Vector2(xAxis, transform.localScale.y);
        //rotate full object via transform, instead of sprite.flip
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }

    private void UpdateJumpVariables()
    {
        if ((IsGrounded() || IsWalled()) && !Input.GetButton("Jump"))
        {
            doubleJump = false;
            pState.jumping = false;
        }

        //restart coyote time if on the ground or on a wall - to allow jump from a wall
        if (IsGrounded() || IsWalled())
        {
            coyoteTimeCounter = _stats.coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    public bool IsGrounded()
    {
        //check if ground is near ~0.3f. Use groundRayObj instead of main obj, bcz closer to surface
        Collider2D[] collider = Physics2D.OverlapCircleAll(groundRayObj.transform.position, 0.3f);

        return collider.Length > 1 ? true : false;

    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private bool IsDead()
    {
        return GetComponent<PlayerHealth>().currentHealth <= 0 ? true : false;
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && xAxis != 0f)
        {
            pState.wallSliding = true;
            heroRB.velocity = new Vector2(heroRB.velocity.x, Mathf.Clamp(heroRB.velocity.y, -_stats.wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            pState.wallSliding = false;
        }
    }

    // void OnDrawGizmos()
    // {
    //      Gizmos.color = Color.red;

    //      Gizmos.DrawSphere(damagingObstacleCheck.position, 0.65f);
    // }
}
