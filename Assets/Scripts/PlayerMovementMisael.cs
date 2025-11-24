using UnityEngine;

public class PlayerMovementMisael : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public int maxJumps = 2;
    private int jumpCount;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpCount = maxJumps;
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(x * moveSpeed, rb.linearVelocity.y, 0);
        rb.linearVelocity = move;
    }

    void Jump()
    {
        if (isGrounded) jumpCount = maxJumps;

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount--;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
