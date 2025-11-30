using UnityEngine;

public class BeetleController : EnemyController
{

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
            animator.SetBool("IsAttacking", true);

            Vector3 distance = player.position - transform.position;
            float distanceSq = distance.sqrMagnitude;
            if(distanceSq > Mathf.Pow(stopDistance, 2))
            {
                attacking = false;
                animator.SetBool("IsAttacking", false);

            }
        }
    }
}
