using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPoolManager : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private RoadMover roadPrefab;
    [SerializeField] private int poolSize = 15;
    [SerializeField] private float roadLength = 30f;
    [SerializeField] private float recycleZThreshold = -30f;

    [Header("Speed Settings")]
    [SerializeField] private float baseMoveSpeed = 20f;

    private float currentMoveSpeed;
    private Queue<RoadMover> roadPool;
    private RoadMover lastRoadPiece;
    private bool canMove = true;

    private void OnEnable()
    {
        PlayerCollision.OnGameOver += StopRoad;
        DifficultyManager.OnDifficultyIncreased += UpdateSpeed;
    }

    private void OnDisable()
    {
        PlayerCollision.OnGameOver -= StopRoad;
        DifficultyManager.OnDifficultyIncreased -= UpdateSpeed;
    }

    private void StopRoad() => canMove = false;

    private void UpdateSpeed(float speedMultiplier, float spawnInterval)
    {
        currentMoveSpeed = baseMoveSpeed * speedMultiplier;
    }

    private void Start()
    {
        currentMoveSpeed = baseMoveSpeed;
        InitializePool();
        StartCoroutine(RoadTick());
    }

    private void InitializePool()
    {
        roadPool = new Queue<RoadMover>();
        float currentSpawnZ = 0f;

        for (int i = 0; i < poolSize; i++)
        {
            RoadMover roadInstance = Instantiate(roadPrefab, transform);
            roadInstance.transform.position = new Vector3(0, 0, currentSpawnZ);
            roadInstance.RandomizeEnvironment();

            roadPool.Enqueue(roadInstance);
            lastRoadPiece = roadInstance;
            currentSpawnZ += roadLength;
        }
    }

    private IEnumerator RoadTick()
    {
        while (true)
        {
            if (canMove)
            {
                foreach (RoadMover road in roadPool)
                {
                    road.Move(currentMoveSpeed);
                }

                if (roadPool.Count > 0 && roadPool.Peek().transform.position.z <= recycleZThreshold)
                {
                    RecycleRoad();
                }
            }
            yield return null;
        }
    }

    private void RecycleRoad()
    {
        RoadMover roadToRecycle = roadPool.Dequeue();
        float newZPosition = lastRoadPiece.transform.position.z + roadLength;

        roadToRecycle.transform.position = new Vector3(
            roadToRecycle.transform.position.x,
            roadToRecycle.transform.position.y,
            newZPosition
        );

        roadToRecycle.RandomizeEnvironment();
        roadPool.Enqueue(roadToRecycle);
        lastRoadPiece = roadToRecycle;
    }
}