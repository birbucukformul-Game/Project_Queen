using UnityEngine;

public class HorizontalLayout3D : MonoBehaviour
{
    [SerializeField] private float spacing = 1.5f;

    [ContextMenu("Align Children Horizontally")]
    public void AlignChildren()
    {
        float currentX = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition = new Vector3(currentX, 0, 0);
            currentX += spacing;
        }
    }
}