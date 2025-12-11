using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [Header("Configuración")]
    public float lifeTime = 5.0f; 
    
    [Tooltip("Valor de gravedad simulada (negativo para apuntar hacia abajo).")]
    public float simulatedGravity = -9.81f; 
    
    [HideInInspector]
    public bool SimulateGravity = true;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // CRÍTICO: Desactivar la gravedad en el Rigidbody para que la simulación sea manual
        rb.gravityScale = 0f; 
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        // Aplicar la simulación de gravedad directamente a la velocidad
        if (SimulateGravity)
        {
            // F = m * a
            // La aceleración (a) es la gravedad simulada
            
            // Usamos AddForce para interactuar correctamente con el motor de física,
            // o modificamos la velocidad directamente para un control absoluto.
            
            // Opción 1: Modificar la Velocidad (Control absoluto y limpio para proyectiles)
            rb.linearVelocity += Vector2.up * simulatedGravity * Time.fixedDeltaTime; 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitObject = collision.gameObject;
        
        if (hitObject.CompareTag("Player"))
        {
            Debug.Log("Proyectil impacta al JUGADOR!");
        }
        else if (hitObject.CompareTag("Enemy"))
        {
            Debug.Log("Fuego amigo detectado!");
        }
        
        // Destruir el proyectil al impactar
        Destroy(gameObject);
    }
}