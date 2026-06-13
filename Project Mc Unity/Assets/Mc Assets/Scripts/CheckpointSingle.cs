using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private CheckpointManager trackManager;

    // Yönetici oyun başlarken kendini bu noktalara tanıtacak
    public void Initialize(CheckpointManager manager)
    {
        trackManager = manager;
    }

    private void OnTriggerEnter(Collider other)
    {
        // İçinden geçen şey bizim arabamızsa (Tag'i Player olarak ayarli olmalı)
        if (other.CompareTag("Player"))
        {
            trackManager.PlayerThroughCheckpoint(this, other.transform);
        }
    }
}