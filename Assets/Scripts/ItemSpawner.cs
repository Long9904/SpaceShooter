using Unity.Hierarchy;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private float minY = -4.5f;
    [SerializeField] private float maxY = 4.5f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float timeSpawn = 2f;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timeSpawn)
        {
            SpawnItem();
            timer = 0f;
        }
    }

    void SpawnItem()
    {
        int randomIndex = Random.Range(0, itemPrefabs.Length);
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0f);
        Collider2D hit = Physics2D.OverlapCircle(spawnPosition, 0.5f);
        if (hit != null)
        {            
            return;
        }
        GameObject item = Instantiate(itemPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        Destroy(item, lifetime);
    }
}
