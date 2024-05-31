using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteManager : MonoBehaviour
{
    public GameObject levelCompleteCanvas;
    public float delayBeforeTitleScreen = 3f; // Adjust the delay as needed

    void Start()
    {
        // Activate the level complete screen when the scene loads
        levelCompleteCanvas.SetActive(true);
        StartCoroutine(ReturnToTitleScreenAfterDelay());
    }

    private IEnumerator ReturnToTitleScreenAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeTitleScreen);
        SceneManager.LoadScene("TitleScreen"); // Replace with your title screen scene name
    }
}
