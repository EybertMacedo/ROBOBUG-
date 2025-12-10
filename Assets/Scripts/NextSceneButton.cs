using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextSceneButton : MonoBehaviour
{
    [Header("Nombre de la escena a cargar")]
    public string sceneName;

    void Start()
    {
        // Obtener el Button del mismo GameObject
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnButtonPressed);
        }
        else
        {
            Debug.LogWarning("No se encontró un Button en este GameObject.");
        }
    }

    void OnButtonPressed()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No se ha definido el nombre de la escena.");
        }
    }
}
