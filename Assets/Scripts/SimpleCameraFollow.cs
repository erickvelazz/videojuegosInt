using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target; // Arrastra tu personaje aquí
    public Vector3 offset;   // La distancia y ángulo de la cámara

    void Start()
    {
        // Calcula el offset inicial para no tener que adivinarlo
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        // En LateUpdate para que se mueva después de que el jugador ya lo hizo
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}