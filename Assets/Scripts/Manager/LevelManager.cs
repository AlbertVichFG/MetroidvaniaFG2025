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
        }
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
        Time.timeScale = 0;
        panelWin.SetActive(true);
    }
}
