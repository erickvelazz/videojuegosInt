using UnityEngine;

public class EnemyHeadStomp : MonoBehaviour
{
    [Header("Referencia al enemigo raíz (GameObject Rogue)")]
    public GameObject enemyRoot;

    public float extraBounceHeight = 0f;

    public float minFallSpeed = -1.0f;

    private bool isDead = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (!other.CompareTag("Player"))
            return;

        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        if (movement == null)
            return;

        if (other.transform.position.y <= transform.position.y)
        {
            return;
        }

        if (movement.VerticalVelocity > minFallSpeed)
        {
            return;
        }

        if (extraBounceHeight > 0f)
        {
            movement.stompBounceHeight = extraBounceHeight;
        }

        movement.Bounce();

        isDead = true;

        if (enemyRoot != null)
        {
            Destroy(enemyRoot);
        }
        else
        {
            Destroy(transform.root.gameObject);
        }
    }
}
