using UnityEngine;
using UnityEngine.SceneManagement;

public class SafeStone : MonoBehaviour
{
    [SerializeField]
    private GameObject safeIcon;
    [SerializeField]
    private bool inSafeZone;

    private LevelManager levelManager;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

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
                //saber a quina escena estic i posicio
                GameManager.instance.GetGameData.SceneSave = SceneManager.GetActiveScene().buildIndex;
                GameManager.instance.GetGameData.LastCheckpointPos = transform.position;
                //Guardar partida
                GameManager.instance.SaveGame();

                GameManager.instance.GetGameData.PlayerMana = GameManager.instance.GetGameData.PlayerMaxMana;
                GameManager.instance.GetGameData.PlayerLIFE = GameManager.instance.GetGameData.PlayerMaxLife;
               
                levelManager.UpdateMana();
                levelManager.UpdateLife();
                Debug.Log("Checkpoint guardat a: " + transform.position);
                safeIcon.SetActive(false);
                inSafeZone = false;
                //Efecto de particluas si vols maco
            }
        
        }

    }
}
