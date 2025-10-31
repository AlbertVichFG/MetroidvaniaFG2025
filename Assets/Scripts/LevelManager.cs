using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Image lifeBar;
    [SerializeField]
    private Image manaBar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
