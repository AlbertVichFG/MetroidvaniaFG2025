using UnityEngine;

public class EnemyChestController : EnemyController
{
    [SerializeField]
    private bool apears;

    [Header("Horizontal Movment")]
    [SerializeField]
    private float desplaDistance = 3f;
    [SerializeField]
    private float desplaSpeed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
