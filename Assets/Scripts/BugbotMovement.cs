using UnityEngine;

public class BugbotMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f;

    [Header("Movimiento activo")]
    public bool canMove = true;
    
    [Header("Teclas fijas")]
    public KeyCode moverArriba = KeyCode.W;
    public KeyCode moverAbajo = KeyCode.S;
    public KeyCode moverIzquierda = KeyCode.A;
    public KeyCode moverDerecha = KeyCode.D;

    [Header("Validador de área de movimiento")]
    public MovementAreaValidator areaValidator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        bool up = Input.GetKey(moverArriba);
        bool down = Input.GetKey(moverAbajo);
        bool left = Input.GetKey(moverIzquierda);
        bool right = Input.GetKey(moverDerecha);

        Vector2 finalMovement = Vector2.zero;

        if (up && right)
            finalMovement = AngleToVector(26f);
        else if (up && left)
            finalMovement = AngleToVector(154f);
        else if (down && right)
            finalMovement = AngleToVector(-26f);
        else if (down && left)
            finalMovement = AngleToVector(-154f);
        else if (up)
            finalMovement = Vector2.up;
        else if (down)
            finalMovement = Vector2.down;
        else if (right)
            finalMovement = Vector2.right;
        else if (left)
            finalMovement = Vector2.left;

        Vector2 desiredPosition = rb.position + finalMovement * moveSpeed * Time.fixedDeltaTime;

        if (areaValidator == null || areaValidator.IsPositionAllowed(desiredPosition))
            rb.MovePosition(desiredPosition);

        if (spriteRenderer != null)
        {
            if (finalMovement.x < 0)
                spriteRenderer.flipX = true;
            else if (finalMovement.x > 0)
                spriteRenderer.flipX = false;
        }
    }

    Vector2 AngleToVector(float angleDegrees)
    {
        float rad = angleDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        if (!enabled && rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.MovePosition(rb.position);
        }
    }
}
