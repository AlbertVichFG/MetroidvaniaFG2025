using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BoosController : MonoBehaviour
{
    public enum BossStates { Waiting, Jumping, Roar, Roll, Spikes, Death}

    [Header("GeneralConfig")]
    [SerializeField]
    private BossStates currentState;
    private Transform playerPos;
    private Animator animator;
    [SerializeField]
    private float bossLife;
    [SerializeField]
    private float dmg;
    [SerializeField]
    private float knockBackforce;
    [SerializeField]
    private Sprite dieSprite;
    [SerializeField] 
    public bool isDeathBoss;

    [SerializeField]
    private float waitingTime;

    [Header("Jump")]
    [SerializeField]
    private float maxJump = 13f;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float timeToJump;


    [Header("RoarBettler")]
    [SerializeField]
    private GameObject bettlerPrefab;
    [SerializeField]
    private Transform bettlerSpawnPoint;
    [SerializeField]
    private float timeToSpawn;

    [Header("Roll")]
    [SerializeField]
    private float timeToRoll;
    [SerializeField]
    private float colliderSizeX; //5.33
    [SerializeField]
    private float rollSpeed;
    private bool collisioned;
    private ContactPoint2D[] puntosContatcto;

    [Header("Spikes")]
    [SerializeField]
    private GameObject spikesPrefab;
    [SerializeField]
    private Transform[] spikesSpawnPoints;
    [SerializeField]
    private float timeToSpike;
    [SerializeField]
    private float timeToTired;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();


        ChangeState();
        
    }

    public void SetDeathAtStart()
    {
        //Cridem l'animator un altre cop xk el boss ja esta mort i no s'activa el start
        animator= GetComponent<Animator>();
        animator.enabled = false;
        GetComponent<SpriteRenderer>().sprite = dieSprite;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        this.enabled = false;
        isDeathBoss = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void ChangeState()
    {
        switch (currentState)
        {

            case BossStates.Waiting:
                StartCoroutine(WaitingCoroutine());
                break;
            case BossStates.Jumping:
               StartCoroutine(JumpCoroutine());
                break;
            case BossStates.Roar:
                StartCoroutine(RoarCoroutine());
                break;
            case BossStates.Roll:
                StartCoroutine(RollCoroutine());
                break;
            case BossStates.Spikes:
                StartCoroutine(SpikesCoroutine());
                break;
            case BossStates.Death:

                break;
            default:

                break;
        }
    }

    IEnumerator WaitingCoroutine()
    {
        //Girar boss
        if(transform.position.x < playerPos.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
        yield return new WaitForSeconds(waitingTime);
        if (transform.position.x < playerPos.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }

        currentState = (BossStates)Random.Range(1, 5);
        ChangeState();
    }


    IEnumerator JumpCoroutine()
    {
        animator.SetBool("IsJumping", true);
        yield return new WaitForSeconds(timeToJump);

        //SEMPRE PRIMER PUNT FINAL EN LA RESTA DE VECTORS
        Vector2 posA = transform.position;
        float posBX = playerPos.position.x;
        float posBY = maxJump;
        float t = 0;

        //Aixo es una parabola
        while (t < 1)
        {
            t += Time.deltaTime * jumpSpeed;

            float posX = Mathf.Lerp(posA.x, posBX, t);
            float posY = posA.y + (4 * maxJump * t * (1-t));

           

            transform.position = new Vector2(posX, posY);

            yield return null;
        }

        animator.SetBool("IsJumping", false);
        currentState = BossStates.Waiting;
        ChangeState();
    }


    IEnumerator RoarCoroutine()
    {
        animator.SetBool("IsRoaring", true);

        yield return new WaitForSeconds(timeToSpawn);

        Instantiate(bettlerPrefab, bettlerSpawnPoint.position, bettlerSpawnPoint.rotation);

        animator.SetBool("IsRoaring", false);
        yield return new WaitForSeconds(timeToSpawn);

        currentState = BossStates.Waiting;
        ChangeState();
    }

    IEnumerator RollCoroutine()
    {
        animator.SetBool("IsRolling", true);
        collisioned = false;
        yield return new WaitForSeconds(timeToRoll);
        //canviar collider
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        float standerColliderX = collider.size.x;
        collider.size = new Vector2(colliderSizeX, collider.size.y);

        while (collisioned == false) 
        {
            transform.Translate(Vector3.left * rollSpeed * Time.deltaTime, Space.Self);
            yield return null;
        } 
        animator.SetBool("IsRolling", false) ;
        //Aixo
        //collisioned = false;
        collider.size = new Vector2 (standerColliderX, collider.size.y) ;
        yield return new WaitForSeconds(timeToRoll);
        currentState = BossStates.Waiting;
        ChangeState();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {

        puntosContatcto = collision.contacts;
        //Debug.Log(collision.GetContact(puntosContatcto.Length - 1).normal);
        
        if (collision.GetContact(puntosContatcto.Length-1).normal.y > -0.6f && collision.GetContact(puntosContatcto.Length - 1).normal.y < 0.6f)
        {
            if (collision.GetContact(puntosContatcto.Length - 1).normal.x > 0.5f || collision.GetContact(puntosContatcto.Length - 1).normal.x < -0.5f)
            {
                collisioned = true;
                //Debug.Log("Funciona!");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Això si vull posar diferent dmg per l'estat tambe es pot fer amb un switch
            //if (currentState == BossStates.Waiting)
            collision.gameObject.GetComponent<PlayerController>().TakeDmg(dmg);
            ContactPoint2D point = collision.GetContact(0);
            if (transform.position.x < playerPos.position.x) //dereta
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.right * knockBackforce);
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.left * knockBackforce);
            }

            StartCoroutine(collision.gameObject.GetComponent<PlayerController>().KnockBackCoroutine());

            /*if (point.normal.y < 0)
            {
                if(point.normal.x > 0)//dreta
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.right * knockBackforce);
                    StartCoroutine(collision.gameObject.GetComponent<PlayerController>().KnockBackCoroutine());
                }
                else //esquerra
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.left * knockBackforce);
                    StartCoroutine(collision.gameObject.GetComponent<PlayerController>().KnockBackCoroutine());
                                    }
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(point.normal * knockBackforce);
                StartCoroutine(collision.gameObject.GetComponent<PlayerController>().KnockBackCoroutine());
            }*/
        }
        }

    IEnumerator SpikesCoroutine()
    {
        animator.SetBool("IsSpiking", true);
        //canviar collider
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        float standerColliderX = collider.size.x;
        collider.size = new Vector2(colliderSizeX, collider.size.y);
        yield return new WaitForSeconds(timeToSpike);
        //per si vols fer algo aqui!
        yield return new WaitForSeconds(timeToTired);


        animator.SetBool("IsSpiking", false);
        //tornar collider normal i state waiting
        collider.size = new Vector2(standerColliderX, collider.size.y);
        currentState = BossStates.Waiting;
        ChangeState();

    }

    public void ShootSpikes()
    {
        for (int i = 0; i < spikesSpawnPoints.Length; i++)
        {
            Instantiate(spikesPrefab, spikesSpawnPoints[i].position, spikesSpawnPoints[i].rotation);
        }
    }

    public void TakeDmg( float _dmg)
    {
        bossLife -= _dmg;
        if (bossLife <= 0)
        {
            //Muerto
            animator.SetTrigger("IsDeath"); 
            StopAllCoroutines();
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            this.enabled = false;
            GameManager.instance.GetGameData.Boss1 = true;
            isDeathBoss = true;
        }
        else
        {

        }
    }
}
