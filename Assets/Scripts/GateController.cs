using UnityEngine;

public class GateController : MonoBehaviour
{
    public float openHeight = 3.0f;
    public float openSpeed = 2.0f; 
    private bool isOpen = false;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position + new Vector3(0, openHeight, 0);
    }

    void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, openSpeed * Time.deltaTime);
        }
    }

    public void OpenGate()
    {
        isOpen = true;
        Debug.Log("¡La puerta se está abriendo!");
    }
}