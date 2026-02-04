using UnityEngine;

public class platformGenerator : MonoBehaviour
{
    [Header("Platform Prefabs")]
    public GameObject platformA;
    public GameObject platformB;
    public GameObject platformC;

    [Header("Chance")]
    [Range(0f, 1f)]
    public float platformBChance = 0.3f; 
    [Range(0f, 1f)]
    public float platformCChance = 0.1f;    

    [Header("Horizontal Range")]
    public float minX = -2.5f;
    public float maxX = 2.5f;

    [Header("Vertical Spacing")]
    public float minYGap = 1.5f;
    public float maxYGap = 2.5f;

    [Header("Initial Spawn Count")]
    public int platformCount = 10;

    [Header("Player Reference")]
    public Transform player;
    public float spawnAheadDistance = 6f;

    private float currentY;

    void Start()
    {
        currentY = transform.position.y;

        for (int i = 0; i < platformCount; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        if (player != null && player.position.y + spawnAheadDistance > currentY)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        float randomX = Random.Range(minX, maxX);
        float randomGap = Random.Range(minYGap, maxYGap);

        currentY += randomGap;

        float roll = Random.value;
        GameObject platformToSpawn;

        
        if (roll < platformCChance)
        {
            platformToSpawn = platformC;
        }
        else if (roll < platformCChance + platformBChance)
        {
            platformToSpawn = platformB;
        }
        else
        {
            platformToSpawn = platformA;
        }

        Vector3 spawnPosition = new Vector3(randomX, currentY, 0f);
        Instantiate(platformToSpawn, spawnPosition, Quaternion.identity);
    }
}

