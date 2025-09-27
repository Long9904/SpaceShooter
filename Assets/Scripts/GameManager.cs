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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

        }
    }
    private void Update()
    { 
        UIController.Instance.UpdateScore(Time.deltaTime);
        if(UIController.Instance.score >= ScoreWin)
        {
            SceneController.Instance.NextLevel();
        }
    }
}
