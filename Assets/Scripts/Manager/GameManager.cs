using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    private GameData gameData;
    public int slot;
    public bool comeFromLoadGame; 

    public int doorToGo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        //Nomes per test, BORRAR DESPRES!
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public GameData GetGameData
    {
        get { return gameData; }
        set { gameData = value; }
    }


    public void SaveGame()
    {
        string data = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString("data" + slot.ToString(), data);
    }

    public void LoadGame()
    {
        if( PlayerPrefs.HasKey("data"+ slot.ToString()) == true)
        {
            string data = PlayerPrefs.GetString("data" + slot.ToString());
            gameData =JsonUtility.FromJson<GameData>(data);
        }
    }
}
