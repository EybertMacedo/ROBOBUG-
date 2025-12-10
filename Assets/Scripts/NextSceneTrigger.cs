using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneTrigger : MonoBehaviour
{
    [Header("Nombre de la escena a cargar")]
    public string sceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Solo cambiar de escena si es el Bugbot
        if (other.CompareTag("Player"))  // asegúrate de que tu bugbot tenga tag "Player"
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning("No se ha definido el nombre de la escena en NextSceneTrigger.");
            }
        }
    }
}
