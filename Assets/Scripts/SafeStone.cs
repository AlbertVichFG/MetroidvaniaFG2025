using UnityEngine;
using UnityEngine.SceneManagement;

public class SafeStone : MonoBehaviour
{
    [SerializeField]
    private GameObject safeIcon;
    [SerializeField]
    private bool inSafeZone;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            safeIcon.SetActive(true);
            inSafeZone = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            safeIcon.SetActive(false);
            inSafeZone = false;
        }
    }



    void Update()
    {
        if (inSafeZone == true) {
            if(Input.GetAxis("Vertical") > 0.5f)
            {
                //saber a quina escena estic
                GameManager.instance.GetGameData.SceneSave = SceneManager.GetActiveScene().buildIndex;
                //Guardar partida
                GameManager.instance.SaveGame();
                inSafeZone = false;
                //Efecto de particluas si vols maco
            }
        
        }

    }
}
