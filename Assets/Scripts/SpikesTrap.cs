using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    [Header("Movimiento vertical (local)")]
    public float raiseHeight = 1.2f;      // cuánto suben los picos en Y
    public float raiseSpeed  = 2f;        // velocidad al subir  (más lento)
    public float lowerSpeed  = 4f;        // velocidad al bajar  (más rápido)

    [Header("Tiempos (variación aleatoria)")]
    public float minTimeDown = 0.5f;      // tiempo mínimo ocultos
    public float maxTimeDown = 1.5f;      // tiempo máximo ocultos
    public float minTimeUp   = 0.3f;      // tiempo mínimo arriba
    public float maxTimeUp   = 1.0f;      // tiempo máximo arriba

    [Header("Daño al jugador")]
    public int damageAmount = 1;          // cuánta vida quita

    private Vector3 downPos;
    private Vector3 upPos;

    private float stateTimer;
    private State state;

    private enum State
    {
        Down,       // ocultos
        GoingUp,    // subiendo
        Up,         // arriba
        GoingDown   // bajando
    }

    private void Start()
    {
        // Usamos la posición local de los picos como "abajo"
        downPos = transform.localPosition;
        upPos   = downPos + Vector3.up * raiseHeight;

        state = State.Down;
        stateTimer = Random.Range(minTimeDown, maxTimeDown);

        // Aseguramos que tenga un collider tipo trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        else
        {
            Debug.LogWarning("[SpikesTrap] No hay Collider en los picos. Agrega un BoxCollider.", this);
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Down:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    state = State.GoingUp;
                }
                break;

            case State.GoingUp:
                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition,
                    upPos,
                    raiseSpeed * Time.deltaTime
                );

                if (Vector3.Distance(transform.localPosition, upPos) < 0.01f)
                {
                    state = State.Up;
                    stateTimer = Random.Range(minTimeUp, maxTimeUp);
                }
                break;

            case State.Up:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    state = State.GoingDown;
                }
                break;

            case State.GoingDown:
                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition,
                    downPos,
                    lowerSpeed * Time.deltaTime
                );

                if (Vector3.Distance(transform.localPosition, downPos) < 0.01f)
                {
                    state = State.Down;
                    stateTimer = Random.Range(minTimeDown, maxTimeDown);
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Solo nos interesa el jugador
        if (!other.CompareTag("Player"))
            return;

        // Solo hacemos daño cuando los picos están arriba o subiendo
        if (state == State.GoingUp || state == State.Up)
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}
