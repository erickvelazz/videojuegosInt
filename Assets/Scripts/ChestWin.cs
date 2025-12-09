using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Necesario para los textos

public class ChestWin : MonoBehaviour
{
    [Header("UI Referencias")]
    public GameObject winPanel;          // Arrastra aquí tu panel "WinPanel"
    public TextMeshProUGUI coinsText;    // Arrastra aquí el texto de "Monedas: 0"
    
    [Header("Configuración de Escenas")]
    public string nextLevelName = "Level2"; // Nombre exacto de tu siguiente nivel
    public string menuSceneName = "LevelMenu"; // Nombre de tu menú de niveles

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GanarJuego();
        }
    }

    void GanarJuego()
    {
        Debug.Log("¡Nivel Completado!");

        // 1. Mostrar el Panel
        winPanel.SetActive(true);

        // 2. Detener el tiempo
        Time.timeScale = 0f;

        // 3. Mostrar el Mouse (para poder dar clic)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 4. Mostrar cuántas monedas juntaste
        if (UIManager.Instance != null)
        {
            // Obtenemos las monedas del UIManager
            int monedasFinales = UIManager.Instance.totalCoins;
            coinsText.text = ": " + monedasFinales;
            //coinsText.text = "Monedas Recogidas: " + monedasFinales;
        }
    }

    // --- FUNCIONES PARA LOS BOTONES ---

    public void RepetirNivel()
    {
        Time.timeScale = 1f; // Despausar antes de cargar
        // Carga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrSiguienteNivel()
    {
        Time.timeScale = 1f;
        // Aquí puedes poner una validación por si no existe el nivel 2 aún
        SceneManager.LoadScene(nextLevelName);
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}