using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] private List<GameObject> enemyPrefabs;
    private List<Transform> spawnPositions;
    
    //Spawn rate and health change variables
    [SerializeField] private float spawnRate;
    private float spawnTimer;
    private int spawnCount;
    [SerializeField] private float changeSpawnRate;
    [SerializeField] private float minSpawnRate;
    [SerializeField] private int changeHealthEvery;
    public int healthMult;

    private int randomPosition;
    private int randomEnemy;

    //Singleton initialization
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //Creates new List for spawnPositions
        spawnPositions = new List<Transform>();

        //Adds to the List the Transform of every child GameObject in parent
        foreach (Transform child in transform)
        {
            spawnPositions.Add(child);
        }

        //Sets healthMult to 1
        healthMult = 1;
    }

    private void Update()
    {
        //Every frame adds a value to spawnTimer equivalent to 1f every second
        spawnTimer += Time.deltaTime;

        //If spawnTimer doesn't reach the value of spawnRate, the function stops
        if (spawnTimer <= spawnRate)
            return;

        //When spawnTimer reaches spawnRate value
        //sets spawnTimer back to zero and continues the function
        spawnTimer = 0;
        
        //Calls the function to spawn an enemy
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        //Generates a random number to get the element for enemy type and spawn position
        randomPosition = Random.Range(0, spawnPositions.Count);
        randomEnemy = Random.Range(0, enemyPrefabs.Count);

        //Creates enemy GameObject based on the random values generated
        Instantiate(enemyPrefabs[randomEnemy], 
                    spawnPositions[randomPosition].position, 
                    Quaternion.identity, 
                    transform);

        //spawnCount += 1
        spawnCount++;

        //Everytime spawnCount reaches a multiplier of 10 and is not already at minSpawnRate
        if (spawnCount % 10 == 0 && spawnRate != minSpawnRate)
        {
            //Highers the rate at which enemies spawn
            ChangeRate();
        }

        //Everytime spawnCount reaches a multiplier of changeHealthEvery
        if (spawnCount % changeHealthEvery == 0)
        {
            //Changes the health amount enemies spawn with
            healthMult++;
        }
    }

    private void ChangeRate()
    {
        //If spawnRate equal to minSpawnRate, the function stops
        if (spawnRate == minSpawnRate)
            return;

        //Lowers spawnRate based on changeRate value
        spawnRate -= changeSpawnRate;

        //If spawnRate goes lower than minSpawnRate, sets spawnRate to minSpawnRate
        if (spawnRate < minSpawnRate)
            spawnRate = minSpawnRate;
    }
}
