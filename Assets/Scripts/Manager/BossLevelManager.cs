using Unity.VisualScripting;
using UnityEngine;

public class BossLevelManager : LevelManager
{
    [SerializeField]
    private BoosController boos;
    [SerializeField]
    private Animator[] doorAnimator;
    [SerializeField]
    private GameObject[] doorObjects;


    private void Start()
    {
        base.Start();
        if(GameManager.instance.GetGameData.Boss1 == true)
        {
            for (int i = 0; i < doorAnimator.Length; i++)
            {
                doorAnimator[i].GetComponent<Collider2D>().enabled = false;
            }
            boos.SetDeathAtStart();
        }
    }

   
    public void StartBattle()
    {
        boos.enabled = true;

        foreach(Animator anim in doorAnimator)
        {
            anim.SetBool("Close", true);
        }

    }

    public void openDoors()
    {
        if (boos.isDeathBoss)
        {
            foreach (Animator anim in doorAnimator)
            {
                anim.SetBool("Close", false);
                anim.SetTrigger("Open");
                anim.GetComponentInChildren<Collider2D>().enabled = false;
            }
            
        }

    }
}
