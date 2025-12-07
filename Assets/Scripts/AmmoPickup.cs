using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int arrowAmount = 5;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooting shooter = other.GetComponent<PlayerShooting>();
            
            if (shooter != null)
            {
                shooter.AddArrows(arrowAmount);

                Destroy(gameObject);
            }
        }
    }
}