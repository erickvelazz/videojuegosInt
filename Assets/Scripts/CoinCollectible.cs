using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo
    public float rotationSpeed = 100f;

    void Update()
    {
        // Hacer girar la moneda sobre su eje Y constantemente
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    // Este método se llama cuando otro objeto con un Collider entra en el Trigger de la moneda
    void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entró es el jugador
        // Asegúrate de que tu objeto jugador tenga el Tag "Player" asignado
        if (other.CompareTag("Player"))
        {
            // AQUÍ PUEDES AÑADIR LÓGICA PARA SUMAR PUNTOS
            // Por ejemplo: GameManager.instance.AddScore(1);
            Debug.Log("¡Moneda recolectada!");

            // Destruir el objeto moneda
            Destroy(gameObject);
        }
    }
}