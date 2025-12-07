using UnityEngine;

public class ChestWin : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡HAS GANADO EL JUEGO!");
        }
    }
}