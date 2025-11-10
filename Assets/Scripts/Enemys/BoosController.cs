using System.Collections;
using UnityEngine;

public class BoosController : MonoBehaviour
{
    public enum BossStates { Waiting, Jumping, Roar, Roll, Spikes, Tired, Death}

    [Header("GeneralConfig")]
    [SerializeField]
    private BossStates currentState;
    private Transform playerPos;
    private Animator animator;


    [SerializeField]
    private float waitingTime;

    [Header("Jump")]
    [SerializeField]
    private float maxJump = 17f;
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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        //Test Only!
        currentState = BossStates.Roar;
        ChangeState();
        ///////
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

                break;
            case BossStates.Tired:

                break;
            case BossStates.Death:

                break;
            default:

                break;
        }
    }

    IEnumerator WaitingCoroutine()
    {
        yield return new WaitForSeconds(waitingTime);
        currentState = (BossStates)Random.Range(1, 5);
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
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        float standerColliderX = collider.size.x;
        collider.size = new Vector2(colliderSizeX, collider.size.y);

        while (collisioned == false) 
        {
            transform.Translate(Vector3.left * rollSpeed * Time.deltaTime, Space.Self);
            yield return null;
        } 
    }
}
