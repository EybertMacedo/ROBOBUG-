using UnityEngine;

public class MovementAreaValidator : MonoBehaviour
{
    [Header("Tags de validación")]
    [Tooltip("Tag de las zonas donde el jugador puede moverse.")]
    public string allowedTag = "MovementArea";

    [Tooltip("Tag de las zonas donde el jugador NO puede moverse.")]
    public string exclusionTag = "ExclusionZone";

    /// <summary>
    /// Devuelve true si la posición deseada está dentro de un área válida.
    /// </summary>
    public bool IsPositionAllowed(Vector2 targetPosition)
    {
        // Verifica todos los colliders 2D en el punto destino.
        Collider2D[] hits = Physics2D.OverlapPointAll(targetPosition);

        bool insideAllowed = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag(exclusionTag))
            {
                // Si el punto cae dentro de una zona de exclusión → prohibido
                return false;
            }
            else if (hit.CompareTag(allowedTag))
            {
                // Si cae dentro de una zona válida → marcar permitido
                insideAllowed = true;
            }
        }

        // Solo permitido si está dentro de al menos una zona válida y ninguna de exclusión
        return insideAllowed;
    }
}
