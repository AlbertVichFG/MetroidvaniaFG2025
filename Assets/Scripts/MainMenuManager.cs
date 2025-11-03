using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelSlots;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartBttn()
    {
        panelSlots.SetActive(true);
    }

    public void SlotBttn(int _slot)
    {
        if (PlayerPrefs.HasKey("data" + _slot.ToString()))
        {
            GameManager.instance.slot = _slot;
            GameManager.instance.LoadGame();
            SceneManager.LoadScene(GameManager.instance.GetGameData.SceneSave);
        }
        else
        {
            GameManager.instance.GetGameData = new GameData();
            GameManager.instance.slot = _slot;
            GameManager.instance.GetGameData.PlayerLIFE = 100;
            GameManager.instance.GetGameData.PlayerMaxLife = 100;
            GameManager.instance.GetGameData.PlayerMana = 50;
            GameManager.instance.GetGameData.PlayerMaxMana = 50;
            GameManager.instance.GetGameData.PlayerDmg = 25;
            GameManager.instance.GetGameData.FireballDmg = 15;
            GameManager.instance.GetGameData.HeavyDmg = 35;
            SceneManager.LoadScene(1);
        }
    }

    public void BackBttn()
    {

    }
}
