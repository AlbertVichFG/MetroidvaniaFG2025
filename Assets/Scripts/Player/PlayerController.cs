using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float groundDistance;
    [SerializeField]
    private bool isGrounded;
    private int JumpCount;
    private int comboCount;


    [Header("FireBall")]
    [SerializeField]
    private GameObject fireballPrefab;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float coldDown;
    private float timePasFireBall;
    [SerializeField]
    private float costFireBall;

    [Header("Knockback")]
    public bool knockBack;
    [SerializeField]
    private float knockBackTime;

    [Header("Dash")]
    [SerializeField]
    private float dashForce = 20f;
    [SerializeField]
    private float dashDuration = 0.12f;
    [SerializeField]
    private float dashCooldown = 0.8f;

    private bool isDashing;
    private float dashTimer;
    private float dashCoolTimer;
    private bool usedDashThisAir;



    private Animator animator;
    private Rigidbody2D rb;
    private LevelManager levelManager;


    //Temporal





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        //Buscar lvlmanager en l'escena per nom
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (knockBack == true) return;


        //Moviment
        if (comboCount == 0)
        {
            float horizontal = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(speed * horizontal, rb.linearVelocity.y);

            if (horizontal == 0f)
            {
                animator.SetBool("IsRunning", false);
            }
            else
            {
                animator.SetBool("IsRunning", true);
            }

            if (horizontal < 0f)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);

            }


            //Salt
            else if (horizontal > 0f)
            {
                transform.eulerAngles = Vector3.zero;
            }

            if (Input.GetButtonDown("Jump") && JumpCount < GameManager.instance.GetGameData.MaxJumps)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                JumpCount++;
            }
            CheckJump();

            if (Input.GetButtonDown("FireBall"))
            {
                if (coldDown <= timePasFireBall && GameManager.instance.GetGameData.PlayerMana >= costFireBall)
                {
                    Instantiate(fireballPrefab, spawnPoint.position, spawnPoint.rotation);
                    GameManager.instance.GetGameData.PlayerMana -= costFireBall;
                    levelManager.UpdateMana();
                    timePasFireBall = 0;
                }

            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }


        //Atack

        if (JumpCount == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                comboCount = Mathf.Clamp(comboCount + 1, 0, 2);
                animator.SetInteger("Comboo", comboCount);

            }
            if (Input.GetButtonDown("Fire2") && comboCount == 0)
            {
                animator.SetTrigger("AttackHeavy");
                comboCount = 1;
            }
        }

        timePasFireBall += Time.deltaTime;


        //Dash

        if (Input.GetButtonDown("Dash") && dashCoolTimer <= 0)
        {
            if (isGrounded)
            {
                StartDash();
                usedDashThisAir = false;   //assegura dash infinit al terra
            }
            else if (!usedDashThisAir)
            {
                StartDash();
                usedDashThisAir = true;    // només gastem dash a l’aire
            }
        }

        HandleDash();

    }

    //Atacs personatge + animacio
    public void CheckCombo1()
    {
        if (comboCount < 2)
        {
            comboCount = 0;
            animator.SetInteger("Comboo", comboCount);
        }
    }
    public void CheckCombo2()
    {
        comboCount = 0;
        animator.SetInteger("Comboo", comboCount);
    }

    public void FinishHeavyAttack()
    {
        comboCount = 0;
        animator.SetInteger("Comboo", comboCount);
    }

    void CheckJump()
    {

        //TocarTerra
        Collider2D[] coliders = Physics2D.OverlapCircleAll(transform.position, groundDistance);

        isGrounded = false;

        for (int i = 0; i < coliders.Length; i++)
        {
            if (coliders[i].transform.tag == "Ground")
            {
                isGrounded = true;

            }

        }
        if (isGrounded == true)
        {
            JumpCount = 0;
            animator.SetBool("IsJumping", false);

            usedDashThisAir = false;
        }
        else
        {
            animator.SetBool("IsJumping", true);


            if (JumpCount == 0)
            {
                JumpCount++;

            }
        }
    }

    //Atacar enemic
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int comboAnimator = animator.GetInteger("Comboo");
            if (comboAnimator > 0)
            {
                try
                {
                    collision.gameObject.GetComponent<EnemyController>().TakeDmg(GameManager.instance.GetGameData.PlayerDmg);
                }
                catch
                {
                    collision.gameObject.GetComponent<BoosController>().TakeDmg(GameManager.instance.GetGameData.PlayerDmg);
                }
            }
            else
            {
                try
                {
                    collision.gameObject.GetComponent<EnemyController>().TakeDmg(GameManager.instance.GetGameData.HeavyDmg);
                }
                catch
                {
                    collision.gameObject.GetComponent<BoosController>().TakeDmg(GameManager.instance.GetGameData.HeavyDmg);
                }
            }
        }
    }

    public void TakeDmg(float _dmg)
    {
        GameManager.instance.GetGameData.PlayerLIFE -= _dmg;
        levelManager.UpdateLife();
        if (GameManager.instance.GetGameData.PlayerLIFE <= 0)
        {
            //Muerte
            animator.SetTrigger("Death");
            //Treure panel GameOver
            //Tornar punt guardat
        }
        else
        {
            //gethit
            animator.SetTrigger("Hit");
            comboCount = 0;

        }
    }

    public IEnumerator KnockBackCoroutine()
    {
        knockBack = true;
        yield return new WaitForSeconds(knockBackTime);
        knockBack = false;
    }

    //Dashhh

    private bool IsGrounded()
    {
        return isGrounded;
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCoolTimer = dashCooldown;

        //  animator.SetTrigger("Dash");

        comboCount = 0;
        animator.SetInteger("Comboo", 0);

    }

    private void HandleDash()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            float dir = transform.eulerAngles.y == 0 ? 1f : -1f;

            // Manté Y per gravetat
            rb.linearVelocity = new Vector2(dir * dashForce, rb.linearVelocity.y);

            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }

        // Cooldown
        if (dashCoolTimer > 0f) dashCoolTimer -= Time.deltaTime;
    }


}


