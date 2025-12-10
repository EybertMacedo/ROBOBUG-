using UnityEngine;
using TMPro;

public class BugbotCodePanel : MonoBehaviour
{
    [Header("Referencias UI")]
    public TMP_InputField codeInput;   // Campo editable
    public BugbotMovementCardinalFixedDiagonal bugbot; // Referencia al bugbot
    public GameObject panelGroup; // Canvas del panel

    private bool isPanelOpen = false;

    void Start()
    {
        if (panelGroup != null)
            panelGroup.SetActive(false);

        // Suscribirse al evento onValueChanged para actualizar teclas dinámicamente
        if (codeInput != null && bugbot != null)
        {
            codeInput.onValueChanged.AddListener(OnCodeChanged);
            // También aplicar al inicio
            bugbot.ConfigureKeysFromText(codeInput.text);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TogglePanel();
        }
    }

    void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;

        if (panelGroup != null)
            panelGroup.SetActive(isPanelOpen);

        if (bugbot != null)
            bugbot.SetMovementEnabled(!isPanelOpen);
    }

    // Se llama cada vez que cambia el texto del InputField
    void OnCodeChanged(string newText)
    {
        if (bugbot != null)
        {
            bugbot.ConfigureKeysFromText(newText);
        }
    }

    // Método opcional para aplicar cambios con un botón “Run”
    public void ApplyCode()
    {
        if (bugbot != null && codeInput != null)
        {
            bugbot.ConfigureKeysFromText(codeInput.text);
        }
    }
}
