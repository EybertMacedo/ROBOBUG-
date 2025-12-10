using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DepthZone : MonoBehaviour
{
    public int defaultOrder = 0;  // Default sorting order
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Example tag: "trigger_z_5"
        if (collision.tag.StartsWith("trigger_z_"))
        {
            // Extract number after "trigger_z_"
            string[] parts = collision.tag.Split('_');
            if (parts.Length >= 3 && int.TryParse(parts[2], out int newOrder))
            {
                spriteRenderer.sortingOrder = newOrder;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.StartsWith("trigger_z_"))
        {
            spriteRenderer.sortingOrder = defaultOrder;
        }
    }
}
