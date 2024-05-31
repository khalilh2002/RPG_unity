using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    // Function to load the main game scene
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // Replace with your main game scene name
    }

    // Function to quit the game
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    
}
