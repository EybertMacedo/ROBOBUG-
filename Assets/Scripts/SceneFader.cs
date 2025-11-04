using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    void Awake()
    {
        // Start fully opaque (black)
        fadeImage.canvasRenderer.SetAlpha(1f);
    }

    void Start()
    {
        // Fade in from black
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        fadeImage.CrossFadeAlpha(0f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }

    public IEnumerator FadeOut(string nextScene)
    {
        fadeImage.CrossFadeAlpha(1f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(nextScene);
    }
}
