using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameData gameData;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameData GetGameData
    {
        get { return gameData; }
        set { gameData = value; }
    }
}
