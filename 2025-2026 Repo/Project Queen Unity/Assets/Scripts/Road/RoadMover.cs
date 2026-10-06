using UnityEngine;

public class RoadMover : MonoBehaviour
{
    [Header("Environment Elements")]
    [Tooltip("Rastgele açýlýp kapanacak aðaç/çevre objelerini buraya sürükleyin.")]
    [SerializeField] private GameObject[] environmentObjects;

    public void Move(float speed)
    {
        transform.Translate(Vector3.back * (speed * Time.deltaTime), Space.World);
    }

    public void RandomizeEnvironment()
    {
        if (environmentObjects == null || environmentObjects.Length == 0) return;

        for (int i = 0; i < environmentObjects.Length; i++)
        {
            bool isActive = Random.value > 0.5f;
            environmentObjects[i].SetActive(isActive);
        }
    }
}