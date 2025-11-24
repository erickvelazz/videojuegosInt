using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyPatrol : MonoBehaviour
{
    public float patrolSpeed = 3f;
    public Animator animator; 
    public Transform modelVisual; 
    
    private CharacterController controller;
    private int direction = 1; 
    private Vector3 gravityVector = new Vector3(0, -9.81f, 0); 
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        
        if (animator != null)
        {
            animator.SetFloat("MovementSpeed", patrolSpeed);
        }
    }

    void Update()
    {
        Vector3 move = new Vector3(patrolSpeed * direction, 0, 0);

        if (controller.isGrounded)
        {
             gravityVector.y = -1f; 
        }

        controller.Move((move * Time.deltaTime) + (gravityVector * Time.deltaTime));
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("WallBoundary"))
        {
            direction *= -1;
            FlipModel();
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }
    
    void Die()
    {
        controller.enabled = false;
        
        if (animator != null)
        {
            animator.SetFloat("MovementSpeed", 0f);
        }
        
        Destroy(gameObject);
    }

    void FlipModel()
    {
        if (modelVisual != null)
        {
            float targetRotationY = (direction == 1) ? 0f : 180f;
            modelVisual.localRotation = Quaternion.Euler(0, targetRotationY, 0);
        }
    }
}