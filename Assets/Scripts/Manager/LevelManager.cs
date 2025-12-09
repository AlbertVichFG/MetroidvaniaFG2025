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


    [Header("GEMMS")]
    [SerializeField]
    private Image gemmeHeal;
    [SerializeField]
    private Image gemmeFire;
    [SerializeField]
    private Image gemmeJumps;
    [SerializeField]
    private Image gemmeDash;
    [SerializeField]
    private Image gemmeWall;
    [SerializeField]
    private Image gemmeCrouch;
    private string valorColorOriginal = "#FFFFFF";


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
            GemmesUpdate();
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


    public void GemmesUpdate()
    {

        //aqui vaig fer el codi molt cutre i li he demanat a CHATGPT que mel faci bonic et deio una de mostra del que tenia jeje
        /*var dates = GameManager.instance.GetGameData;
        Color colorOriginal;
        if (dates.CanHeal)
        {
            if (ColorUtility.TryParseHtmlString(valorColorOriginal, out colorOriginal))
            {
                gemmeHeal.color = colorOriginal;
            }
        }*/
        // Obtenim les dades del joc amb comprovació de seguretat
        var dates = GameManager.instance?.GetGameData;
        if (dates == null) return;

        // Convertim el valor hex a Color només una vegada
        if (!ColorUtility.TryParseHtmlString(valorColorOriginal, out Color colorOriginal)) return;

        if (dates.CanHeal && gemmeHeal != null) gemmeHeal.color = colorOriginal;
        if (dates.HasFireBall && gemmeFire != null) gemmeFire.color = colorOriginal;
        if (dates.MaxJumps > 1 && gemmeJumps != null) gemmeJumps.color = colorOriginal;
        if (dates.CanDash && gemmeDash != null) gemmeDash.color = colorOriginal;
        if (dates.CanGrabWall && gemmeWall != null) gemmeWall.color = colorOriginal;
        if (dates.CanCrouch && gemmeCrouch != null) gemmeCrouch.color = colorOriginal;
    }
}


