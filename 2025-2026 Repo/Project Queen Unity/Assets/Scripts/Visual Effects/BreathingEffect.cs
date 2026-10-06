using UnityEngine;
using DG.Tweening;

public class BreathingEffect : MonoBehaviour
{
    [Header("Efekt Ayarlarý")]
    [Tooltip("Objenin ulaþacaðý maksimum büyüklük çarpaný (Örn: 1.1 = %10 büyüme)")]
    [SerializeField] private float scaleMultiplier = 1.1f;

    [Tooltip("Nefes alma döngüsünün süresi (Saniye)")]
    [SerializeField] private float duration = 1.0f;

    [Tooltip("Animasyonun yumuþatma tipi")]
    [SerializeField] private Ease easeType = Ease.InOutSine;

    [Header("Baþlangýç Ayarlarý")]
    [Tooltip("Oyun baþlar baþlamaz nefes almaya baþlasýn mý?")]
    [SerializeField] private bool playOnStart = true;

    private Vector3 _originalScale;
    private Tween _breathingTween;

    private void Start()
    {
        _originalScale = transform.localScale;

        if (playOnStart)
        {
            StartBreathing();
        }
    }

    public void StartBreathing()
    {
        StopBreathing();

        _breathingTween = transform.DOScale(_originalScale * scaleMultiplier, duration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }

    public void StopBreathing()
    {
        if (_breathingTween != null && _breathingTween.IsActive())
        {
            _breathingTween.Kill();
            transform.localScale = _originalScale;
        }
    }

    private void OnDestroy()
    {
        StopBreathing();
    }
}