using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections; 

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 3; 
    public string sceneToRestart = "SampleScene"; 

    [Header("Configuración UI Game Over")]
    public GameObject gameOverPanel; 
    public string menuSceneName = "LevelMenu";
    
    [Header("Invulnerabilidad")]
    public float invulnerabilityDuration = 0.5f; 
    private bool isInvulnerable = false; 
    
    void Start()
    {
        Time.timeScale = 1f;

        currentHealth = 3; 
        Debug.Log("Juego iniciado. Vidas: " + currentHealth);
        if(UIManager.Instance != null)
            UIManager.Instance.UpdateLives(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable)
        {
            return;
        }
        
        StartCoroutine(BecomeInvulnerable());
        
        currentHealth -= amount;
        Debug.Log("Jugador golpeado. Vidas restantes: " + currentHealth);

        if(UIManager.Instance != null) 
            UIManager.Instance.UpdateLives(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;
        
        yield return new WaitForSeconds(invulnerabilityDuration);
        
        isInvulnerable = false;
        
    }

    void Die()
    {
        Debug.Log("¡Game Over!");
        
        // 1. Mostrar el cursor del mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 2. Activar el Panel de Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 3. Detener el tiempo del juego
        Time.timeScale = 0f;
    }

    // --- FUNCIONES PARA LOS BOTONES DEL PANEL ---

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f; // Importante: Descongelar el tiempo
        SceneManager.LoadScene(sceneToRestart);
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f; // Importante: Descongelar el tiempo
        SceneManager.LoadScene(menuSceneName);
    }
}