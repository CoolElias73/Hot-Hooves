using UnityEngine;



public class PlatformGenerator : MonoBehaviour
{
    [Header("Platform")]
    public GameObject platformPrefab;

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

        Vector3 spawnPosition = new Vector3(randomX, currentY, 0f);
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
    }
}


