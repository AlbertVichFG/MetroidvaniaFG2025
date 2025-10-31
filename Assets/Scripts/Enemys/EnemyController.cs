using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header ("Config")]
    [SerializeField]
    private float life;
    [SerializeField]
    private float dmg;
    [SerializeField]
    private float speed;
    private bool playerDetected;
    public bool attacking;
    public float stopDistance;
    private bool isDeath;

    public Transform player;

   

    private Rigidbody2D rb;
    public Animator animator;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (isDeath == true)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }
        if (playerDetected == true && attacking == false)
        {
            Vector3 distance = player.transform.position - transform.position;
            if (distance.x > 0) //Dreta
            {
                rb.linearVelocity = speed * Vector2.right;
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if(distance.x < 0) //Esquerra
            {
                rb.linearVelocity = speed * Vector2.left;
                transform.eulerAngles = Vector3.zero;
            }

            //Calcular quan enemic esta a prop del jugador per atacar
            Vector3 distanceStop = player.position - transform.position;
            float distanceSQR = distanceStop.sqrMagnitude;
            if (distanceSQR <= Mathf.Pow(stopDistance, 2))
            {
                attacking = true;
                rb.linearVelocity = Vector2.zero;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag =="Player")
        {
            animator.SetTrigger("Alert");
            Invoke("StartMoving", animator.GetCurrentAnimatorStateInfo(0).length);
            player = collision.transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDmg(dmg);
        }
    }

    private void StartMoving()
    {
        playerDetected = true;
        animator.SetBool("PlayerDetect", true);
    }

    public void TakeDmg(float _dmg)
    {
        life -= _dmg;
        if (life <= 0)
        {
            //Muerte fer que es quedi el cos 
            animator.SetTrigger("Death");
            rb.gravityScale = 0;
            GetComponent<Collider2D>().enabled = false;
            isDeath = true;
        }
        else
        {
            //hit
            animator.SetTrigger("Hit");

        }
    }
}
