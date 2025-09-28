using TMPro;
using UnityEngine;

public class UIEndGame : MonoBehaviour
{
    public static UIEndGame Instance;

    public float score { get; set; }
    public GameObject gameOverPanel;
    public TMP_Text endScore;
    public TMP_Text highScoreText;
    private int highScore;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
        highScore = PlayerPrefs.GetInt("HighScore", 0);

    }

    public void ShowGameOver()
    {
        // Use the score from UIController or BossUIController
        if (UIController.Instance != null)
        {
            score = UIController.Instance.score;
        }
        else if (BossUIController.Instance != null)
        {
            score = BossUIController.Instance.score;
        }

        endScore.text = "Score : " + Mathf.RoundToInt(score);
        if (score > highScore)
        {
            highScore = Mathf.RoundToInt(score);
            PlayerPrefs.SetInt("HighScore", highScore); // save high score
            PlayerPrefs.Save();
            highScoreText.text = "New High Score: " + highScore;
        }
        else
        {
            highScoreText.text = "High Score: " + highScore;
        }
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

}
