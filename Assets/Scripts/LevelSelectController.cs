using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class LevelSelectController : MonoBehaviour
{
    public string nivel1 = "Level1Scene"; 
    public string nivel2 = "Level2Scene";
    
    public Button btnNivel2;
    public Button btnNivel3;

    void Start()
    {
        if(btnNivel2 != null) btnNivel2.interactable = true;
        if(btnNivel3 != null) btnNivel3.interactable = false;
    }

    public void CargarNivel1()
    {
        Debug.Log("¡Botón presionado! Intentando cargar: " + nivel1); 
        SceneManager.LoadScene(nivel1);
    }
    
    public void CargarNivel2()
    {
        SceneManager.LoadScene(nivel2);
    }
    
    public void VolverAlInicio()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
