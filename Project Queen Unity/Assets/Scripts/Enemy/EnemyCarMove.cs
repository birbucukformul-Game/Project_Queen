using UnityEngine;

public class EnemyCarMove : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float engineSpeed = 15f;
    [SerializeField] private float despawnZPos = -30f;

    [HideInInspector] public int poolID;

    private float currentWorldSpeed;
    private EnemyCarPool myPool;
    private bool canMove = true;

    private void OnEnable()
    {
        canMove = true;
        PlayerCollision.OnGameOver += StopMovement;
    }

    private void OnDisable()
    {
        PlayerCollision.OnGameOver -= StopMovement;
    }

    private void StopMovement() => canMove = false;

    public void Initialize(float worldSpeed, EnemyCarPool pool)
    {
        currentWorldSpeed = worldSpeed;
        myPool = pool;
    }

    private void Update()
    {
        if (!canMove) return;

        float totalSpeed = currentWorldSpeed + engineSpeed;
        transform.Translate(Vector3.back * (totalSpeed * Time.deltaTime));

        if (transform.position.z <= despawnZPos)
        {
            Recycle();
        }
    }

    private void Recycle()
    {
        if (myPool != null)
        {
            myPool.ReturnCar(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}