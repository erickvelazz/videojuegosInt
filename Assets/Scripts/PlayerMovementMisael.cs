using UnityEngine;

public class PlayerMovementMisael : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    private float horizontalInput;

    [Header("Jump Settings")]
    public float jumpForce = 12f;        // Primer salto
    public float doubleJumpForce = 9f;   // Segundo salto más bajito
    public int maxJumps = 2;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private int jumpCount;
    private bool isGrounded;

    [Header("Better Jump Feel")]
    public float fallMultiplier = 2.2f;
    public float lowJumpMultiplier = 2.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (horizontalInput > 0.1f)
    transform.localScale = new Vector3(1, 1, 1);
else if (horizontalInput < -0.1f)
    transform.localScale = new Vector3(-1, 1, 1);

    
        horizontalInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Resetear los saltos cuando toca el piso
        if (isGrounded)
            jumpCount = 0;

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount == 0)       // Primer salto
                Jump(jumpForce);
            else if (jumpCount == 1)  // Doble salto
                Jump(doubleJumpForce);
        }

        BetterJumpPhysics();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontalInput * moveSpeed, rb.linearVelocity.y, 0f);
        rb.linearVelocity = movement;
    }

    void Jump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, 0f);
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        jumpCount++;
    }

    void BetterJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Caída más pesada
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            // Si sueltas el espacio, sube menos (salto corto)
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
