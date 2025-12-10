using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject panelSlots;
    [SerializeField]
    private GameObject panelMainMenu;
    [SerializeField]
    private GameObject panelAdvertence;
    [SerializeField]
    private AudioClip bgMusic;


    // Infocada slot partida
    [System.Serializable]
    public class SlotUIData
    {
        public int slotID;
        public TMP_Text titleText;
        public GameObject deleteButton;
    }

    public SlotUIData[] slots;


    private void Start()
    {
        AudioManager.instance.PlayAmbient(bgMusic, 0.2f);
        // Venim del joc no anim inicial
        if (GameManager.instance.cameFromGame)
        {
            panelAdvertence.SetActive(false);
            panelMainMenu.SetActive(true);

            //GameManager.instance.cameFromGame = false; // Reset
            return;
        }

        panelAdvertence.SetActive(true);
        panelMainMenu.SetActive(false);

        RefreshSlots();
    }


    private void RefreshSlots()
    {
        foreach (var _slot in slots)
        {
            if (PlayerPrefs.HasKey("data" + _slot.slotID))
            {
                _slot.titleText.text = "CONTINUE";
                _slot.deleteButton.SetActive(true);

            }
            else
            {
                _slot.titleText.text = "NEW GAME";
                _slot.deleteButton.SetActive(false);

            }
        }
    }


    public void StartBttn()
    {
        GameManager.instance.cameFromGame = true;
        Destroy(panelAdvertence);
        panelMainMenu.SetActive(false);
        panelSlots.SetActive(true);
        RefreshSlots();
        Time.timeScale = 1;
    }


    public void SlotBttn(int _slot)
    {
        if (PlayerPrefs.HasKey("data" + _slot))
        {
            GameManager.instance.slot = _slot;
            GameManager.instance.LoadGame();
            GameManager.instance.comeFromLoadGame = true;
            SceneManager.LoadScene(GameManager.instance.GetGameData.SceneSave);
            GameManager.instance.GetGameData.PlayerLIFE = GameManager.instance.GetGameData.PlayerMaxLife;
            GameManager.instance.GetGameData.PlayerMana = GameManager.instance.GetGameData.PlayerMaxMana;
        }
        else
        {


            GameManager.instance.GetGameData = new GameData();
            GameManager.instance.slot = _slot;
            GameManager.instance.GetGameData.PlayerLIFE = 100;
            GameManager.instance.GetGameData.PlayerMaxLife = 100;
            GameManager.instance.GetGameData.PlayerMana = 75;
            GameManager.instance.GetGameData.PlayerMaxMana = 75;
            GameManager.instance.GetGameData.PlayerDmg = 7;
            GameManager.instance.GetGameData.FireballDmg = 30;
            GameManager.instance.GetGameData.HeavyDmg = 15;
            GameManager.instance.GetGameData.MaxJumps = 1;

            GameManager.instance.GetGameData.CanDash = false;
            GameManager.instance.GetGameData.HasFireBall = false;
            GameManager.instance.GetGameData.CanCrouch = false;
            GameManager.instance.GetGameData.CanGrabWall = false;
            GameManager.instance.GetGameData.CanHeal = false;

            SceneManager.LoadScene(1);
        }

        Time.timeScale = 1;

    }


    public void DeleteSlot(int slotID)
    {
        PlayerPrefs.DeleteKey("data" + slotID);
        RefreshSlots();
    }


    public void ExitButton()
    {
        Application.Quit();
    }

    public void BackToMM()
    {
        panelMainMenu.SetActive(true);
        panelSlots.SetActive(false);
    }

    public void EndAdvertence()
    {
        panelAdvertence.SetActive(false);
        panelMainMenu.SetActive(true);

        //Guardar perque no torni a saltar l'anim del principi
        PlayerPrefs.SetInt("AdvertencePlayed", 1);
    }
}
