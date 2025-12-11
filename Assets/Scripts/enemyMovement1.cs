using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement1 : MonoBehaviour
{
    // Variables accesibles por el panel de código
    public float wanderSpeed = 2.0f;
    public float chaseSpeed = 3.5f;

    [Header("Comportamiento de IA")]
    public float detectionRange = 7.0f; 
    public float destinationTolerance = 0.2f; 
    public float turnSmoothness = 3.0f;

    [Header("Vagabundeo")]
    public float wanderDistanceMax = 5.0f;
    
    // Bandera de control de movimiento expuesta para el panel
    private bool isMovementEnabled = true;

    // Estados de la IA
    private enum State { Wander, Chase }
    private State currentState = State.Wander;

    // Referencias
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform; 
    private Collider2D wanderAreaCollider; // EnemyMovementArea
    private Collider2D extendedAreaCollider; // MovementArea

    private Vector2 currentDestination;
    private Vector2 currentVelocity; 

    // Métodos públicos para el panel de código
    
    public void SetChaseSpeed(float speed)
    {
        if (speed >= 0) { chaseSpeed = speed; }
    }

    public void SetDetectionRange(float range)
    {
        if (range > 0) { detectionRange = range; }
    }
    
    public void SetMovementEnabled(bool enabled)
    {
        isMovementEnabled = enabled;
        if (!enabled && rb != null)
        {
            currentVelocity = Vector2.zero;
            rb.linearVelocity = Vector2.zero; 
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; 
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject wanderAreaObject = GameObject.FindWithTag("EnemyMovementArea");
        if (wanderAreaObject != null)
        {
            wanderAreaCollider = wanderAreaObject.GetComponent<PolygonCollider2D>();
        }
        
        GameObject extendedAreaObject = GameObject.FindWithTag("MovementArea");
        if (extendedAreaObject != null)
        {
            extendedAreaCollider = extendedAreaObject.GetComponent<PolygonCollider2D>();
        }

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        currentDestination = rb.position;
        currentVelocity = Vector2.zero;
        GetNewWanderDestination();
    }

    void FixedUpdate()
    {
        if (!isMovementEnabled) return;

        // Verificar si el jugador está en rango y cambiar de estado
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(rb.position, playerTransform.position);

            if (distanceToPlayer <= detectionRange)
            {
                currentState = State.Chase;
            }
            else if (currentState == State.Chase)
            {
                currentState = State.Wander;
                GetNewWanderDestination(); 
            }
        }

        switch (currentState)
        {
            case State.Wander:
                HandleWander();
                break;
            case State.Chase:
                HandleChase();
                break;
        }

        // ------------------
        // Aplicación del Movimiento
        // ------------------
        float currentSpeed = (currentState == State.Chase) ? chaseSpeed : wanderSpeed;
        
        // CORRECCIÓN: Detención Inmediata si la velocidad objetivo es cero
        if (currentSpeed <= 0.01f)
        {
            currentVelocity = Vector2.zero; 
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (currentDestination - rb.position).normalized;
        Vector2 desiredVelocity = direction * currentSpeed;
        
        // Suavizar el giro
        currentVelocity = Vector2.Lerp(currentVelocity, desiredVelocity, Time.fixedDeltaTime * turnSmoothness);

        // Aplicar la posición
        Vector2 newPosition = rb.position + currentVelocity * Time.fixedDeltaTime;
        
        // VALIDACIÓN DE LÍMITES CONDICIONAL
        Collider2D currentBoundary = (currentState == State.Chase) ? extendedAreaCollider : wanderAreaCollider;

        if (currentBoundary != null && !currentBoundary.OverlapPoint(newPosition))
        {
            if (currentState == State.Wander) GetNewWanderDestination();
            return;
        }

        rb.MovePosition(newPosition);

        // Orientación del Asset (FlipX)
        if (spriteRenderer != null)
        {
            if (currentVelocity.x < -0.01f)
                spriteRenderer.flipX = true;
            else if (currentVelocity.x > 0.01f)
                spriteRenderer.flipX = false;
        }
    }

    void HandleWander()
    {
        if (Vector2.Distance(rb.position, currentDestination) <= destinationTolerance)
        {
            GetNewWanderDestination();
        }
    }

    void HandleChase()
    {
        if (playerTransform != null)
        {
            currentDestination = playerTransform.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Se asegura de que no estamos completamente detenidos antes de reflejar
        if (currentVelocity.magnitude < 0.01f) return; 

        Vector2 reflectDirection = Vector2.Reflect(currentVelocity.normalized, collision.contacts[0].normal).normalized;
        
        float currentSpeed = (currentState == State.Chase) ? chaseSpeed : wanderSpeed;
        currentVelocity = reflectDirection * currentSpeed;
        
        if (currentState == State.Wander)
        {
            currentDestination = GetValidRandomPointInWanderArea();
        }
    }

    void GetNewWanderDestination()
    {
        Vector2 targetPosition;
        int attempts = 0;
        const int maxAttempts = 10;
        
        currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, Time.deltaTime * 5f);
        Collider2D currentWanderBoundary = wanderAreaCollider;

        do
        {
            Vector2 randomOffset = Random.insideUnitCircle * wanderDistanceMax;
            targetPosition = rb.position + randomOffset;
            attempts++;
            
        } while (currentWanderBoundary != null && !currentWanderBoundary.OverlapPoint(targetPosition) && attempts < maxAttempts);

        if (attempts < maxAttempts)
        {
            currentDestination = targetPosition;
        }
        else
        {
            currentDestination = GetValidRandomPointInWanderArea();
        }
    }
    
    Vector2 GetValidRandomPointInWanderArea()
    {
        if (wanderAreaCollider == null) return rb.position;
        
        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(wanderAreaCollider.bounds.min.x, wanderAreaCollider.bounds.max.x);
            float y = Random.Range(wanderAreaCollider.bounds.min.y, wanderAreaCollider.bounds.max.y);
            Vector2 point = new Vector2(x, y);
            
            if (wanderAreaCollider.OverlapPoint(point))
            {
                return point;
            }
        }
        return rb.position;
    }
}