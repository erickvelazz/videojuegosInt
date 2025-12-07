using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerShooting : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject arrowPrefab; 
    public Transform firePoint;    
    public int currentArrows = 0;  
    
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentArrows > 0)
            {
                Shoot();
            }
            else
            {
                Debug.Log("Clic detectado (New Input), pero NO tienes flechas.");
            }
        }

        if(UIManager.Instance != null) 
            UIManager.Instance.UpdateArrows(currentArrows);
    }

    void Shoot()
    {
        if (arrowPrefab == null || firePoint == null) return;

        currentArrows--; 

        if(UIManager.Instance != null) 
            UIManager.Instance.UpdateArrows(currentArrows);

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, transform.rotation);
            
        Collider playerCollider = GetComponent<Collider>();
        Collider arrowCollider = arrow.GetComponent<Collider>();

        if (playerCollider != null && arrowCollider != null)
        {
            Physics.IgnoreCollision(arrowCollider, playerCollider);
        }
    }

    public void AddArrows(int amount)
    {
        currentArrows += amount;
        Debug.Log("Flechas recogidas. Total: " + currentArrows);

        if(UIManager.Instance != null) 
            UIManager.Instance.UpdateArrows(currentArrows);
    }
}