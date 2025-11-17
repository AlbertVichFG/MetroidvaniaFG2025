using UnityEngine;

public class BossLevelManager : LevelManager
{
    [SerializeField]
    private BoosController boos;
    [SerializeField]
    private Animator[] doorAnimator;


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
        /*for (int i = 0; i < doorAnimator.Length; i++)
        {
            doorAnimator[i].SetBool("Close", true);
        }*/

        foreach(Animator anim in doorAnimator)
        {
            anim.SetBool("Close", true);
        }



        //Començar musica
        //Barra vida
        //VFX
    }
}
