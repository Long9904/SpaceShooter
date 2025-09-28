using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private Slider energySlider;
    [SerializeField] private TMP_Text energyText;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private Slider gasSlider;
    [SerializeField] private TMP_Text gasText;

    [SerializeField] private TMP_Text scoreText;
    public float score { get; set; }
    public GameObject pausePanel;

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
    public void UpdateGasSlider(float current, float max)
    {
        gasSlider.maxValue = max;
        gasSlider.value = Mathf.RoundToInt(current);
        gasText.text = gasSlider.value + "/" + gasSlider.maxValue;
    }
    public void UpdateScore(float amount)
    {
        score += amount;
        scoreText.text = "Score : " + Mathf.RoundToInt(score);
    }
}
