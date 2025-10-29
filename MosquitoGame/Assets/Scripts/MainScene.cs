using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string sceneName = "NextScene";
    [SerializeField] private bool waitForSound = true;

    private bool isTransitioning = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isTransitioning) return;

        if (Input.GetMouseButtonDown(0))
        {
            isTransitioning = true;

            if (audioSource != null)
            {
                if (waitForSound && audioSource.clip != null)
                {
                    audioSource.Play();
                    StartCoroutine(WaitAndLoad());
                }
                else
                {
                    audioSource.Play();
                    SceneManager.LoadScene(sceneName);
                }
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    private System.Collections.IEnumerator WaitAndLoad()
    {
        while (audioSource != null && audioSource.isPlaying)
            yield return null;

        SceneManager.LoadScene(sceneName);
    }
}
