using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroPlayer : MonoBehaviour
{
    public string nextScene = "MainMenu";

    void Start()
    {
        var vp = GetComponent<VideoPlayer>();
        vp.playOnAwake = false;
        vp.source = VideoSource.Url;
        vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "intro_robobug.webm");
        vp.renderMode = VideoRenderMode.CameraFarPlane;
        vp.audioOutputMode = VideoAudioOutputMode.AudioSource;
        vp.SetTargetAudioSource(0, GetComponent<AudioSource>());
        vp.Prepare();
        vp.prepareCompleted += (player) => player.Play();
        vp.loopPointReached += (player) =>
            {
                FindObjectOfType<SceneFader>().StartCoroutine(
                    FindObjectOfType<SceneFader>().FadeOut(nextScene)
                );
            };

        vp.errorReceived += (player, msg) => Debug.LogError("Video error: " + msg);
    }
}
