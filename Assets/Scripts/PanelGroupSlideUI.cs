using UnityEngine;

public class PanelGroupSlideUI : MonoBehaviour
{
    [Header("Main Panel Group")]
    public RectTransform panelGroup;      // The container (parent object with all elements)

    [Header("Positions")]
    public Vector2 hiddenPosition;        // Position when hidden
    public Vector2 visiblePosition;       // Position when visible

    [Header("Animation Settings")]
    public float slideSpeed = 5f;         // Speed of slide animation

    private bool isVisible = false;       // Current visibility
    private bool isSliding = false;       // Prevents overlapping animations

    void Start()
    {
        if (panelGroup != null)
            panelGroup.anchoredPosition = visiblePosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && !isSliding)
        {
            StartCoroutine(SlidePanelGroup(isVisible ? hiddenPosition : visiblePosition));
            isVisible = !isVisible;
        }
    }

    private System.Collections.IEnumerator SlidePanelGroup(Vector2 targetPos)
    {
        isSliding = true;

        while (Vector2.Distance(panelGroup.anchoredPosition, targetPos) > 0.1f)
        {
            panelGroup.anchoredPosition = Vector2.Lerp(
                panelGroup.anchoredPosition,
                targetPos,
                Time.deltaTime * slideSpeed
            );
            yield return null;
        }

        panelGroup.anchoredPosition = targetPos;
        isSliding = false;
    }
}
