using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UICanvasMenu : MonoBehaviour
{
    public TMP_Text highScoreText;
    private int highScore;
    public GameObject settingsPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickButtonEvent(string sceneName)
    {
        AudioManagement.instance.PlayButtonClick();
        SceneManager.LoadScene(sceneName);
    }

    public void OnClickSettingsButton()
    {
        AudioManagement.instance.PlayButtonClick();
        settingsPanel.SetActive(true);
    }

}
