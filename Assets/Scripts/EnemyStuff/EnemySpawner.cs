using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;
    private List<Transform> spawnPositions;

    [SerializeField] private float spawnRate;
    private float spawnTimer;

    [SerializeField] private int spawnsUntilChange;
    private int spawnCount;
    [SerializeField] private float changeRate;
    [SerializeField] private float minRate;

    private int randomPosition;
    private int randomEnemy;

    private void Start()
    {
        //Creates new List for spawnPositions
        spawnPositions = new List<Transform>();

        //Adds to the List the Transform of every child GameObject in parent
        foreach (Transform child in transform)
        {
            spawnPositions.Add(child);
        }
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
                    spawnPositions[randomPosition]);

        //spawnCount += 1
        spawnCount++;

        //The function stops if spawnCount doesn't reach the value of spawnsUntilChange
        if (spawnCount < spawnsUntilChange)
            return;

        //When spawnCount reaches spawnsUntilChange
        //sets spawnCount back to zero and continues the function
        spawnCount = 0;

        //Calls ChangeRate function
        ChangeRate();
    }

    private void ChangeRate()
    {
        //If spawnRate is lower or equal to minRate, the function stops
        if (spawnRate < minRate)
            return;

        //Lowers spawnRate based on changeRate value
        spawnRate -= changeRate;

        //If spawnRate goes lower than minRate, sets spawnRate to minRate
        if (spawnRate < minRate)
            spawnRate = minRate;
    }
}
