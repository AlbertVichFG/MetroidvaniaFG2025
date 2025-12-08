using UnityEngine;

public class ComboCheking : MonoBehaviour
{
    private PlayerController player;

    private LevelManager levelManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishAttack1()
    {
        player.CheckCombo1();
    }

    public void FinishAttack2()
    {
        player.CheckCombo2();

    }

    public void FinishAttackHeavy()
    {
        player.FinishHeavyAttack();
    }


    //PosoAixò aqui  espero que no doni problemes;
    public void GameOverSpawn()
    {
        levelManager.GameOverPanel();
    }
}
