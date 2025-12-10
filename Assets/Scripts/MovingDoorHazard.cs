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
        Vector3 startPos = transform.position;
        bottomPos = startPos + Vector3.up * bottomOffset;
        topPos    = startPos + Vector3.up * topOffset;

        transform.position = bottomPos;
        goingUp = true;
        targetPos = topPos;
        PickNewSpeed();

        BoxCollider col = GetComponent<BoxCollider>();
        col.isTrigger = true;
    }

    void Update()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer > 0f)
                return;

            isWaiting = false;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            currentSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            goingUp = !goingUp;
            targetPos = goingUp ? topPos : bottomPos;

            PickNewSpeed();
            waitTimer = Random.Range(minPauseTime, maxPauseTime);
            isWaiting = waitTimer > 0f;
        }
    }

    void PickNewSpeed()
    {
        if (goingUp)
            currentSpeed = Random.Range(minUpSpeed, maxUpSpeed);   
        else
            currentSpeed = Random.Range(minDownSpeed, maxDownSpeed); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }
}
