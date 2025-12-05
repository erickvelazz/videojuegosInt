using UnityEngine;

public class ChestWin : MonoBehaviour
{
    // Opcional: Si tienes una escena de victoria o un panel de UI
    // public GameObject winPanel; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡HAS GANADO EL JUEGO!");
            
            // Aquí puedes detener el tiempo o cargar la escena de créditos
            // Time.timeScale = 0; 
            // O cargar escena: SceneManager.LoadScene("WinScene");
            
            // Feedback visual (opcional): Abrir el cofre
            // GetComponent<Animator>().SetTrigger("Open");
        }
    }
}