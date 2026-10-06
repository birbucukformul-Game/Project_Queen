using UnityEngine;
using UnityEngine.UI;

public class AnimationSettingsUI : MonoBehaviour
{
    [Tooltip("Animasyonlarý açýp kapatacak Toggle (Checkbox) objesi")]
    [SerializeField] private Toggle animationToggle;

    private void Start()
    {
        if (animationToggle != null)
        {
            animationToggle.isOn = PlayerPrefs.GetInt("EnableAnimations", 1) == 1;

            animationToggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    private void OnToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("EnableAnimations", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        if (animationToggle != null)
        {
            animationToggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }
}