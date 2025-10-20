using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed;
    
    private Rigidbody2D rb; 
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = speed * Vector2.right * horizontal;

        if(horizontal == 0f)
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
        else if (horizontal > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }

    }

    
}
