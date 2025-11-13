using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float gravity = -25f;
    public float rotationSpeed = 10f;
    public float jumpHeight = 1.8f;
    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity; 
    private Vector2 moveInput;
    private bool isGrounded; // Para guardar el estado

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
            {
                animator.SetTrigger("Jump"); // Llama al Trigger "Jump"
            }
        }
    }

    void Update()
    {
        // --- 1. CHEQUEO DE SUELO ---
        isGrounded = controller.isGrounded;



        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // --- 2. MOVIMIENTO HORIZONTAL ---
        Vector3 moveDirection = new Vector3(moveInput.x, 0, 0); 
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // --- 3. ROTACIÓN ---
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        // --- 4. GRAVEDAD Y SALTO ---
        velocity.y += gravity * Time.deltaTime; 
        controller.Move(velocity * Time.deltaTime); 

        // --- 5. ACTUALIZAR ANIMATOR ---
        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("MovementSpeed", moveDirection.magnitude);
        }
    }
}