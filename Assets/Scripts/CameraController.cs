using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Vector3 camOffset;
    [SerializeField]
    private float minX, maxX, minY, maxY;



    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Clamp(player.position.x,minX,maxX);
        float y = Mathf.Clamp(player.position.y, minY, maxY);
        transform.position = new Vector3(x, y,camOffset.z);


    }


}
