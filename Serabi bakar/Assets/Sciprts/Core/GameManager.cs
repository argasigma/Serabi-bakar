using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject pauseMenu;

    public GameObject gameOver;

    public static GameManager Instance;

    public GameState currentState;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
            {
                PauseGame();
            }
            else if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        currentState = GameState.Paused;
        pauseMenu.SetActive(true);
        gameOver.SetActive(false);
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        currentState = GameState.Playing;
        pauseMenu.SetActive(false);
        gameOver.SetActive(false);
        Debug.Log("Game Resumed");
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Over");
        currentState = GameState.GameOver;
        gameOver.SetActive(true);
        pauseMenu.SetActive(false); 
        }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
        Debug.Log("Game Restarted");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        Debug.Log("Returned to Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}