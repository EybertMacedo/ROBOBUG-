using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyWaypointPatrol : MonoBehaviour
{
    [Header("Targets")]
    public Transform playerTarget;
    public Transform[] waypoints;

    [Header("Settings")]
    public float detectionRadius = 5f;
    public bool enableDebugging = true;

    private int currentWaypoint = 0;
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // force 2d configuration
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Start()
    {
        // critical check: is agent actually on the mesh?
        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"[NAV ERROR] {gameObject.name} is not placed on a NavMesh. Bake the NavMesh or move the object to Z=0.");
            enabled = false; // stop script to prevent spam
        }
    }

    void Update()
    {
        if (enableDebugging) RunDiagnostics();

        if (playerTarget != null && Vector2.Distance(transform.position, playerTarget.position) < detectionRadius)
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
        if (playerTarget == null) return;
        agent.SetDestination(playerTarget.position);
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        // check if we reached waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

        agent.SetDestination(waypoints[currentWaypoint].position);
    }

    void UpdateSpriteOrientation()
    {
        if (Mathf.Abs(agent.velocity.x) > 0.1f)
        {
            spriteRenderer.flipX = agent.velocity.x < 0;
        }
    }

    // --- debugging tools ---
    
    void RunDiagnostics()
    {
        // check 1: is the agent stopped via code?
        if (agent.isStopped) 
            Debug.LogWarning($"[NAV DEBUG] Agent is stopped via isStopped flag.");

        // check 2: is speed zero?
        if (agent.speed == 0) 
            Debug.LogWarning($"[NAV DEBUG] Agent speed is 0 in inspector.");

        // check 3: is path invalid?
        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
            Debug.LogWarning($"[NAV DEBUG] Agent cannot find path to target (Unreachable).");

        // check 4: is target null?
        if (waypoints.Length == 0 && playerTarget == null)
            Debug.LogWarning($"[NAV DEBUG] No targets assigned in Inspector.");
    }

    void OnDrawGizmos()
    {
        if (!enableDebugging) return;

        // draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (agent == null) return;

        // draw calculated path
        if (agent.hasPath)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
            }
        }
        else
        {
            // draw line to intended destination if path failed
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, agent.destination);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}