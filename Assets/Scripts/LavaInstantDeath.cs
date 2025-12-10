using UnityEngine;

public class LavaInstantDeath : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MatarJugador(collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MatarJugador(other.gameObject);
        }
    }

    void MatarJugador(GameObject jugador)
    {
        PlayerHealth salud = jugador.GetComponent<PlayerHealth>();
        
        if (salud != null)
        {
            Debug.Log("¡Caíste en la lava!");
            salud.TakeDamage(9999);
        }
    }
}