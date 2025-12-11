using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement2 : MonoBehaviour
{
    // --- VARIABLES DE LANZAMIENTO (Ajustadas para la simulación de proyectiles) ---
    [Header("Configuración de Proyectil")]
    [Tooltip("Tiempo total en segundos para alcanzar el destino (1.0s por requisito).")]
    public float flightDuration = 1.0f; 
    
    [Tooltip("Factor para aumentar la altura del arco (simulación de fuerza/ángulo).")]
    public float arcHeightFactor = 0.5f;

    [Tooltip("Gravedad simulada aplicada manualmente.")]
    public float simulatedGravity = -9.81f; 
    
    // --- VARIABLES INTERNAS ---
    private Rigidbody2D rb;
    private Transform playerTransform; 
    
    // Variables de trayectoria
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float startTime;
    private float totalDistanceX;
    private float peakHeight; 

    // Métodos públicos para el panel de código (Necesarios para que el panel no dé error, pero sin uso aquí)
    public void SetChaseSpeed(float speed) { /* No usado */ }
    public void SetDetectionRange(float range) { /* No usado */ }
    public void SetMovementEnabled(bool enabled) { /* No usado */ }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Desactivar la gravedad en el Rigidbody para que la simulación sea manual
        rb.gravityScale = 0f; 
        
        // Buscar al jugador solo al instanciar
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            targetPosition = playerTransform.position;
        }
        
        // Inicializar la trayectoria
        startPosition = rb.position;
        startTime = Time.time;
        
        // Calcular los parámetros de la trayectoria
        totalDistanceX = targetPosition.x - startPosition.x;
        
        // Calcular la altura máxima necesaria para el arco
        // Usamos una fórmula heurística simple: distancia horizontal * factor
        peakHeight = Mathf.Abs(totalDistanceX) * arcHeightFactor;
        
        // El proyectil se destruye si no choca
        Destroy(gameObject, flightDuration + 0.5f);
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;
        
        float timeElapsed = Time.time - startTime;
        
        // 1. Terminar el movimiento si el tiempo ha expirado
        if (timeElapsed >= flightDuration)
        {
            // Opcional: Destrucción al finalizar la trayectoria (se maneja en Destroy en Start)
            // Para simular el impacto, podríamos forzar una colisión o destrucción aquí.
            Destroy(gameObject);
            return;
        }

        // 2. Cálculo de la Posición Parábolica (Simulación de Gravedad)
        
        // Progresión horizontal (lineal en el tiempo)
        float x = Mathf.Lerp(startPosition.x, targetPosition.x, timeElapsed / flightDuration);
        
        // Progresión vertical (parabólica)
        // Usamos una parábola basada en la altura máxima (peakHeight) y el tiempo total (flightDuration)
        // La fórmula de la parábola: y = H * 4 * (t/T) * (1 - t/T)
        // Donde H es la altura máxima y t/T es el progreso normalizado (0 a 1).
        float normalizedTime = timeElapsed / flightDuration;
        float y = startPosition.y + targetPosition.y + peakHeight * 4 * normalizedTime * (1 - normalizedTime);
        
        // Aplicamos solo el arco si el objetivo está a la misma altura. 
        // Para tener en cuenta el desplazamiento vertical (startPosition.y a targetPosition.y):
        // Desplazamiento vertical lineal
        float y_linear = Mathf.Lerp(startPosition.y, targetPosition.y, normalizedTime);
        
        // Arco sobre el desplazamiento lineal:
        float y_arc = peakHeight * 4 * normalizedTime * (1 - normalizedTime);
        
        // Posición Y final
        float finalY = y_linear + y_arc;
        
        Vector2 newPosition = new Vector2(x, finalY);

        // 3. Aplicar el Movimiento
        rb.MovePosition(newPosition);

        // La orientación del asset no es necesaria para un proyectil
    }

    // El proyectil choca con otro objeto
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