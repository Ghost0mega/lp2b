using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject fruitPrefab;
    public int spawnCount = 0;
    float spawnCountDown = 1.0f;
    public static bool isSpawning = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawnCountDown -= Time.deltaTime; // Decrease countdown each frame
        if (fruitPrefab != null && spawnCountDown <= 0.0f && isSpawning)
        {
            SpawnFruit();
            spawnCountDown = Random.Range(0f, 4f); // Reset countdown
        }
    }

    private void SpawnFruit()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-7.0f, 7.0f), 8.0f, 0);
        Quaternion spawnRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        GameObject newFruit = Instantiate(fruitPrefab);
        newFruit.transform.position = spawnPosition;
        newFruit.transform.rotation = spawnRotation;
        spawnCount++;

        // Debug.Log("Fruits spawned: " + spawnCount);
    }
    
    public static void ToggleSpawning(bool isRunning)
    {
        isSpawning = isRunning;
        if (!isRunning)
        {
            // GameObject[] fruits = GameObject.FindGameObjectsWithTag("Collectible");
            // foreach (GameObject fruit in fruits)
            // {
            //     Destroy(fruit); // Destroy all existing fruits when spawning is stopped
            // }
            // Debug.Log("Spawning stopped, all fruits destroyed.");

            // /!\ FIND FUNCTION IS PROHIBITED /!\
        }
        else
        {
            Debug.Log("Spawning resumed.");
        }
    }
}
