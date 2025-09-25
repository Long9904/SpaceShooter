using UnityEngine;

public class EnemyHaviour : MonoBehaviour
{
    [SerializeField] private GameObject lazePrefab;
    [SerializeField] private Transform firerTran;
    [SerializeField] private float timeShooter = 1f;
    private float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeShooter)
        {
            Instantiate(lazePrefab, transform.position, Quaternion.identity);
            timer = 0f; 
        }
    }
}
