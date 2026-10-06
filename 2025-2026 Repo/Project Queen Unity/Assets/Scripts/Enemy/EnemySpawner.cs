using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Baðýmlýlýklar")]
    [SerializeField] private EnemyCarPool carPool;

    [Header("Spawn Ayarlarý")]
    [SerializeField] private float spawnZPosition = 100f;
    [SerializeField] private float laneDistance = 3.5f;

    [Header("Dünya/Oyun Ayarlarý")]
    [SerializeField] private float baseWorldSpeed = 20f;

    [Header("Zorluk / Ýkili Spawn Ayarlarý")]
    [Tooltip("Ayný anda 2 araba gelme ihtimali (0 ile 1 arasý, örn: 0.3 = %30)")]
    [Range(0f, 1f)]
    [SerializeField] private float doubleSpawnChance = 0.3f;

    private float currentSpawnInterval = 1.5f;
    private float currentWorldSpeed;
    private float spawnTimer;
    private bool canSpawn = true;

    private readonly int[] lanes = { -1, 0, 1 };

    private void OnEnable()
    {
        PlayerCollision.OnGameOver += StopSpawning;
        DifficultyManager.OnDifficultyIncreased += UpdateDifficulty;
    }

    private void OnDisable()
    {
        PlayerCollision.OnGameOver -= StopSpawning;
        DifficultyManager.OnDifficultyIncreased -= UpdateDifficulty;
    }

    private void StopSpawning() => canSpawn = false;

    private void UpdateDifficulty(float speedMultiplier, float newSpawnInterval)
    {
        currentWorldSpeed = baseWorldSpeed * speedMultiplier;
        currentSpawnInterval = newSpawnInterval;
    }

    private void Awake()
    {
        currentWorldSpeed = baseWorldSpeed;
    }

    private void Update()
    {
        if (!canSpawn) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            HandleSpawning();
            spawnTimer = currentSpawnInterval;
        }
    }

    private void HandleSpawning()
    {
        if (Random.value <= doubleSpawnChance)
        {
            int lane1 = Random.Range(0, 3);
            int lane2;

            do
            {
                lane2 = Random.Range(0, 3);
            } while (lane1 == lane2);

            SpawnSingleCar(lanes[lane1]);
            SpawnSingleCar(lanes[lane2]);
        }
        else
        {
            int randomLane = Random.Range(0, 3);
            SpawnSingleCar(lanes[randomLane]);
        }
    }

    private void SpawnSingleCar(int laneMultiplier)
    {
        if (carPool == null) return;

        EnemyCarMove car = carPool.GetRandomCar();
        if (car == null) return;

        float xPos = laneMultiplier * laneDistance;
        car.transform.position = new Vector3(xPos, 0f, spawnZPosition);
        car.Initialize(currentWorldSpeed, carPool);
    }
}