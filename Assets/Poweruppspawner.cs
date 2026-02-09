using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject powerupPrefab;
    public GameObject[] powerupPrefabs;
    public Transform spawnCenter;
    public float spawnRangeX = 4f;   
    public float spawnRangeY = 4f;   

    public float spawnInterval = 3f; 

    private void Start()
    {
        if (spawnCenter == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                spawnCenter = player.transform;
        }
       
        InvokeRepeating("SpawnPowerup", 1f, spawnInterval);
    }

    void SpawnPowerup()
    {
        GameObject prefab = GetRandomPowerupPrefab();
        if (prefab == null)
            return;

        Vector2 origin = spawnCenter != null ? (Vector2)spawnCenter.position : (Vector2)transform.position;

        // Random position within range
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        float y = Random.Range(-spawnRangeY, spawnRangeY);
        Vector2 spawnPosition = origin + new Vector2(x, y);

        
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    GameObject GetRandomPowerupPrefab()
    {
        if (powerupPrefabs != null && powerupPrefabs.Length > 0)
            return powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];

        return powerupPrefab;
    }
}

