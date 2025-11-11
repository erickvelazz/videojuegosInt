using UnityEngine;
using UnityEngine.InputSystem; // Asegúrate de tener esta línea

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f; // Velocidad para girar al personaje

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 moveInput;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Esta función la llama tu "PlayerInput"
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        // --- 1. Calcular la Dirección del Movimiento (EL GRAN CAMBIO) ---
        
        // Esta es la nueva línea clave. 
        // Usamos los ejes del *mundo*, no los del personaje.
        // moveInput.x (A/D) controla el eje X del mundo.
        // moveInput.y (W/S) controla el eje Z del mundo.
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Aplicamos el movimiento
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // --- 2. Rotar el Personaje ---
        
        // Si el jugador se está moviendo (la dirección no es cero)
        if (moveDirection != Vector3.zero)
        {
            // Calculamos la rotación que necesitamos para "mirar" en esa dirección
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            
            // Rotamos suavemente al personaje hacia esa dirección
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // --- 3. Simulación de Gravedad (Igual que antes) ---
        
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}