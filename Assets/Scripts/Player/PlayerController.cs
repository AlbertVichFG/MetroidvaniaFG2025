using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
    private Animator animator;
    private int JumpCount;
    private int comboCount;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float groundDistance;
    

    [Header ("FireBall")]
    [SerializeField]
    private GameObject fireballPrefab;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float coldDown;
    private float timePasFireBall;

    [SerializeField]
    private float costFireBall;
    

    private LevelManager levelManager;


    //Temporal
    private int maxJumps = 1;


    

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

            if (Input.GetButtonDown("Jump") && JumpCount < maxJumps)
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
        Collider2D[] coliders = Physics2D.OverlapCircleAll(transform.position,  groundDistance);
        bool isGrounded = false;

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
                collision.gameObject.GetComponent<EnemyController>().TakeDmg(GameManager.instance.GetGameData.PlayerDmg);
            }
            else
            {
                collision.gameObject.GetComponent<EnemyController>().TakeDmg(GameManager.instance.GetGameData.HeavyDmg);
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
        }
    }
}
