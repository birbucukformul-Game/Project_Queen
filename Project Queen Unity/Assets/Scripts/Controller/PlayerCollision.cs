using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public static event Action OnGameOver;

    private bool hasCrashed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasCrashed) return;

        if (other.CompareTag("Enemy"))
        {
            hasCrashed = true;
            OnGameOver?.Invoke();
        }
    }
}