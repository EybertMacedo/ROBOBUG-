using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyController : MonoBehaviour
{
    [Header("Waypoints para patrullaje")]
    public Transform[] waypoints;
    public float speed = 3f;
    public bool isMoving = true;

    private int currentWaypoint = 0;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Configurar valores iniciales desde el panel
        ApplyPanelConfig();
    }

    void Update()
    {
        if (!isMoving || waypoints.Length == 0) return;

        MoveAlongWaypoints();
    }

    void MoveAlongWaypoints()
    {
        Transform target = waypoints[currentWaypoint];
        Vector2 direction = (target.position - transform.position).normalized;
        Vector2 newPos = Vector2.MoveTowards(rb.position, target.position, speed * Time.deltaTime);

        rb.MovePosition(newPos);

        // Flip visual según dirección
        if (direction.x > 0)
            spriteRenderer.flipX = false;  // mirando derecha
        else if (direction.x < 0)
            spriteRenderer.flipX = true;   // mirando izquierda

        // Llegó al waypoint actual
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Reinicia el nivel
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // ------------------------------
    // Lectura de pseudocódigo desde InputField
    // ------------------------------
    public void ApplyPanelConfig()
    {
        GameObject inputObj = GameObject.FindGameObjectWithTag("textpanel");
        if (inputObj == null) return;

        TMP_InputField inputField = inputObj.GetComponent<TMP_InputField>();
        if (inputField == null) return;

        string text = inputField.text;

        speed = GetFloatFromCode(text, "setSpeed", speed);
        isMoving = GetBoolFromCode(text, "setIsMoving", isMoving);
    }

    float GetFloatFromCode(string text, string action, float defaultValue)
    {
        string pattern = action + " = ";
        int index = text.IndexOf(pattern);
        if (index == -1) return defaultValue;

        int start = index + pattern.Length;
        int end = text.IndexOf('\n', start);
        if (end == -1) end = text.Length;

        string valueStr = text.Substring(start, end - start).Trim();
        if (float.TryParse(valueStr, out float result))
            return result;
        return defaultValue;
    }

    bool GetBoolFromCode(string text, string action, bool defaultValue)
    {
        string pattern = action + " = ";
        int index = text.IndexOf(pattern);
        if (index == -1) return defaultValue;

        int start = index + pattern.Length;
        int end = text.IndexOf('\n', start);
        if (end == -1) end = text.Length;

        string valueStr = text.Substring(start, end - start).Trim().ToLower();
        return valueStr == "true";
    }
}
