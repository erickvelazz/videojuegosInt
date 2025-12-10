using UnityEngine;

public class SurfaceDetector : MonoBehaviour
{
    public Transform[] groundChecks; // Tus 4 rayos
    public float rayDistance = 0.3f; // Distancia del rayito

    public string currentSurface = "Normal"; // Resultado final para saber en qué piso estás

    void Update()
    {
        DetectSurface();
    }

    void DetectSurface()
    {
        currentSurface = "Normal"; // Valor por defecto

        foreach (Transform check in groundChecks)
        {
            RaycastHit2D hit = Physics2D.Raycast(check.position, Vector2.down, rayDistance);

            if (hit.collider != null)
            {
                // Si el piso tiene un tag, lo tomamos
                currentSurface = hit.collider.tag;
                break; // Ya encontramos superficie, salimos
            }
        }
    }
}
