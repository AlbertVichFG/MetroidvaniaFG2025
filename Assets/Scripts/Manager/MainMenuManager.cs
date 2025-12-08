using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject panelSlots;
    [SerializeField]
    private GameObject panelMainMenu;
    [SerializeField]
    private GameObject panelAdvertence;



    [System.Serializable]
    public class SlotUIData
    {
        public int slotID;
        public TMPro.TMP_Text titleText;
        public GameObject deleteButton;
    }

    public SlotUIData[] slots;


    private void Start()
    {
        //Desactivar Panel Animacio
        if (PlayerPrefs.GetInt("AdvertencePlayed", 0) == 1)
        {
            panelAdvertence.SetActive(false);
            panelMainMenu.SetActive(true);
        }

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
