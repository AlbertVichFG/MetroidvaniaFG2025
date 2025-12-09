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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

        base.Update();
        if (attacking == true)
        {
            StartCoroutine(LametazoAtackCoroutine());
        }
    }

    IEnumerator LametazoAtackCoroutine()
    {
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
        }
        else
        {
            yield return new WaitForSeconds(timeToLick);

        }
    }

    public void playFrog()
    {
        AudioManager.instance.PlaySFX(frogSound, 0.5f);
    }
}
