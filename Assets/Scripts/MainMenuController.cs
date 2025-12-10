using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string nombreEscenaNiveles = "LevelMenu";

    public void IrASeleccionDeNiveles()
    {
        SceneManager.LoadScene(nombreEscenaNiveles);
    }

    public void Salir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }
}