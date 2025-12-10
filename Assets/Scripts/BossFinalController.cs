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

    [Header("--- COMPORTAMIENTO (SALTO) ---")]
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    public float minTimeBetweenJumps = 3f;
    public float maxTimeBetweenJumps = 6f;
    private float jumpTimer;
    
    private Vector3 velocity;

    [Header("--- VISUAL ---")]
    public Animator animator; 
    public Transform modelVisual; 
    
    private CharacterController controller;
    public GateController gateToOpen;
    private Transform playerTransform; 
    
    [Header("--- ATAQUE ESPECIAL (CARGA) ---")]
    [SerializeField] private float chargeDuration = 1.5f; // Tiempo que tarda en cargar el ataque
    [SerializeField] private float chargeSpeed = 15f; // Velocidad del movimiento de carga
    [SerializeField] private float chargeCooldown = 8f; // Tiempo entre ataques de carga
    private float currentChargeCooldown;
    private bool isCharging = false;
    private bool isTelegraphing = false; // Indica si está mostrando la animación/efecto de carga
    
    void Start()
    {
        currentLives = maxLives;
        controller = GetComponent<CharacterController>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
        
        shotTimer = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        jumpTimer = Random.Range(minTimeBetweenJumps, maxTimeBetweenJumps);

        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.SetFloat("MovementSpeed", 0f);

        currentChargeCooldown = chargeCooldown; // Inicializar cooldown
    }

    void Update()
    {
        // Si está en medio de un ataque especial (carga), no ejecutar movimiento normal/salto
        if (isTelegraphing || isCharging)
        {
            return;
        }

        // Reducir Cooldown del Ataque de Carga
        currentChargeCooldown -= Time.deltaTime;
        if (currentChargeCooldown <= 0)
        {
            StartCoroutine(ChargeAttackSequence());
            currentChargeCooldown = chargeCooldown; // Reiniciar Cooldown
            return; // Salir de Update para no procesar movimiento en este frame
        }
        bool isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0 && isGrounded)
        {
            PerformJump();
            jumpTimer = Random.Range(minTimeBetweenJumps, maxTimeBetweenJumps);
        }

        velocity.y += gravity * Time.deltaTime;
        
        controller.Move(velocity * Time.deltaTime);

        FacePlayer();

        HandleShooting();
    }

    void PerformJump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void FacePlayer()
    {
        if (playerTransform != null && modelVisual != null)
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            
            if (directionToPlayer.x > 0)
            {
                modelVisual.localRotation = Quaternion.Euler(0, 0f, 0); 
            }
            else
            {
                modelVisual.localRotation = Quaternion.Euler(0, 180f, 0);
            }
        }
    }

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
            
            if (ball.GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(controller, ball.GetComponent<Collider>());
            }

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 shootDir = modelVisual.forward + new Vector3(0, 0.2f, 0); 
                rb.AddForce(shootDir.normalized * 10f, ForceMode.Impulse); 
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
                TakeDamage(); 
            }
            else
            {
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
            if(animator) animator.SetTrigger("Hit"); 
        }
    }

    IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(1.0f); 
        isInvulnerable = false;
    }

    void Die()
    {
        Debug.Log("¡Jefe Derrotado!");
        
        if (gateToOpen != null)
        {
            gateToOpen.OpenGate();
        }

        controller.enabled = false;
        if (animator != null) animator.SetFloat("MovementSpeed", 0f);
        
    
        Destroy(gameObject, 0.5f);
    }

    private IEnumerator ChargeAttackSequence()
    {
        // 1. Telegrafía (Advertencia)
        isTelegraphing = true;
        // Opcional: Cambiar color o animar para advertir
        if (modelVisual != null) 
        {
            // Pequeña sacudida o cambio visual podría ir aquí
            Debug.Log("¡Jefe preparando carga!");
        }

        yield return new WaitForSeconds(chargeDuration); 

        // 2. Carga Rápida
        isTelegraphing = false;
        isCharging = true;

        float direction = 1f;
        // Cargar hacia donde está mirando el modelo visual (que mira al player)
        // Check localRotationY mainly.
        // Si modelVisual.localRotation.y es 180 (aprox), mira a izquierda (-1)
        // Si es 0, mira a derecha (1)
        if (Mathf.Abs(modelVisual.localEulerAngles.y - 180f) < 10f) 
        {
             direction = -1f;
        }
        else
        {
             direction = 1f;
        }

        // Aplicar movimiento continuo durante la carga
        float chargeTime = 0.5f;
        float elapsed = 0f;

        while(elapsed < chargeTime)
        {
            if(controller.enabled)
            {
                 // Mover horizontalmente rápido, manteniendo gravedad o Y actual
                 // Para CharacterController simple:
                 Vector3 chargeMove = new Vector3(direction * chargeSpeed, velocity.y, 0);
                 controller.Move(chargeMove * Time.deltaTime);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 3. Recuperación
        isCharging = false;
        velocity.x = 0; // Detener impulso horizontal
    }
}
