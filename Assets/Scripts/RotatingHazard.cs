using UnityEngine;

public class RotatingHazard : MonoBehaviour
{
    [Header("Rotación (grados/segundo)")]
    public Vector3 rotationSpeed = new Vector3(0f, 0f, 360f);


    [Header("Movimiento vertical")]
    public float moveAmplitude = 1f; 
    public float moveSpeed = 1f;     

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);

        float offsetY = Mathf.Sin(Time.time * moveSpeed) * moveAmplitude;

        transform.position = new Vector3(
            startPos.x,
            startPos.y + offsetY,
            startPos.z
        );
    }
}
