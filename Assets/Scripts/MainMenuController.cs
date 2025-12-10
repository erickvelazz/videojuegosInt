using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string nombreEscenaNiveles = "LevelMenu";
    public string nombreEscenaCharacters = "CharactersScene";

    public void IrASeleccionDeNiveles()
    {
        SceneManager.LoadScene(nombreEscenaNiveles);
    }

    public void IrASeleccionDePersonajes()
    {
        SceneManager.LoadScene(nombreEscenaCharacters);
    }

    public void Salir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }
}