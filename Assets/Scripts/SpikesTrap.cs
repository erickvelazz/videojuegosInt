using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    [Header("Movimiento vertical (local)")]
    public float raiseHeight = 1.2f;     
    public float raiseSpeed  = 2f;        
    public float lowerSpeed  = 4f;        

    [Header("Tiempos (variación aleatoria)")]
    public float minTimeDown = 0.5f;      
    public float maxTimeDown = 1.5f;      
    public float minTimeUp   = 0.3f;      
    public float maxTimeUp   = 1.0f;      

    [Header("Daño al jugador")]
    public int damageAmount = 1;          

    private Vector3 downPos;
    private Vector3 upPos;

    private float stateTimer;
    private State state;

    private enum State
    {
        Down,       
        GoingUp,    
        Up,         
        GoingDown   
    }

    private void Start()
    {
        downPos = transform.localPosition;
        upPos   = downPos + Vector3.up * raiseHeight;

        state = State.Down;
        stateTimer = Random.Range(minTimeDown, maxTimeDown);

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
        if (!other.CompareTag("Player"))
            return;

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
