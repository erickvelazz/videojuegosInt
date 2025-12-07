using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HazardDamage : MonoBehaviour
{
    [Header("Daño al tocar al jugador")]
    public int damageAmount = 1;

    void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
