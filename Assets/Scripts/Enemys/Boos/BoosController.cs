using System.Collections;
using UnityEngine;

public class BoosController : MonoBehaviour
{
    public enum BossStates { Waiting, Jumping, Roar, Roll, Spikes, Death }

    [Header("GeneralConfig")]
    [SerializeField] private BossStates currentState;
    private Transform playerPos;
    private Animator animator;
    private Rigidbody2D rb;

    [SerializeField] private float bossLife;
    [SerializeField] private float dmg;
    [SerializeField] private float knockBackforce;
    [SerializeField] private Sprite dieSprite;
    [SerializeField] public bool isDeathBoss;

    [SerializeField] private float waitingTime;

    [Header("Jump")]
    [SerializeField] private float maxJump = 13f;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float timeToJump;

    [Header("RoarBettler")]
    [SerializeField] private GameObject bettlerPrefab;
    [SerializeField] private Transform bettlerSpawnPoint;
    [SerializeField] private float timeToSpawn;

    [Header("Roll")]
    [SerializeField] private float timeToRoll;
    [SerializeField] private float colliderSizeX;
    [SerializeField] private float rollSpeed;
    private bool collisioned;
    private ContactPoint2D[] puntosContatcto;

    [Header("Spikes")]
    [SerializeField] private GameObject spikesPrefab;
    [SerializeField] private Transform[] spikesSpawnPoints;
    [SerializeField] private float timeToSpike;
    [SerializeField] private float timeToTired;



    private void FreezeMovement()
    {
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void UnfreezeMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }



    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        FreezeMovement();  

        ChangeState();
    }

    public void SetDeathAtStart()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        GetComponent<SpriteRenderer>().sprite = dieSprite;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        this.enabled = false;
        isDeathBoss = true;
    }

    // -----------------------------
    // STATES
    // -----------------------------

    void ChangeState()
    {
        switch (currentState)
        {
            case BossStates.Waiting: StartCoroutine(WaitingCoroutine()); break;
            case BossStates.Jumping: StartCoroutine(JumpCoroutine()); break;
            case BossStates.Roar: StartCoroutine(RoarCoroutine()); break;
            case BossStates.Roll: StartCoroutine(RollCoroutine()); break;
            case BossStates.Spikes: StartCoroutine(SpikesCoroutine()); break;
            case BossStates.Death: break;
        }
    }

    // -----------------------------
    // WAITING
    // -----------------------------

    IEnumerator WaitingCoroutine()
    {
        FreezeMovement();   //  NO pot moure’s

        LookAtPlayer();
        yield return new WaitForSeconds(waitingTime);
        LookAtPlayer();

        currentState = (BossStates)Random.Range(1, 5);
        ChangeState();
    }

    private void LookAtPlayer()
    {
        if (transform.position.x < playerPos.position.x)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = Vector3.zero;
    }

    // -----------------------------
    // JUMP
    // -----------------------------

    IEnumerator JumpCoroutine()
    {
        UnfreezeMovement();   // Es pot moure

        animator.SetBool("IsJumping", true);
        yield return new WaitForSeconds(timeToJump);

        Vector2 posA = transform.position;
        float posBX = playerPos.position.x;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * jumpSpeed;
            float posX = Mathf.Lerp(posA.x, posBX, t);
            float posY = posA.y + (4 * maxJump * t * (1 - t));
            transform.position = new Vector2(posX, posY);
            yield return null;
        }

        animator.SetBool("IsJumping", false);

        FreezeMovement();  //  Quan aterra, no es mou més
        currentState = BossStates.Waiting;
        ChangeState();
    }

    // -----------------------------
    // ROAR
    // -----------------------------

    IEnumerator RoarCoroutine()
    {
        FreezeMovement();  //  Quiet durant el roar

        animator.SetBool("IsRoaring", true);
        yield return new WaitForSeconds(timeToSpawn);

        Instantiate(bettlerPrefab, bettlerSpawnPoint.position, bettlerSpawnPoint.rotation);

        animator.SetBool("IsRoaring", false);
        yield return new WaitForSeconds(timeToSpawn);

        currentState = BossStates.Waiting;
        ChangeState();
    }

    // -----------------------------
    // ROLL
    // -----------------------------

    IEnumerator RollCoroutine()
    { 
        UnfreezeMovement();   //  Es pot moure mentre roda

        animator.SetBool("IsRolling", true);
        collisioned = false;

        yield return new WaitForSeconds(timeToRoll);

        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        float standerColliderX = collider.size.x;
        collider.size = new Vector2(colliderSizeX, collider.size.y);

        while (!collisioned)
        {
            transform.Translate(Vector3.left * rollSpeed * Time.deltaTime, Space.Self);
            yield return null;
        }

        animator.SetBool("IsRolling", false);
        collider.size = new Vector2(standerColliderX, collider.size.y);

        FreezeMovement();   //  Atura’t quan xoca
        yield return new WaitForSeconds(timeToRoll);

        currentState = BossStates.Waiting;
        ChangeState();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        puntosContatcto = collision.contacts;

        if (collision.GetContact(puntosContatcto.Length - 1).normal.y > -0.6f &&
            collision.GetContact(puntosContatcto.Length - 1).normal.y < 0.6f)
        {
            if (Mathf.Abs(collision.GetContact(puntosContatcto.Length - 1).normal.x) > 0.5f)
                collisioned = true;
        }
    }

    // -----------------------------
    // SPIKES
    // -----------------------------

    IEnumerator SpikesCoroutine()
    {
        FreezeMovement();  //  Quiet durant spikes

        animator.SetBool("IsSpiking", true);

        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        float standerColliderX = collider.size.x;
        collider.size = new Vector2(colliderSizeX, collider.size.y);

        yield return new WaitForSeconds(timeToSpike);
        yield return new WaitForSeconds(timeToTired);

        animator.SetBool("IsSpiking", false);
        collider.size = new Vector2(standerColliderX, collider.size.y);

        currentState = BossStates.Waiting;
        ChangeState();
    }

    public void ShootSpikes()
    {
        for (int i = 0; i < spikesSpawnPoints.Length; i++)
            Instantiate(spikesPrefab, spikesSpawnPoints[i].position, spikesSpawnPoints[i].rotation);
    }

    // -----------------------------
    // DAMAGE
    // -----------------------------

    public void TakeDmg(float _dmg)
    {
        bossLife -= _dmg;

        if (bossLife <= 0)
        {
            animator.SetTrigger("IsDeath");
            StopAllCoroutines();
            FreezeMovement();

            GetComponent<CapsuleCollider2D>().enabled = false;
            rb.gravityScale = 0;

            this.enabled = false;
            GameManager.instance.GetGameData.Boss1 = true;
            isDeathBoss = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDmg(dmg);

            if (transform.position.x < playerPos.position.x)
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.right * knockBackforce);
            else
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.left * knockBackforce);

            StartCoroutine(collision.gameObject.GetComponent<PlayerController>().KnockBackCoroutine());
        }
    }
}
