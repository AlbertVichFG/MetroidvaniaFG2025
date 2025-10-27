using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = speed * transform.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag != "Player")
        {
            animator.SetTrigger("Death");
            speed = 0.75f;
        }
    }

    public void DestroyBall()
    {
        Destroy(gameObject);
    } 
}
