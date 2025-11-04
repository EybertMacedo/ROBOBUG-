using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideoController : MonoBehaviour
{
    public string nextScene = "MainMenu"; // Cambia por el nombre de tu escena de menú
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Carga el video por ruta absoluta
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = "";

        // (Opcional) desactiva el audio si no lo necesitas
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;

        // Cuando el video termine, cambiar de escena
        videoPlayer.loopPointReached += OnVideoEnd;

        // Reproduce
        videoPlayer.Play();
    }

    void Update()
    {
        // Permitir saltar la intro con una tecla (Espacio o Escape)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            LoadNextScene();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
