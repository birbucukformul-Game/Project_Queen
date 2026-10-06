using System.Collections.Generic;
using UnityEngine;

public class EnemyCarPool : MonoBehaviour
{
    [Header("Çoklu Havuz Ayarlarý")]
    [Tooltip("Havuza eklenecek farklý düþman arabasý prefab'larý")]
    [SerializeField] private EnemyCarMove[] enemyCarPrefabs;

    [Tooltip("Oyun baþlarken HER BÝR araba türünden üretilecek sayý")]
    [SerializeField] private int initialPoolSizePerCar = 5;

    private Queue<EnemyCarMove>[] poolQueues;
    private int lastSpawnedPoolIndex = -1;

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        poolQueues = new Queue<EnemyCarMove>[enemyCarPrefabs.Length];

        for (int i = 0; i < enemyCarPrefabs.Length; i++)
        {
            poolQueues[i] = new Queue<EnemyCarMove>();

            for (int j = 0; j < initialPoolSizePerCar; j++)
            {
                EnemyCarMove car = Instantiate(enemyCarPrefabs[i], transform);
                car.gameObject.SetActive(false);
                car.poolID = i;
                poolQueues[i].Enqueue(car);
            }
        }
    }

    public EnemyCarMove GetRandomCar()
    {
        if (enemyCarPrefabs.Length == 0) return null;

        int randomPoolIndex = 0;

        if (enemyCarPrefabs.Length > 1)
        {
            do
            {
                randomPoolIndex = Random.Range(0, enemyCarPrefabs.Length);
            }
            while (randomPoolIndex == lastSpawnedPoolIndex);
        }

        lastSpawnedPoolIndex = randomPoolIndex;

        Queue<EnemyCarMove> selectedQueue = poolQueues[randomPoolIndex];

        if (selectedQueue.Count > 0)
        {
            EnemyCarMove car = selectedQueue.Dequeue();
            car.gameObject.SetActive(true);
            return car;
        }

        EnemyCarMove newCar = Instantiate(enemyCarPrefabs[randomPoolIndex], transform);
        newCar.poolID = randomPoolIndex;
        return newCar;
    }

    public void ReturnCar(EnemyCarMove car)
    {
        car.gameObject.SetActive(false);
        poolQueues[car.poolID].Enqueue(car);
    }
}