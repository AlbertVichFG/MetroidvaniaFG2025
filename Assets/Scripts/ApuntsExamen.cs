using UnityEngine;

public class ApuntsExamen : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        //Primer
    }

    private void OnEnable()
    {
        //Cada vegada que s'activa
    }

    private void OnDisable()
    {
        //Cada vegada es desactiva
    }

    void Start()
    {
        //Quan s'inicia


        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Cada frame   

        //Moviment LOCAL transfom
        transform.Translate(Vector3.right * 5 * Time.deltaTime ); //left up back forward);

        //Moviemnt WORLD trasfom
        transform.Translate(Vector3.up * 5 * Time.deltaTime, Space.World);

        //Fisicas
        //WORLD
        rb.linearVelocity = Vector3.forward * 6;
        //LOCAL
        rb.linearVelocity= transform.forward * 4;

        rb.AddForce(Vector3.down * 34);
    }

    private void LateUpdate()
    {
        
    }

    private void FixedUpdate()
    {
        //Temps determinat per nosaltres
    }


    //SENSE RB NO ES PODEN DETECTAR COLLISIONS
    private void OnCollisionEnter(Collision collision)
    {
        //Primer frame entra
    }

    private void OnCollisionExit(Collision collision)
    {
        //Primer frame qun surt
    }

    private void OnCollisionStay(Collision collision)
    {
        //Mentres hi hagi contacte i moviment
    }

    //Areas de deteccio
    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
