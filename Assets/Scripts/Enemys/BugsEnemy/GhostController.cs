using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class GhostController : EnemyController
{
    [Header("Fire Settings")]
    [SerializeField]
    private GameObject purpleShoot;
    [SerializeField]
    private Transform spwanPoint;
    

    [Header("Shoot")]
    [SerializeField]
    private float bullet;
    [SerializeField]
    private float shotDelay;
    [SerializeField]
    private float shootReset;
    [SerializeField]
    private bool shooting;

    [Header("Horizontal Movment")]
    [SerializeField]
    private float desplaDistance = 3f;
    [SerializeField]
    private float desplaSpeed = 2f;


    private Vector3 startPos;

    void Start()
    {
        base.Start();
        startPos = transform.position;
    }

    void Update()
    {
        if (isDeath == true)
        {
            return;
        }
        LookAtPlayer();
        MoveInfinit();

        if (attacking && !shooting)
        {
            StartCoroutine(ShootPurple());
        }

    }


    private void MoveInfinit()
    {
        // Moviment lateral
        float t = Time.time * desplaSpeed;

        // Infinit 
        float offsetX = Mathf.Sin(t) * desplaDistance;
        float offsetY = Mathf.Sin(t * 2f) * (desplaDistance / 2f);

        transform.position = new Vector3(
            startPos.x + offsetX, startPos.y + offsetY, transform.position.z);
    }


    IEnumerator ShootPurple()
    {
        shooting = true;
       
        yield return new WaitForSeconds(0.2f);
        while (bullet < 3)
        {
            animator.SetBool("IsAttacking", true);
            // Debug.Log("Bullet: " + bullet);
            Instantiate(purpleShoot, spwanPoint.position, spwanPoint.rotation * Quaternion.Euler(0, 180, 0));
            bullet++;
            yield return new WaitForSeconds(shotDelay);

        }
        animator.SetBool("IsAttacking", false);
        attacking = false;

        // Reinici
        Debug.Log("Reload");
        bullet = 0;
        yield return new WaitForSeconds(shootReset);
        shooting = false;
        Debug.Log(bullet + "Bales");

    }

    private void LookAtPlayer()
    {
        if (player == null) return;

        float dir = player.position.x - transform.position.x;

        if (dir > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
    }


    //Stay aixi attacking es mante true i pot tornar a disparar
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("DETECTO EL PLAYER");
            attacking = true;
        }
    }

}
