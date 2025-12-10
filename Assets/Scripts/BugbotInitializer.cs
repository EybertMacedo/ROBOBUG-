using UnityEngine;
using TMPro;

public class BugbotInitializer : MonoBehaviour
{
    void Start()
    {
        // Busca el InputField con la etiqueta "textpanel"
        GameObject inputObj = GameObject.FindGameObjectWithTag("textpanel");
        if (inputObj == null)
        {
            Debug.LogError("No se encontró ningún InputField con tag 'textpanel'");
            return;
        }

        TMP_InputField inputField = inputObj.GetComponent<TMP_InputField>();
        if (inputField == null)
        {
            Debug.LogError("El objeto tagueado 'textpanel' no tiene TMP_InputField");
            return;
        }

        // Busca el bugbot en la escena
        BugbotMovementCardinalFixedDiagonal bugbot = FindObjectOfType<BugbotMovementCardinalFixedDiagonal>();
        if (bugbot == null)
        {
            Debug.LogError("No se encontró ningún Bugbot en la escena");
            return;
        }

        // Configura las teclas del bugbot usando el pseudocódigo del InputField
        bugbot.ConfigureKeysFromText(inputField.text);
    }
}
