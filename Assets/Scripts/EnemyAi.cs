using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAi : MonoBehaviour
{
    [Header("Targets")]
    public Transform playerTarget;
    public Transform[] waypoints;

    [Header("Settings")]
    public float detectionRadius = 5f;
    // agent speed is now controlled by the navmesh agent component

    private int currentWaypoint = 0;
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // disable automatic rotation for 2d usage
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (playerTarget == null) return;

        // check distance to player
        float distance = Vector2.Distance(transform.position, playerTarget.position);

        if (distance < detectionRadius)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        UpdateSpriteOrientation();
    }

    void ChasePlayer()
    {
        agent.SetDestination(playerTarget.position);
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        // cycle waypoint if close to destination
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }

        agent.SetDestination(waypoints[currentWaypoint].position);
    }

    void UpdateSpriteOrientation()
    {
        // flip sprite based on horizontal velocity
        if (Mathf.Abs(agent.velocity.x) > 0.1f)
        {
            spriteRenderer.flipX = agent.velocity.x < 0;
        }
    }

    // collision with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}