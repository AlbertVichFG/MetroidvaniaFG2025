using UnityEngine;

public class TorchLight : MonoBehaviour
{
    [SerializeField]
    private float minScale, maxScale;
    [SerializeField]
    private float updateTime;
    private float timePass = 0;


    void FixedUpdate()
    {
        //ajustar temps escalat llum
        if ( timePass >= updateTime)
        {
            float randomScale = Random.Range(minScale, maxScale);
            transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            timePass = 0;
        }

        timePass += Time.fixedDeltaTime;
    }
}
