using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    private int sceneToGo;
    [SerializeField]
    private int doorPoint;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.doorToGo = doorPoint;
            SceneManager.LoadScene(sceneToGo);
        }
    }
}
