using UnityEngine;

public class SpikesController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float dmg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right *-1 * speed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = rb.linearVelocity.normalized;
        Vector2 objetive = transform.position + direction;

        transform.LookAt(objetive);
        transform.Rotate(0, 90, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDmg(dmg);
     
        }

      Destroy(gameObject);
    }


}
