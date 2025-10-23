using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void TitleScreen()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }

    /*
    public void QuitGame()
    {
        Application.Quit();
    }
    */
}
