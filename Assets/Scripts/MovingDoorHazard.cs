using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MovingDoorHazard : MonoBehaviour
{
    [Header("Recorrido vertical (offset respecto a la posición inicial)")]
    public float bottomOffset = 0f;   // normalmente 0 (punto más bajo)
    public float topOffset    = 3f;   // hasta dónde sube (en unidades de Y)

    [Header("Velocidades (con variación)")]
    public float minUpSpeed   = 0.5f; // subir lento
    public float maxUpSpeed   = 1.5f;
    public float minDownSpeed = 3f;   // bajar rápido
    public float maxDownSpeed = 6f;

    [Header("Pausas en los extremos")]
    public float minPauseTime = 0.1f;
    public float maxPauseTime = 0.6f;

    [Header("Daño al jugador")]
    public int damageAmount = 1;      // cuánta vida resta (tu PlayerHealth ya maneja invulnerabilidad)

    private Vector3 bottomPos;
    private Vector3 topPos;
    private Vector3 targetPos;

    private float currentSpeed;
    private float waitTimer;
    private bool isWaiting = false;
    private bool goingUp = true;

    void Start()
    {
        // Calculamos posiciones de extremos según la posición inicial
        Vector3 startPos = transform.position;
        bottomPos = startPos + Vector3.up * bottomOffset;
        topPos    = startPos + Vector3.up * topOffset;

        // Colocamos la puerta abajo al inicio
        transform.position = bottomPos;
        goingUp = true;
        targetPos = topPos;
        PickNewSpeed();

        // Aseguramos que el collider sea trigger (para no empujar al jugador)
        BoxCollider col = GetComponent<BoxCollider>();
        col.isTrigger = true;
    }

    void Update()
    {
        // Si está en pausa en un extremo
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer > 0f)
                return;

            isWaiting = false;
        }

        // Movimiento hacia el objetivo actual
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            currentSpeed * Time.deltaTime
        );

        // ¿Llegó al extremo?
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            // Cambiar dirección
            goingUp = !goingUp;
            targetPos = goingUp ? topPos : bottomPos;

            // Nueva velocidad y pausa aleatoria
            PickNewSpeed();
            waitTimer = Random.Range(minPauseTime, maxPauseTime);
            isWaiting = waitTimer > 0f;
        }
    }

    void PickNewSpeed()
    {
        if (goingUp)
            currentSpeed = Random.Range(minUpSpeed, maxUpSpeed);   // lento
        else
            currentSpeed = Random.Range(minDownSpeed, maxDownSpeed); // rápido
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si no es el jugador, ignorar
        if (!other.CompareTag("Player"))
            return;

        // Tu PlayerHealth ya gestiona invulnerabilidad y reinicio
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }
}
