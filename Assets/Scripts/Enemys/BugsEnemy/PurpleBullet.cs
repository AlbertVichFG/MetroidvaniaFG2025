using UnityEngine;

public class PurpleBullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float dmg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Treure DMG
            Debug.Log("I Hit the player");
            collision.gameObject.GetComponent<PlayerController>().TakeDmg(dmg);
        }
        Destroy(gameObject);

    }
}
