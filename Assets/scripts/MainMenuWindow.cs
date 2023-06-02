using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuWindow : MonoBehaviour
{
    public void LoadNextScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        int currentScene = scene.buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
