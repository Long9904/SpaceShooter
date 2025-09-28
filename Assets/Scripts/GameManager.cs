using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float worldSpeed;

    public float ScoreWin = 200f;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);

        }
       

    }
    private void Update()
    {
        UIController.Instance.UpdateScore(Time.deltaTime);
        if (UIController.Instance.score >= ScoreWin)
        {
            SceneController.Instance.NextLevel();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

    }

    public void Pause()
    {
        if (UIController.Instance.pausePanel.activeSelf)
        {
            Time.timeScale = 1f;
            UIController.Instance.pausePanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            UIController.Instance.pausePanel.SetActive(true);
            PlayerController.Instance.ExitBoost();
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnMainMenu()
    {
         Destroy(gameObject);
        SceneManager.LoadScene(0);
    }
}
