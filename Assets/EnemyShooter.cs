using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Configuración del Proyectil")]
    public GameObject projectilePrefab;
    public float launchVelocity = 10f; 
    public float launchAngleDegrees = 45f; 
    public float fireRate = 0.2f;

    [Header("Referencias")]
    private Transform playerTransform;
    private float nextFireTime;
    private EnemyMovement1 enemyMovementScript; 

    void Awake()
    {
        enemyMovementScript = GetComponent<EnemyMovement1>();
        if (enemyMovementScript != null)
        {
            // Deshabilita el movimiento del enemigo (lo convierte en torreta)
            enemyMovementScript.SetMovementEnabled(false); 
        }

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        
        nextFireTime = Time.time + fireRate;
    }

    void FixedUpdate()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("ERROR: Asigna el Prefab del Proyectil.");
            return;
        }
        
        if (playerTransform == null) return;

        if (Time.time >= nextFireTime)
        {
            LaunchProjectileAtPlayer();
            nextFireTime = Time.time + fireRate;
        }
    }

    void LaunchProjectileAtPlayer()
    {
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = playerTransform.position;
        
        // La instanciación ocurre correctamente.
        GameObject projectileGO = Instantiate(projectilePrefab, startPosition, Quaternion.identity);
        Rigidbody2D rbProjectile = projectileGO.GetComponent<Rigidbody2D>();
        
        if (rbProjectile == null)
        {
            // Este error ya se maneja en el Prefab, pero es una doble verificación.
            Debug.LogError("Error: El proyectil instanciado no tiene Rigidbody2D.");
            Destroy(projectileGO); 
            return;
        }
        
        // 1. Cálculo del Vector de Velocidad Inicial
        Vector2 displacement = targetPosition - startPosition;
        float angleRad = launchAngleDegrees * Mathf.Deg2Rad;
        
        float v0x = launchVelocity * Mathf.Cos(angleRad);
        float v0y = launchVelocity * Mathf.Sin(angleRad);

        if (displacement.x < 0)
        {
            v0x = -v0x;
        }
        
        // 2. Aplicar Velocidad
        rbProjectile.linearVelocity = new Vector2(v0x, v0y);
        
        // 3. Asegurar que la simulación de gravedad se active
        Projectile projectileScript = projectileGO.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SimulateGravity = true;
        }
    }
}