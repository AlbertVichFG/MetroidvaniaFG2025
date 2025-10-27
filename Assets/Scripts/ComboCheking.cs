using UnityEngine;

public class ComboCheking : MonoBehaviour
{
    private PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
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
}
