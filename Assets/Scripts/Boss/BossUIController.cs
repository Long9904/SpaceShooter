using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUIController : MonoBehaviour
{
    public static BossUIController Instance;

    [SerializeField] private Slider energySlider;
    [SerializeField] private TMP_Text energyText;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] private TMP_Text bossHealthText;

    [SerializeField] private TMP_Text scoreText;
    public GameObject pausePanel;

    public float score { get; set; }

    void Awake()
    {
        // take score from previous scene
        score = BossGameManager.Instance.score;
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        scoreText.text = "Score : " + Mathf.RoundToInt(score);
        AudioManagement.instance.PlayBackgroundMusic();
    }

    public void UpdateEnergySlider(float current, float max)
    {
        energySlider.maxValue = max;
        energySlider.value = Mathf.RoundToInt(current);
        energyText.text = energySlider.value + "/" + energySlider.maxValue;
    }

    public void UpdateHealthSlider(float current, float max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = Mathf.RoundToInt(current);
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;
    }
   
    public void UpdateBossHealthSlider(float current, float max)
    {
        bossHealthSlider.maxValue = max;
        bossHealthSlider.value = Mathf.RoundToInt(current);
        bossHealthText.text = bossHealthSlider.value + "/" + bossHealthSlider.maxValue;
    }

    public void AddScore(float value)
    {
        score += value;
        scoreText.text = "Score : " + Mathf.RoundToInt(score);
    }
}
