using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] zombiePrefabs;
    [Min(1)] public int maxZombies = 20;
    [Min(0.1f)] public float spawnRate = 0.5f;
    [Min(1)] public int zombiesPerWave = 20;
    [Min(0f)] public float minimumSpawnDistance = 18f;
    [Min(0f)] public float maximumSpawnDistance = 30f;
    [Min(0.1f)] public float navMeshSampleDistance = 5f;
    [Min(1)] public int spawnPositionAttempts = 30;

    [Header("Wave Settings")]
    public bool enableWaves = true;
    public int currentWave = 1;
    [Min(0f)] public float timeBetweenWaves = 8f;

    [Header("Difficulty")]
    [Min(0f)] public float walkSpeedIncreasePerWave = 0.15f;
    [Min(0f)] public float chaseSpeedIncreasePerWave = 0.25f;

    float nextSpawnTime;
    float nextWaveStartTime;
    int zombiesAlive;
    int zombiesSpawnedThisWave;
    int zombiesToSpawnThisWave;
    bool waveInProgress;

    void Start()
    {
        if (!HasSpawnConfiguration())
        {
            Debug.LogError("ZombieSpawner needs at least one zombie prefab and a Player tagged Player.", this);
            enabled = false;
            return;
        }

        if (minimumSpawnDistance >= maximumSpawnDistance)
        {
            Debug.LogError("Maximum spawn distance must be greater than minimum spawn distance.", this);
            enabled = false;
            return;
        }

        if (!enableWaves)
            return;

        currentWave = Mathf.Clamp(currentWave, 1, GameManager.TotalWaves);
        nextWaveStartTime = Time.time;
    }

    void Update()
    {
        GameManager gameManager = GameManager.Instance;
        if (Time.timeScale <= 0f || (gameManager != null && !gameManager.isGameActive))
            return;

        if (enableWaves)
            WaveSpawn();
        else
            ContinuousSpawn();
    }

    bool HasSpawnConfiguration()
    {
        if (zombiePrefabs == null || zombiePrefabs.Length == 0)
            return false;

        bool hasPrefab = false;
        foreach (GameObject prefab in zombiePrefabs)
        {
            if (prefab != null)
            {
                hasPrefab = true;
                break;
            }
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        return hasPrefab && playerObject != null;
    }

    void ContinuousSpawn()
    {
        zombiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (zombiesAlive < maxZombies && Time.time >= nextSpawnTime)
        {
            SpawnZombie();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void WaveSpawn()
    {
        if (!waveInProgress)
        {
            if (Time.time >= nextWaveStartTime)
                StartWave();
            return;
        }

        zombiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (zombiesSpawnedThisWave < zombiesToSpawnThisWave &&
            zombiesAlive < maxZombies &&
            Time.time >= nextSpawnTime)
        {
            if (SpawnZombie())
            {
                zombiesSpawnedThisWave++;
            }
            nextSpawnTime = Time.time + spawnRate;
        }

        zombiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (zombiesSpawnedThisWave >= zombiesToSpawnThisWave && zombiesAlive == 0)
            EndWave();
    }

    void StartWave()
    {
        if (currentWave > GameManager.TotalWaves)
            return;

        waveInProgress = true;
        zombiesSpawnedThisWave = 0;
        zombiesToSpawnThisWave = zombiesPerWave;
        nextSpawnTime = Time.time;
        Debug.Log("Wave " + currentWave + " started. Zombies: " + zombiesToSpawnThisWave);

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
            gameManager.UpdateWave(currentWave);
    }

    void EndWave()
    {
        waveInProgress = false;
        if (currentWave >= GameManager.TotalWaves)
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager != null)
                gameManager.CompleteGame();
            else
                Debug.LogError("All waves completed, but no GameManager exists.", this);
            enabled = false;
            return;
        }

        Debug.Log("Wave completed! Next wave in " + timeBetweenWaves + " seconds");
        currentWave++;
        nextWaveStartTime = Time.time + timeBetweenWaves;
    }

    bool SpawnZombie()
    {
        GameObject zombiePrefab = GetRandomZombiePrefab();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (zombiePrefab == null || playerObject == null)
        {
            Debug.LogError("ZombieSpawner could not find a valid zombie prefab or Player.", this);
            return false;
        }

        if (!TryGetSpawnPosition(playerObject.transform.position, out Vector3 spawnPosition))
        {
            Debug.LogWarning("Could not find a valid NavMesh spawn position around the Player. Check the baked NavMesh and spawn distances.", this);
            return false;
        }

        GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        ZombieAI zombieAI = zombie.GetComponent<ZombieAI>();
        if (zombieAI != null)
        {
            float waveDifficulty = currentWave - 1;
            zombieAI.walkSpeed += walkSpeedIncreasePerWave * waveDifficulty;
            zombieAI.chaseSpeed += chaseSpeedIncreasePerWave * waveDifficulty;
        }

        return true;
    }

    GameObject GetRandomZombiePrefab()
    {
        int startIndex = Random.Range(0, zombiePrefabs.Length);
        for (int offset = 0; offset < zombiePrefabs.Length; offset++)
        {
            GameObject prefab = zombiePrefabs[(startIndex + offset) % zombiePrefabs.Length];
            if (prefab != null)
                return prefab;
        }

        return null;
    }

    bool TryGetSpawnPosition(Vector3 playerPosition, out Vector3 spawnPosition)
    {
        for (int attempt = 0; attempt < spawnPositionAttempts; attempt++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float distance = Random.Range(minimumSpawnDistance, maximumSpawnDistance);
            Vector3 candidate = playerPosition + new Vector3(
                Mathf.Cos(angle) * distance,
                0f,
                Mathf.Sin(angle) * distance);

            if (!NavMesh.SamplePosition(candidate, out NavMeshHit hit, navMeshSampleDistance, NavMesh.AllAreas))
                continue;

            float horizontalDistance = Vector2.Distance(
                new Vector2(playerPosition.x, playerPosition.z),
                new Vector2(hit.position.x, hit.position.z));
            if (horizontalDistance < minimumSpawnDistance || horizontalDistance > maximumSpawnDistance)
                continue;

            spawnPosition = hit.position;
            return true;
        }

        spawnPosition = Vector3.zero;
        return false;
    }
}
