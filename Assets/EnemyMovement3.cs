using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement3 : MonoBehaviour
{
    // --- VARIABLES ACCESIBLES POR EL PANEL DE CÓDIGO ---
    
    [Header("Comportamiento de Bandada (Boids)")]
    public float perceptionRadius = 3.0f;
    public float separationDistance = 1.0f; 
    
    // Pesos de las reglas de Boids
    public float separationWeight = 3.0f;
    public float cohesionWeight = 1.0f;
    public float alignmentWeight = 1.0f;
    public float targetWeight = 2.0f; // Fuerza para ir hacia el jugador
    
    // Velocidad: Aceleración suave
    public float maxSpeed = 5.0f;
    public float accelerationRate = 0.5f; 

    // --- VARIABLES INTERNAS ---
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform; 
    
    private Vector2 currentVelocity;
    private float currentSpeed;
    private List<EnemyMovement3> neighbors; 
    private static List<EnemyMovement3> allBoids;

    // Métodos públicos para el panel de código
    
    public void SetSeparationDistance(float distance)
    {
        if (distance > 0) { separationDistance = distance; }
    }
    
    public void SetMaxSpeed(float speed)
    {
        if (speed >= 0) { maxSpeed = speed; }
    }
    
    public void SetSeparationWeight(float weight)
    {
        if (weight >= 0) { separationWeight = weight; }
    }
    
    public void SetCohesionWeight(float weight)
    {
        if (weight >= 0) { cohesionWeight = weight; }
    }
    
    public void SetAlignmentWeight(float weight)
    {
        if (weight >= 0) { alignmentWeight = weight; }
    }
    
    public void SetTargetWeight(float weight)
    {
        if (weight >= 0) { targetWeight = weight; }
    }
    
    // Métodos de compatibilidad con el lector de código
    public void SetChaseSpeed(float speed) { maxSpeed = speed; }
    public void SetDetectionRange(float range) { /* No usado */ }
    public void SetMovementEnabled(bool enabled) { /* Se mantiene para compatibilidad */ }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false; 
        rb.linearDamping = 0.5f; 
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (allBoids == null)
        {
            allBoids = new List<EnemyMovement3>();
        }
        allBoids.Add(this); 

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        currentVelocity = Vector2.zero;
        currentSpeed = 0f;
    }

    void OnDestroy()
    {
        if (allBoids != null)
        {
            allBoids.Remove(this);
        }
    }

    void FixedUpdate()
    {
        // 1. Aceleración Suave
        if (currentSpeed < maxSpeed)
        {
            currentSpeed = Mathf.Min(maxSpeed, currentSpeed + accelerationRate * Time.fixedDeltaTime);
        }

        // 2. Encontrar Vecinos
        neighbors = GetNeighbors();
        
        // 3. Calcular Fuerzas de Boids y Persecución
        Vector2 separation = CalculateSeparation() * separationWeight;
        Vector2 alignment = CalculateAlignment() * alignmentWeight;
        Vector2 cohesion = CalculateCohesion() * cohesionWeight;
        Vector2 targetForce = CalculateTargetForce() * targetWeight;

        // 4. Fuerza Total
        Vector2 totalForce = separation + alignment + cohesion + targetForce;

        // 5. Aplicar la Fuerza y Limitar la Velocidad
        currentVelocity += totalForce * Time.fixedDeltaTime;
        currentVelocity = Vector2.ClampMagnitude(currentVelocity, currentSpeed);

        // 6. Aplicar el movimiento
        rb.linearVelocity = currentVelocity; 

        // 7. Orientación del Asset (FlipX)
        if (spriteRenderer != null && currentVelocity.magnitude > 0.01f)
        {
            if (currentVelocity.x < 0)
                spriteRenderer.flipX = true;
            else if (currentVelocity.x > 0)
                spriteRenderer.flipX = false;
        }
    }

    // --- MÉTODOS DE BOIDS ---

    List<EnemyMovement3> GetNeighbors()
    {
        List<EnemyMovement3> nearby = new List<EnemyMovement3>();
        
        foreach (EnemyMovement3 other in allBoids)
        {
            if (other == this) continue;
            
            float dist = Vector2.Distance(rb.position, other.rb.position);
            
            if (dist <= perceptionRadius)
            {
                nearby.Add(other);
            }
        }
        return nearby;
    }

    // Regla 1: Separación
    Vector2 CalculateSeparation()
    {
        Vector2 steering = Vector2.zero;
        int separationCount = 0;

        foreach (EnemyMovement3 neighbor in neighbors)
        {
            Vector2 neighborPos = neighbor.rb.position;
            float dist = Vector2.Distance(rb.position, neighborPos);

            if (dist < separationDistance)
            {
                Vector2 diff = rb.position - neighborPos; 
                steering += diff.normalized / dist; 
                separationCount++;
            }
        }

        if (separationCount > 0)
        {
            steering /= separationCount;
        }
        return steering.normalized * maxSpeed;
    }

    // Regla 2: Cohesión
    Vector2 CalculateCohesion()
    {
        if (neighbors.Count == 0) return Vector2.zero;
        
        Vector2 centerOfMass = Vector2.zero;
        
        foreach (EnemyMovement3 neighbor in neighbors)
        {
            centerOfMass += neighbor.rb.position;
        }
        
        centerOfMass /= neighbors.Count;
        
        Vector2 direction = centerOfMass - rb.position;
        return direction.normalized * maxSpeed;
    }

    // Regla 3: Alineación
    Vector2 CalculateAlignment()
    {
        if (neighbors.Count == 0) return Vector2.zero;

        Vector2 averageVelocity = Vector2.zero;
        
        foreach (EnemyMovement3 neighbor in neighbors)
        {
            averageVelocity += neighbor.currentVelocity;
        }
        
        averageVelocity /= neighbors.Count;
        
        return averageVelocity.normalized * maxSpeed;
    }

    // Regla 4: Ir hacia el jugador
    Vector2 CalculateTargetForce()
    {
        if (playerTransform == null) return Vector2.zero;
        
        Vector2 direction = (Vector2)playerTransform.position - rb.position;
        return direction.normalized * maxSpeed;
    }
}