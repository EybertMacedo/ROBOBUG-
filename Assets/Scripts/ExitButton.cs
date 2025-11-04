using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");

    #if UNITY_EDITOR
        // Stop Play Mode in the Editor
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        // Quit in the built game
        Application.Quit();
    #endif
    }
}
