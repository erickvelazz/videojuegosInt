using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // <--- NECESARIO PARA EL NUEVO SISTEMA

public class PauseMenuController : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject pausePanel; 
    public string menuSceneName = "LevelMenu"; 
    
    private bool isPaused = false;

    void Start()
    {
        // Asegurarnos de que el menú empiece apagado al iniciar el nivel
        if(pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        
        // Asegurarnos de que el tiempo corra normal al empezar
        Time.timeScale = 1f;
    }

    void Update()
    {
        // --- CÓDIGO NUEVO PARA DETECTAR LA TECLA ESC ---
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        isPaused = true;
        if(pausePanel != null) pausePanel.SetActive(true); 
        Time.timeScale = 0f; // Congelar tiempo
        
        // Desbloquear mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Reanudar()
    {
        isPaused = false;
        if(pausePanel != null) pausePanel.SetActive(false); 
        Time.timeScale = 1f; // Descongelar tiempo
        
        // Opcional: Bloquear mouse de nuevo si es necesario
        // Cursor.lockState = CursorLockMode.Locked;
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(menuSceneName);
    }
}