using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [Header("Görsel Ayarlar (Mesh)")]
    [Tooltip("Arabanýn 3D modeli (Collider havaya kalkmasýn diye sadece bunu zýplatacaðýz)")]
    [SerializeField] private Transform carMesh;

    [Tooltip("Þerit deðiþtirirken arabanýn ne kadar yükseðe zýplayacaðý")]
    [SerializeField] private float jumpPower = 1.5f;

    [Header("Þerit Ayarlarý (Lane Settings)")]
    [Tooltip("Þeritler arasý X eksenindeki mesafe.")]
    [SerializeField] private float laneDistance = 3f;

    [Header("Hareket Ayarlarý (Movement Settings)")]
    [Tooltip("Þerit deðiþtirme animasyonunun süresi (saniye).")]
    [SerializeField] private float moveDuration = 0.25f;

    private int _currentLane = 0;
    private bool canMove = true;
    private float _originalMeshY;

    private void Start()
    {
        if (carMesh != null)
        {
            _originalMeshY = carMesh.localPosition.y;
        }
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnMoveLeft += MoveLeft;
        PlayerInputHandler.OnMoveRight += MoveRight;
        PlayerCollision.OnGameOver += DisableMovement;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnMoveLeft -= MoveLeft;
        PlayerInputHandler.OnMoveRight -= MoveRight;
        PlayerCollision.OnGameOver -= DisableMovement;
    }

    private void DisableMovement()
    {
        canMove = false;

        if (carMesh != null)
        {
            carMesh.DOKill();
            carMesh.localPosition = new Vector3(0, _originalMeshY, 0);
        }
    }

    private void MoveLeft()
    {
        if (!canMove || _currentLane == -1) return;

        ChangeLane(-1);
    }

    private void MoveRight()
    {
        if (!canMove || _currentLane == 1) return;

        ChangeLane(1);
    }

    private void ChangeLane(int direction)
    {
        _currentLane = Mathf.Clamp(_currentLane + direction, -1, 1);
        float targetPositionX = _currentLane * laneDistance;

        transform.DOMoveX(targetPositionX, moveDuration).SetEase(Ease.OutQuad);

        if (carMesh != null)
        {
            carMesh.DOKill();
            carMesh.localPosition = new Vector3(0, _originalMeshY, 0);
            bool isAnimationEnabled = PlayerPrefs.GetInt("EnableAnimations", 1) == 1;

            if (isAnimationEnabled)
            {
                carMesh.DOLocalJump(new Vector3(0, _originalMeshY, 0), jumpPower, 1, moveDuration);
            }
        }
    }
}