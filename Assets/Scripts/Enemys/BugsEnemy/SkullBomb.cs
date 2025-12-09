using UnityEngine;

public class SkullBomb : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private float life;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float maxSpeed ;

    [Header("Explosion")]
    [SerializeField]
    private float explosionRadius = 2f;
    [SerializeField]
    private float explosionDamage = 25f;
    [SerializeField]
    private float knockbackHorizontal = 8f;
    [SerializeField]
    private float knockbackVertical = 5f;
    [SerializeField]
    private LayerMask playerMask;
    [SerializeField]
    private AudioClip explosionAudio;


    [Header("References")]
    private Rigidbody2D rb;
    private Animator animator; 
    private Transform player;
    [SerializeField]
    public bool isSkull;

    private bool playerDetected = false;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isSkull = true;
    }

    void Update()
    {
        if (!playerDetected || isDead) return;

        // Direcció flotant cap al player (no cau)
        Vector2 direction = (player.position - transform.position).normalized;

        // FLIP DEL SPRITE
        if(direction.x > 0) { 
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x < 0){

            transform.localScale = new Vector3(1, 1, 1);
        }

        rb.AddForce(direction * acceleration);

        // Limit la velocitat 
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);

        animator.Play("Fly");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            player = other.transform;

        }
    }

    public void TakeDmg(float _amount)
    {
        if (isDead) return;

        life -= _amount;
        if (life <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Ground"))
        {
            animator.SetTrigger("Hit");
        }
        Die();
        AudioManager.instance.PlaySFX(explosionAudio, 0.5f);



    }

    private void Die()
    {
        if (isDead) return;
        
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        animator.SetTrigger("Hit");

        DoExplosionDamage();

        Destroy(gameObject, 0.5f);
    }

    void DoExplosionDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerMask);

        foreach (Collider2D h in hits)
        {
            PlayerController player = h.GetComponent<PlayerController>();
            if (player != null)
            {
                // Dmg
                player.TakeDmg(explosionDamage);

                // Knockback
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Direcció horitzontal: cap al costat oposat de la bomba
                    float dirX = Mathf.Sign(player.transform.position.x - transform.position.x);

                    Vector2 force = new Vector2(dirX * knockbackHorizontal, knockbackVertical);
                    rb.linearVelocity = Vector2.zero; // reset de velocitat per consistència
                    rb.AddForce(force, ForceMode2D.Impulse);
                }
            }
        }
    }
}
