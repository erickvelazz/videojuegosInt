using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class IgnorePlayerCollision : MonoBehaviour
{
    void Start()
    {
        var enemyCC = GetComponent<CharacterController>();
        var player = GameObject.FindGameObjectWithTag("Player");

        if (enemyCC == null || player == null) return;

        var playerCC = player.GetComponent<CharacterController>();
        if (playerCC == null) return;

        Physics.IgnoreCollision(enemyCC, playerCC);

        Debug.Log("[IgnorePlayerCollision] Ignorando colisión entre enemigo y jugador");
    }
}
