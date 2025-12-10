 using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class ReturnToLevels : MonoBehaviour
{
    // Nombre exacto de tu escena de menú de niveles
    public string sceneName = "StartMenu";

    public void VolverAlMenu()
    {
        // Descongelamos el tiempo por si acaso vienes de una pausa
        Time.timeScale = 1f; 
        SceneManager.LoadScene(sceneName);
    }
}