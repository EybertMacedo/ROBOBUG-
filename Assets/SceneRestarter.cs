using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para gestionar escenas

public class SceneRestarter : MonoBehaviour
{
    // Este método se llama en cada frame
    void Update()
    {
        // Verificar si la tecla 'R' ha sido presionada
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartCurrentScene();
        }
    }

    // Función que carga la escena actualmente activa
    void RestartCurrentScene()
    {
        // 1. Obtener el índice de la escena actualmente activa
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 2. Cargar la escena usando su índice. Esto reinicia el nivel.
        SceneManager.LoadScene(currentSceneIndex);
        
        // Nota: Asegúrate de que las escenas estén añadidas al Build Settings.
    }
}