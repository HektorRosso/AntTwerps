using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public void LoadNextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int nextLevel = currentLevel + 1;

        if (Time.timeScale == 0)
            Time.timeScale = 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            Debug.Log("No more levels to load.");
            SceneManager.LoadScene(0);
        }
    }

    public void RestartLevel()
    {
        int previousLevel = SceneManager.GetActiveScene().buildIndex;

        if (Time.timeScale == 0)
            Time.timeScale = 1;
        SceneManager.LoadScene(previousLevel);
    }
}