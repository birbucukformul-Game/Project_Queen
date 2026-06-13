using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static event Action<int> OnLapCompleted;
    public static event Action OnWrongWay;

    [Header("Pist Ayarları")]
    [Tooltip("Haritadaki tüm checkpointleri SIRASIYLA buraya sürükle")]
    [SerializeField] private List<CheckpointSingle> checkpointList;

    [Tooltip("Toplam kaç tur atılacak?")]
    [SerializeField] private int totalLaps = 15;

    private int nextCheckpointIndex = 0;
    private int currentLap = 0;

    private void Start()
    {
        foreach (CheckpointSingle cp in checkpointList)
        {
            cp.Initialize(this);
        }
    }

    public void PlayerThroughCheckpoint(CheckpointSingle checkpoint, Transform playerTransform)
    {
        int hitCheckpointIndex = checkpointList.IndexOf(checkpoint);

        // Doğru checkpoint'ten mi geçti?
        if (hitCheckpointIndex == nextCheckpointIndex)
        {
            // EĞER GEÇTİĞİ YER START/FINISH ÇİZGİSİ (Checkpoint 0) İSE:
            if (hitCheckpointIndex == 0)
            {
                currentLap++;

                // İlk geçişte currentLap 1 olur (Yarış yeni başlıyor). 
                // Eğer 1'den büyükse, demek ki turu dolaşıp geri gelmiştir!
                if (currentLap > 1)
                {
                    int completedLap = currentLap - 1;
                    Debug.Log($"Tur {completedLap} Tamamlandı!");
                    OnLapCompleted?.Invoke(completedLap);

                    if (completedLap >= totalLaps)
                    {
                        Debug.Log("YARIŞ BİTTİ!");
                        // İleride GameManager bitiş ekranını buraya bağlayacağız
                    }
                }
            }

            Debug.Log("Doğru Geçiş! Sıradaki: " + ((nextCheckpointIndex + 1) % checkpointList.Count));
            nextCheckpointIndex = (nextCheckpointIndex + 1) % checkpointList.Count;
        }
        else
        {
            Debug.Log("Yanlış Yön! Geri Dön!");
            OnWrongWay?.Invoke();
        }
    }
}