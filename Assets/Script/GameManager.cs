using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("Name of the game scene to load when starting the game.")]
    public string gameSceneName = "GameScene"; // Replace with your actual game scene name

    /// <summary>
    /// Starts the game by loading the game scene.
    /// This method should be called by the Start Game button.
    /// </summary>
    public void StartGame()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogWarning("Game scene name is not set. Please assign it in the Inspector.");
        }
    }

    /// <summary>
    /// Loads the next scene in the build settings.
    /// </summary>
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within the range of build settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more levels to load. This is the last scene in the build settings.");
        }
    }

    /// <summary>
    /// Quits the game. This works only in a built application.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
