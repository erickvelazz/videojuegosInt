using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset;   

    void Start()
    {
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}