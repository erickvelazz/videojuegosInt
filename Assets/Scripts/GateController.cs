using UnityEngine;

public class GateController : MonoBehaviour
{
    public float openHeight = 3.0f; // Cuánto va a subir
    public float openSpeed = 2.0f;  // Velocidad de subida
    private bool isOpen = false;
    private Vector3 targetPosition;

    void Start()
    {
        // Calculamos la posición final sumando la altura actual + lo que debe subir
        targetPosition = transform.position + new Vector3(0, openHeight, 0);
    }

    void Update()
    {
        // Si la bandera isOpen es verdadera, movemos la puerta hacia arriba
        if (isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, openSpeed * Time.deltaTime);
        }
    }

    // Esta función la llamará el Jefe al morir
    public void OpenGate()
    {
        isOpen = true;
        Debug.Log("¡La puerta se está abriendo!");
    }
}