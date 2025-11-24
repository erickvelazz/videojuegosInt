using UnityEngine;

public class MovingArrow : MonoBehaviour
{
    [Header("Movimiento vertical")]
    public float amplitude = 2f;
    public float minSpeed = 1f;
    public float maxSpeed = 4f;

    [Header("Pausas en los extremos")]
    public float minPauseTime = 0.1f;
    public float maxPauseTime = 0.7f;

    [Header("Rotación en los extremos (EJE X)")]
    public float angleTop = 90f;
    public float angleBottom = -90f;

    private Vector3 startPos;
    private Vector3 topPos;
    private Vector3 bottomPos;
    private Vector3 targetPos;

    private float currentSpeed;
    private float waitTimer;
    private bool isWaiting = false;
    private bool goingUp = true; 

    void Start()
    {
        startPos  = transform.position;
        topPos    = startPos + Vector3.up * amplitude;
        bottomPos = startPos - Vector3.up * amplitude;

        goingUp = true;
        targetPos = topPos;
        PickNewSpeed();

        SetAngle(angleBottom);
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
            if (goingUp)
                SetAngle(angleTop);     
            else
                SetAngle(angleBottom);  

            goingUp = !goingUp;
            targetPos = goingUp ? topPos : bottomPos;

            PickNewSpeed();
            waitTimer = Random.Range(minPauseTime, maxPauseTime);
            isWaiting = true;
        }
    }

    void PickNewSpeed()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);
    }

    void SetAngle(float angle)
    {
        Vector3 euler = transform.eulerAngles;
        euler.x = angle;
        transform.eulerAngles = euler;
    }
}
