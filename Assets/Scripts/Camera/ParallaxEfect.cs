using UnityEngine;

public class ParallaxEfect : MonoBehaviour
{
    private Transform cam;
    [SerializeField]
    private float percent;
    private Vector3 previousPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main.transform;
        previousPos = cam.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 loQueSeHaMovido = cam.position - previousPos;
        transform.Translate(loQueSeHaMovido *  percent);
        previousPos = cam.position;
    }
}
