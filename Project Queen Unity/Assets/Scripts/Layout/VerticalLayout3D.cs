using UnityEngine;

public class VerticalLayout3D : MonoBehaviour
{
    [SerializeField] private float spacing = 1.0f;

    [ContextMenu("Align Children")]
    public void AlignChildren()
    {
        float currentY = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition = new Vector3(0, -currentY, 0);
            currentY += spacing;
        }
    }
}