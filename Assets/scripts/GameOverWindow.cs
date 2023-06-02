using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    public Bird bird;
    public GameObject gameOverWindow;
    public Text scoreText_Window;
    public Text scoreText_Game;

    public bool flag = false;
    private void ShowGameWindow()
    {
        if(bird.isDead && !flag)
        {
            flag = true;
            scoreText_Window.text = scoreText_Game.text;
            gameOverWindow.SetActive(true);
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Start()
    {
        gameOverWindow.SetActive(false);
    }
    private void Update()
    {
        ShowGameWindow();
    }
}
