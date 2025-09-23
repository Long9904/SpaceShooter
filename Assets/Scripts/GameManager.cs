using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float worldSpeed;

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
}
