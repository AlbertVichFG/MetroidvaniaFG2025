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
    [SerializeField]
    private float manaRecovery;
    /*[SerializeField]
    public bool canDash;*/
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

    //NEWWW--------------------------------------
    [Header("Wall Slide")]
    [SerializeField] private Transform wallCheck;          
    [SerializeField] private float wallCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private float wallStickDelay = 0.5f;  // Temps abans de començar a caure lentament

    [Header("Life Recovery")]
    [SerializeField] 
    private float lifeRecoveryAmount;
    [SerializeField] 
    private float manaCostLifeRecover; 
    [SerializeField] 
    private float recoverCooldown; // Temps mínim entre recuperacions

    private float recoverTimer;


    private bool isTouchingWall;
    private bool isWallSliding;
    private float wallStickTimer;
    private int wallSlideDirection; // -1 esquerra, 1 dreta


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

        float horizontal = Input.GetAxis("Horizontal");

        //Moviment
        if (comboCount == 0)
        {
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

            //DispararFireBall

            if (GameManager.instance.GetGameData.HasFireBall == true)
            {
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
        if (GameManager.instance.GetGameData.CanDash == true)
        {
            if (Input.GetButtonDown("Dash") && dashCoolTimer <= 0 && comboCount == 0 && isTouchingWall == false)
            {
                if (isGrounded)
                {
                    StartDash();

                    usedDashThisAir = false;   //assegura dash infinit al terra
                }
                else if (!usedDashThisAir)
                {
                    StartDash();
                    usedDashThisAir = true;    // nom�s gastem dash a l�aire
                }
            }

            HandleDash();

        }

        //Crouch
        if (GameManager.instance.GetGameData.CanCrouch == true)
        {
            if (Input.GetButtonDown("Crouch"))
            {
                animator.SetBool("IsCrouch", true);
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                animator.SetBool("IsCrouch", false);

            }
        }

        //WallJump
        if (GameManager.instance.GetGameData.CanGrabWall == true)
        {
            CheckWallSlide();
        }


        //Recuperar Vida

        if (GameManager.instance.GetGameData.CanHeal == true)
        {
         
            recoverTimer += Time.deltaTime;


            bool isQuiet = Mathf.Abs(horizontal) < 0.05f && comboCount == 0 && !isDashing; //Aixo ho fa quan esta quiet 100%

            if (Input.GetAxis("Vertical") < -0.8f && recoverTimer >= recoverCooldown && isQuiet)
            {
                RecoverLife();
                recoverTimer = 0;
                Debug.Log("He Recuperat Vida");
            }
        }

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

    //Atacar a enemic 
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // SkullBomb skull = collision.gameObject.GetComponent<SkullBomb>();
        if (collision.gameObject.tag == "Enemy" /*&& skull.isSkull == false*/)
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

            RecoveryMana();
        }
        if (collision.gameObject.tag == "Enemy" /*&& skull.isSkull == true*/)
        {

            int comboAnimator = animator.GetInteger("Comboo");
            if (comboAnimator > 0)
            {
                collision.gameObject.GetComponent<SkullBomb>().TakeDmg(GameManager.instance.GetGameData.PlayerDmg);
            }
            else
            {
                collision.gameObject.GetComponent<SkullBomb>().TakeDmg(GameManager.instance.GetGameData.HeavyDmg);

            }

            RecoveryMana();


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


    private void StartDash()
    {
        animator.SetTrigger("Dash");
        isDashing = true;
        dashTimer = dashDuration;
        dashCoolTimer = dashCooldown;


        comboCount = 0;
        animator.SetInteger("Comboo", 0);

    }

    private void HandleDash()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            float dir = transform.eulerAngles.y == 0 ? 1f : -1f;

            // Mant� Y per gravetat

            //rb.linearVelocity = new Vector2(dir * dashForce, rb.linearVelocity.y);
            rb.AddForce(new Vector2(dir * dashForce, 0f), ForceMode2D.Impulse);


            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }

        // Cooldown
        if (dashCoolTimer > 0f) dashCoolTimer -= Time.deltaTime;
    }


    //Recuperar Mana!

    public void RecoveryMana()
    {
        if (GameManager.instance.GetGameData.PlayerMana <= GameManager.instance.GetGameData.PlayerMaxMana)
        {
            Debug.Log("Suma mana");
            GameManager.instance.GetGameData.PlayerMana += manaRecovery;
            if (GameManager.instance.GetGameData.PlayerMana > GameManager.instance.GetGameData.PlayerMaxMana)
            {
                Debug.Log("IgualaMana");
                GameManager.instance.GetGameData.PlayerMana = GameManager.instance.GetGameData.PlayerMaxMana;
            }
        }
        levelManager.UpdateMana();
    }

    //WallJump
    private void CheckWallSlide()
    {

        Vector2 dir = wallCheck.right;
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, dir, wallCheckDistance, groundLayer);

        //Debug.DrawRay(wallCheck.position, dir * wallCheckDistance, Color.red);

        // Comprovar si toca la paret
        isTouchingWall = hit.collider != null;

        // Enganxar si toca la paret i eta al aire
        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;
            animator.SetBool("IsHowall", true);
            //slow caigud vertc
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            //reset Jumps
            JumpCount = 0;
        }
        else
        {
            isWallSliding = false;
            

        }

        if (isTouchingWall == false)
        {

            animator.SetBool("IsHowall", false);
        }
    }

    private void RecoverLife()
    {
        //Pot fer això per molt que tingui tota la vida igual que al HollowKnight per si em buscaves l'error jeje

        if (GameManager.instance.GetGameData.PlayerMana >= manaCostLifeRecover)
        {
            GameManager.instance.GetGameData.PlayerMana -= manaCostLifeRecover;
            GameManager.instance.GetGameData.PlayerLIFE += lifeRecoveryAmount;

            if (GameManager.instance.GetGameData.PlayerLIFE > GameManager.instance.GetGameData.PlayerMaxLife)
            {
                GameManager.instance.GetGameData.PlayerLIFE = GameManager.instance.GetGameData.PlayerMaxLife;
            }

            levelManager.UpdateMana();
            levelManager.UpdateLife();
        }
    }

}



