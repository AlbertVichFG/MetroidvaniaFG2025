using System.Collections;
using UnityEngine;

public class GhostController : EnemyController
{
    [Header("Fire Settings")]
    [SerializeField]
    private GameObject purpleShoot;
    [SerializeField]
    private Transform spwanPoint;
    [SerializeField]
    private float shotDelay = 0.3f;

    [Header("Vertical Movement")]
    [SerializeField]
    private float upDistance = 3f;
    [SerializeField]
    private float moveDuration = 1f;

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
            MoveHorizontal();
    }


    private void MoveHorizontal()
    {
        // Moviment lateral 
        float offset = Mathf.Sin(Time.time * desplaSpeed) * desplaDistance;
        transform.position = new Vector3(startPos.x + offset, transform.position.y, transform.position.z);
    }


    IEnumerator ShootMoveSequence()
    {
        Debug.Log("EntraAqui");

        animator.SetBool("IsAttacking", true);

        Vector3 originalPos = transform.position;
        Vector3 topPos = originalPos + Vector3.up * upDistance;

        //Pujar
       yield return StartCoroutine(MoveSmooth(originalPos, topPos, moveDuration));

        //Disparar 3 boles de foc
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("ATAKA MOSTRE DEL ULL");
            Shoot();
            yield return new WaitForSeconds(shotDelay);
        }

        //Baixar
        yield return StartCoroutine(MoveSmooth(topPos, originalPos, moveDuration));

        animator.SetBool("IsAttacking", false);

        attacking = false;
    }

    void Shoot()
    {
        Instantiate(purpleShoot, spwanPoint.position, spwanPoint.rotation);
    }

   IEnumerator MoveSmooth(Vector3 from, Vector3 to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            transform.position = Vector3.Lerp(from, to, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
    }


}
