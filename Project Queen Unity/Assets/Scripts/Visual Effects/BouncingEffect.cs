using UnityEngine;
using DG.Tweening;

public class BouncingEffect : MonoBehaviour
{
    [Header("Hedef Obje")]
    [Tooltip("Zýplayacak olan model. EÐER ARABAYA ATIYORSAN, ana objeyi deðil arabanýn 3D modelini (Mesh) sürükle! Boþ býrakýrsan scriptin olduðu objeyi zýplatýr.")]
    [SerializeField] private Transform objectToBounce;

    [Header("Zýplama Ayarlarý")]
    [Tooltip("Ne kadar yükseðe zýplayacaðý")]
    [SerializeField] private float jumpHeight = 1.0f;

    [Tooltip("Bir zýplamanýn (sadece yukarý çýkýþýn) süresi. Düþüþ de bir bu kadar sürer.")]
    [SerializeField] private float jumpDuration = 0.3f;

    [Tooltip("Zýplama hissiyatý. OutQuad, yukarý çýkarken yavaþlayýp aþaðý düþerken hýzlanmayý (yerçekimi hissini) mükemmel verir.")]
    [SerializeField] private Ease easeType = Ease.OutQuad;

    [Header("Baþlangýç Ayarlarý")]
    [Tooltip("Oyun baþlar baþlamaz zýplamaya baþlasýn mý?")]
    [SerializeField] private bool playOnStart = true;

    private Tween _bounceTween;
    private float _originalY;

    private void Start()
    {
        if (objectToBounce == null)
        {
            objectToBounce = transform;
        }

        _originalY = objectToBounce.localPosition.y;

        bool isAnimationEnabled = PlayerPrefs.GetInt("EnableAnimations", 1) == 1;

        if (playOnStart && isAnimationEnabled)
        {
            StartBouncing();
        }
    }

    public void StartBouncing()
    {
        StopBouncing();

        _bounceTween = objectToBounce.DOLocalMoveY(_originalY + jumpHeight, jumpDuration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }

    public void StopBouncing()
    {
        if (_bounceTween != null && _bounceTween.IsActive())
        {
            _bounceTween.Kill();
            objectToBounce.localPosition = new Vector3(
                objectToBounce.localPosition.x,
                _originalY,
                objectToBounce.localPosition.z
            );
        }
    }

    private void OnDestroy()
    {
        StopBouncing();
    }
}