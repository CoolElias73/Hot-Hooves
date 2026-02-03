using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject powerupPrefab; 
    public float spawnRangeX = 4f;   
    public float spawnRangeY = 4f;   

    public float spawnInterval = 3f; 

    private void Start()
    {
       
        InvokeRepeating("SpawnPowerup", 1f, spawnInterval);
    }

    void SpawnPowerup()
    {
        // Random position within range
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        float y = Random.Range(-spawnRangeY, spawnRangeY);
        Vector2 spawnPosition = new Vector2(x, y);

        
        Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
    }
}

