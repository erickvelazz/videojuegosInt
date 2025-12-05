using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class BossFinalController : MonoBehaviour
{
    [Header("--- VIDA DEL JEFE ---")]
    public int maxLives = 5; 
    private int currentLives;
    private bool isInvulnerable = false;

    [Header("--- ATAQUE (BOLAS ROJAS) ---")]
    public GameObject projectilePrefab; 
    public Transform firePoint; 
    public float minTimeBetweenShots = 2f;
    public float maxTimeBetweenShots = 4f;
    private float shotTimer;

    [Header("--- MOVIMIENTO (Idéntico a EnemyPatrol) ---")]
    public float patrolSpeed = 3f;
    public Animator animator; 
    public Transform modelVisual; 
    
    private CharacterController controller;
    private int direction = 1; 
    private Vector3 gravityVector = new Vector3(0, -9.81f, 0); 

    public GateController gateToOpen;
    
    void Start()
    {
        currentLives = maxLives;
        controller = GetComponent<CharacterController>();
        
        shotTimer = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);

        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.SetFloat("MovementSpeed", patrolSpeed);

        // Asegurar orientación inicial correcta
        FlipModel();
    }

    void Update()
    {
        // 1. MOVIMIENTO (Copiado exacto de tu EnemyPatrol)
        Vector3 move = new Vector3(patrolSpeed * direction, 0, 0);

        if (controller.isGrounded)
        {
             gravityVector.y = -1f; 
        }

        controller.Move((move * Time.deltaTime) + (gravityVector * Time.deltaTime));

        // 2. DISPARO
        HandleShooting();
    }

    // --- LÓGICA DE GIRO (Copiada exacta de tu EnemyPatrol) ---
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("WallBoundary"))
        {
            direction *= -1;
            FlipModel();
        }
    }

    void FlipModel()
    {
        if (modelVisual != null)
        {
            // Usamos 0 y 180 tal como pediste en el ejemplo
            float targetRotationY = (direction == 1) ? 0f : 180f;
            modelVisual.localRotation = Quaternion.Euler(0, targetRotationY, 0);
        }
    }
    // -----------------------------------------------------------

    void HandleShooting()
    {
        shotTimer -= Time.deltaTime;

        if (shotTimer <= 0)
        {
            Shoot();
            shotTimer = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject ball = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            
            // *** ESTO EVITA QUE SALTE ***
            // Le dice al motor de físicas: "Ignora el choque entre YO (el jefe) y ESTA BOLA"
            if (ball.GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(controller, ball.GetComponent<Collider>());
            }

            // Disparar
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Dispara hacia donde apunta el firePoint (o hacia el frente según la dirección)
                Vector3 shootDir = new Vector3(direction, 0.2f, 0).normalized; 
                rb.AddForce(shootDir * 10f, ForceMode.Impulse); 
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float yDifference = other.transform.position.y - transform.position.y;
            float headHitThreshold = 1.2f; 

            if (yDifference > headHitThreshold)
            {
                // Jugador salta encima: Daño al jefe
                TakeDamage(); 
                
                // (Opcional) Aquí podrías empujar al jugador hacia arriba si tienes acceso a su script
            }
            else
            {
                // Choque de frente: Daño al jugador
                PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }
            }
        }
    }

    public void TakeDamage()
    {
        if (isInvulnerable) return;

        currentLives--;
        Debug.Log("Jefe herido. Vidas restantes: " + currentLives);

        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
            if(animator) animator.SetTrigger("Hit"); // Si tienes animación de dolor
        }
    }

    IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(1.0f); // 1 segundo invulnerable
        isInvulnerable = false;
    }

    void Die()
    {
        Debug.Log("¡Jefe Derrotado!");
        
        // NUEVO: Avisar a la puerta que se abra
        if (gateToOpen != null)
        {
            gateToOpen.OpenGate();
        }

        controller.enabled = false;
        if (animator != null) animator.SetFloat("MovementSpeed", 0f);
        
        Destroy(gameObject, 0.5f);
    }
}