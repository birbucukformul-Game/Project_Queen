using UnityEngine;
using DG.Tweening;

public class CrashFeedback : MonoBehaviour
{
    [Header("Kamera Sarsýntýsý (Camera Shake)")]
    [Tooltip("Titremesini istediðin kamera (Boþ býrakýrsan otomatik Main Camera'yý bulur)")]
    [SerializeField] private Camera targetCamera;

    [Tooltip("Sarsýntýnýn ne kadar süreceði (saniye)")]
    [SerializeField] private float shakeDuration = 0.4f;

    [Tooltip("Sarsýntýnýn þiddeti (Deðer büyüdükçe ekran daha çok savrulur)")]
    [SerializeField] private float shakeStrength = 0.5f;

    [Header("Çarpýþma Efekti (Ýsteðe Baðlý)")]
    [Tooltip("Kaza anýnda çýkacak kývýlcým veya toz efekti (Prefab)")]
    [SerializeField] private GameObject crashParticlePrefab;

    private void OnEnable()
    {
        PlayerCollision.OnGameOver += TriggerCrashFeedback;
    }

    private void OnDisable()
    {
        PlayerCollision.OnGameOver -= TriggerCrashFeedback;
    }

    private void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void TriggerCrashFeedback()
    {
        if (targetCamera != null)
        {
            targetCamera.transform.DOShakePosition(shakeDuration, shakeStrength);
        }

        if (crashParticlePrefab != null)
        {
            Instantiate(crashParticlePrefab, transform.position, Quaternion.identity);
        }
    }
}