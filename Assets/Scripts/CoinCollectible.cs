using UnityEngine;
using System.Collections; // Necesario para usar Coroutines

public class CoinCollectible : MonoBehaviour
{
    // --- PARÁMETROS CONFIGURABLES ---
    public float rotationSpeed = 100f; 
    public float moveDistance = 0.5f; 
    public float moveDuration = 0.15f; 
    
    // Bandera para evitar colecciones múltiples mientras se mueve
    private bool isCollected = false; 

    void Update()
    {
        // Solo rotar si no ha sido coleccionada
        if (!isCollected)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 1. Verificación: Si ya está en proceso de colección O no es el jugador, salimos.
        if (isCollected || !other.CompareTag("Player"))
        {
            return;
        }

        // 2. Iniciar el proceso de colección con el movimiento
        isCollected = true; // Bloquea la colección y la rotación
        StartCoroutine(CollectAndMove());
    }

    IEnumerator CollectAndMove()
    {
        // Lógica de juego (Puntuación)
        Debug.Log("¡Moneda recolectada! Activando efecto de movimiento.");

        if(UIManager.Instance != null) 
        {
            UIManager.Instance.AddCoin();
        }

        // --- EFECTO DE MOVIMIENTO ---
        
        Vector3 startPosition = transform.position;
        Vector3 initialScale = transform.localScale;

        float startTime = Time.time;
        float fraction = 0f;

        // Bucle para mover la moneda suavemente
        while (fraction < 1f)
        {
            // Calcula la fracción de tiempo transcurrida
            fraction = Mathf.Clamp01((Time.time - startTime) / moveDuration);
            
            // Movimiento: Subir y bajar (arco parabólico)
            // Usamos Sin(fraction * PI) para que vaya de 0 a 1 y vuelva a 0
            float yOffset = Mathf.Sin(fraction * Mathf.PI) * moveDistance;
            transform.position = startPosition + Vector3.up * yOffset;

            // Escala: Reducir tamaño hasta desaparecer ("guardándose")
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, fraction);
            
            yield return null; // Espera un frame
        }

        // Asegura que desaparezca visualmente
        transform.localScale = Vector3.zero;

        // Destruir el objeto después de que el movimiento ha terminado
        Destroy(gameObject);
    }
}