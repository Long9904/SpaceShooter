using UnityEngine;
using UnityEngine.SceneManagement;

public class BossGameManager : MonoBehaviour
{
    public static BossGameManager Instance;

    public float worldSpeed;

    public float ScoreWin = 200f;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (GameManager.Instance != null)
            {
                Destroy(GameManager.Instance.gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

        }
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

    }

    public void Pause()
    {
        if (BossUIController.Instance.pausePanel.activeSelf)
        {
            Time.timeScale = 1f;
            BossUIController.Instance.pausePanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            BossUIController.Instance.pausePanel.SetActive(true);
            BossPlayerController.Instance.ExitBoost();
        }
    }

    public void ReturnMainMenu()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
