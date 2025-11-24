using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Daño al tocar al jugador")]
    public int damageAmount = 1;

    [Header("Referencia para ignorar stomp")]
    [Tooltip("Transform de Rogue_Head")]
    public Transform headTransform;

    public float minHeightOffset = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (headTransform != null)
        {
            float playerY = other.transform.position.y;
            float headY   = headTransform.position.y + minHeightOffset;

            if (playerY > headY)
            {
                return;
            }
        }

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }
}
