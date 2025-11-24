using UnityEngine;

public class EnemyStompOrDamage : MonoBehaviour
{
    [Header("Daño al tocar al jugador")]
    public int damageAmount = 1;

    [Header("Detección de stomp")]
    public float bodyOffset = 0.1f;      // margen sobre la Y del enemigo
    public float minFallSpeed = -0.1f;   // basta con que vaya hacia abajo

    // Si tu vida máxima es 3, ponlo aquí
    public int maxPlayerHealth = 3;

    private bool isDead = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Enemy] OnTriggerEnter con {other.name}");

        if (isDead) return;
        if (!other.CompareTag("Player"))
            return;

        var movement = other.GetComponent<PlayerMovement>();
        var health   = other.GetComponent<PlayerHealth>();

        if (movement == null || health == null)
            return;

        float playerY = other.transform.position.y;
        float enemyY  = transform.position.y + bodyOffset;

        bool playerHigher  = playerY > enemyY;
        bool playerFalling = movement.VerticalVelocity <= minFallSpeed;

        bool fromAbove = playerHigher && playerFalling;

        Debug.Log($"[Enemy] playerHigher={playerHigher}, playerFalling={playerFalling}, fromAbove={fromAbove}");

        if (fromAbove)
        {
            Debug.Log("[Enemy] STOMP: matando enemigo");

            // 🔥 Reembolso de la vida que pudo haberse descontado
            health.currentHealth = Mathf.Min(
                health.currentHealth + damageAmount,
                maxPlayerHealth
            );

            movement.Bounce();
            isDead = true;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("[Enemy] GOLPE: haciendo daño al jugador");
            health.TakeDamage(damageAmount);
        }
    }
}
