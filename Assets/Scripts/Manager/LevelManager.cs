using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Image lifeBar;
    [SerializeField]
    private Image manaBar;
    [SerializeField]
    private Transform[] doorsPoints;

    GameManager gameManager;



    [Header("Panels")]
    [SerializeField]
    public GameObject panelGameOver;
    [SerializeField]
    private GameObject panelPause;
    [SerializeField]
    private GameObject panelWin;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        //Aqui hem tirat de CHAT GPT no aconseguia que al morir fes spwan a la safeStone

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (GameManager.instance.comeFromLoadGame)
        {
            // El jugador ve de carregar partida
            GameManager.instance.comeFromLoadGame = false;

            // Carregar escena i posició guardada
            Vector3 spawnPos = GameManager.instance.GetGameData.LastCheckpointPos;

            if (spawnPos == Vector3.zero)
            {
                if (doorsPoints.Length > 0)
                    spawnPos = doorsPoints[GameManager.instance.doorToGo].position;
            }

            player.transform.position = spawnPos;
            player.transform.rotation = Quaternion.identity;
        }
        else
        {
            // Spawn normal del nivell (portes)
            player.transform.position = doorsPoints[GameManager.instance.doorToGo].position;
            player.transform.rotation = doorsPoints[GameManager.instance.doorToGo].rotation;
        }

        UpdateMana();
        UpdateLife();

        ///////////////////////////        
        /*

        gameManager = GetComponent<GameManager>();
        if (GameManager.instance.comeFromLoadGame == true)
        {
            GameManager.instance.comeFromLoadGame = false;
            GameObject.FindGameObjectWithTag("Player").transform.position = 
                GameObject.Find("SafeStone").transform.position;
            GameObject.FindGameObjectWithTag("Player").transform.rotation =
                GameObject.Find("SafeStone").transform.rotation;
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").transform.position =
                doorsPoints[GameManager.instance.doorToGo].position;
            GameObject.FindGameObjectWithTag("Player").transform.rotation =
                doorsPoints[GameManager.instance.doorToGo].rotation;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLife()
    {
        lifeBar.fillAmount = GameManager.instance.GetGameData.PlayerLIFE / GameManager.instance.GetGameData.PlayerMaxLife;
    }

    public void UpdateMana()
    {
        manaBar.fillAmount = GameManager.instance.GetGameData.PlayerMana / GameManager.instance.GetGameData.PlayerMaxMana;

    }


    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);

    }


    public void GameOverPanel()
    {
        panelGameOver.SetActive(true);
        Time.timeScale = 0;
    }

    public void Pause()
    {
        if (panelPause.activeInHierarchy == false)
        {
            panelPause.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            panelPause.SetActive(false);
            Time.timeScale = 1;
        }
    }


    public void WinGameEnd()
    {
        
        panelWin.SetActive(true);
        Time.timeScale = 0;
    }

    //Swapn on toca?

    public void RespawnDeath()
    {
        Time.timeScale = 1;

        GameManager.instance.comeFromLoadGame = true;
        GameManager.instance.LoadGame();
        SceneManager.LoadScene(GameManager.instance.GetGameData.SceneSave);
    }


}
