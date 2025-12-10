using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWaypointPatrol : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    private int currentWaypoint = 0;

    [Header("Movimiento activo")]
    public bool isMoving = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!isMoving || waypoints.Length == 0) return;

        Vector2 target = waypoints[currentWaypoint].position;
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (spriteRenderer != null)
            spriteRenderer.flipX = target.x > rb.position.x ? false : true;

        if (Vector2.Distance(newPos, target) < 0.05f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
                currentWaypoint = 0;
        }
    }

    public void SetMoving(bool enabled)
    {
        isMoving = enabled;
        if (!enabled && rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    // Detectar choque con jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Reinicia la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
