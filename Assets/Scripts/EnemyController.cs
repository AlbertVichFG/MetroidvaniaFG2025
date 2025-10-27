using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float life;
    [SerializeField]
    private float dmg;
    [SerializeField]
    private float speed;
    private bool playerDetected;

    private Transform player;

    private Rigidbody2D rb;
    private Animator animator;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (playerDetected == true)
        {
            Vector3 distance = player.transform.position - transform.position;
            if (distance.x > 0) //dreta
            {
                rb.linearVelocity = speed * Vector2.right;
            }
            else if(distance.x < 0) //Esquerra
            {
                rb.linearVelocity = speed * Vector2.left;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag =="Player")
        {
            playerDetected = true;
            player = collision.transform;
        }
    }
}
