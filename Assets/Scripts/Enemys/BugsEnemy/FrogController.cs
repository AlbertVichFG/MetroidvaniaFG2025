using System.Collections;
using UnityEngine;

public class FrogController : EnemyController
{
    [SerializeField]
    private float timeToLick;
    [SerializeField]
    private float frogCombo;
    [SerializeField]
    private AudioClip frogSound;
    private bool isLicking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

        base.Update();
        if (attacking == true && !isLicking)
        {
            StartCoroutine(LametazoAtackCoroutine());
        }
    }

    IEnumerator LametazoAtackCoroutine()
    {
        isLicking = true;

        if (frogCombo == 0)
        {
            animator.SetBool("IsAttacking", true);
            frogCombo = 1;
            animator.SetBool("PlayerDetect", false);
        }

        Vector3 distance = player.position - transform.position;
        float distanceSq = distance.sqrMagnitude;
        if (distanceSq > Mathf.Pow(stopDistance, 2) && frogCombo == 1)
        {
            attacking = false;
            animator.SetBool("IsAttacking", false);
            animator.SetBool("PlayerDetect", true);

            // 
            frogCombo = 0;
            isLicking = false;
            yield break;
        }
        yield return new WaitForSeconds(timeToLick);

        // REBRE HIT i seguir l’atac
        if (!isDeath)
            attacking = true;

        isLicking = false;
    }

    public void playFrog()
    {
        AudioManager.instance.PlaySFX(frogSound, 0.5f);
    }
}
