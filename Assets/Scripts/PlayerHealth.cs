using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections; 

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 3; 
    public string sceneToRestart = "SampleScene"; 
    
    [Header("Invulnerabilidad")]
    public float invulnerabilityDuration = 0.5f; 
    private bool isInvulnerable = false; 
    
    void Start()
        {
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
        Debug.Log("¡Game Over! Reiniciando escena...");
        
        SceneManager.LoadScene(sceneToRestart); 
    }
}