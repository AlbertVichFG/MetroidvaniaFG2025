using UnityEngine;

public class LanceGoblin : MonoBehaviour
{
    [SerializeField]
    private float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Treure DMG
            Debug.Log("I Hit the player");
        }
        Destroy(gameObject);

    }
}
