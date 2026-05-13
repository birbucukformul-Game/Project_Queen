using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    [Tooltip("Tekerleðin dönüþ hýzý (Modelin büyüklüðüne göre ayarlamalýsýn)")]
    [SerializeField] private float rotationSpeed = 1000f;

    [Tooltip("Hangi eksende döneceði (Genelde tekerlekler X ekseninde döner)")]
    [SerializeField] private Vector3 rotationAxis = Vector3.right;

    private bool canRotate = true;

    private void OnEnable()
    {
        canRotate = true;
        PlayerCollision.OnGameOver += StopRotation;
    }

    private void OnDisable()
    {
        PlayerCollision.OnGameOver -= StopRotation;
    }

    private void StopRotation() => canRotate = false;

    private void Update()
    {
        if (!canRotate) return;
        transform.Rotate(rotationAxis * (rotationSpeed * Time.deltaTime), Space.Self);
    }
}