using UnityEngine;

public class SkullBomb : MonoBehaviour
{
    [Header("Stats")]
    public float life = 10f;
    public float dmg = 10f;
    public float speed = 3f;

    [Header("Explosion")]
    public float explosionRadius = 2f;
    public float explosionDamage = 25f;
    public LayerMask playerMask;
    public GameObject explosionFx;

    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;
    public Transform player;

    private bool playerDetected = false;
    private bool exploded = false;

    void Start()
    {
        animator.Play("Idle");
    }

    void Update()
    {
        if (!playerDetected || exploded) return;

        // Moviment cap al player
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);

        animator.Play("Fly");
    }

    // Detecta el player the same as EnemyController
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            player = other.transform;

            animator.SetTrigger("Alert");
        }
    }

    // Rep dany
    public void TakeDmg(float amount)
    {
        life -= amount;

        if (life <= 0)
        {
            Explode();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (exploded) return;

        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerController>()?.TakeDmg(dmg);
            Explode();
        }

        if (collision.collider.CompareTag("Ground"))
        {
            Explode();
        }
    }

    void Explode()
    {
        if (exploded) return;
        exploded = true;

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        GetComponent<Collider2D>().enabled = false;

        animator.Play("Death");

        // FX
        if (explosionFx)
            Instantiate(explosionFx, transform.position, Quaternion.identity);

        // AOE damage
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerMask);
        foreach (Collider2D h in hit)
        {
            h.GetComponent<PlayerController>()?.TakeDmg(explosionDamage);
        }

        Destroy(gameObject, 0.4f);
    }
}
