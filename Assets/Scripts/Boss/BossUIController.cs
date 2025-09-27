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
}
