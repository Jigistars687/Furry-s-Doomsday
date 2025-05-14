using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyWallFollower : MonoBehaviour
{
    public Transform playerTarget;
    public float maxFollowDistance = 50f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;

    public float probeRadius = 0.5f;
    public float probeDistance = 1.5f;
    public float wallFollowTimeout = 1f;

    private enum State { Seeking, WallFollowing }
    private State currentState = State.Seeking;

    private Rigidbody rb;
    private Vector3 wallNormal;
    private Vector3 wallFollowDir;
    private float wallFollowTimer;
    private float initialY;
    private Vector3 currentDir;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;  // непрерывная детекция :contentReference[oaicite:2]{index=2}
        rb.constraints = RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ;
    }

    void Start()
    {
        initialY = transform.position.y;
        currentDir = transform.forward;
    }

    void FixedUpdate()
    {
        if (playerTarget == null) return;

        Vector3 fromPos = transform.position + Vector3.up * 0.5f;  // поднимаем луч над полом
        Vector3 toPlayer = playerTarget.position - fromPos;
        float distToPlayer = toPlayer.magnitude;
        if (distToPlayer > maxFollowDistance)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Vector3 desiredDir = toPlayer / distToPlayer;

        // Отладочный луч к игроку
        Debug.DrawRay(fromPos, desiredDir * distToPlayer, Color.red, 0f, false);  // визуализация Raycast :contentReference[oaicite:3]{index=3}

        // 1) Проверка прямой видимости до игрока
        if (!Physics.Raycast(
                fromPos,
                desiredDir,
                out RaycastHit rayHit,
                distToPlayer,
                ~0,
                QueryTriggerInteraction.Ignore))
        {
            // Прямой путь свободен — сразу идём к игроку
            currentState = State.Seeking;
            currentDir = Vector3.Lerp(currentDir, desiredDir, 0.2f).normalized;  // сглаживание :contentReference[oaicite:4]{index=4}
        }
        else
        {
            // Есть препятствие — переключаемся на логику обхода
            switch (currentState)
            {
                case State.Seeking: TrySeek(desiredDir); break;
                case State.WallFollowing: DoWallFollow(desiredDir); break;
            }
        }

        // Применяем движение через velocity — физика не пропустит сквозь стены :contentReference[oaicite:5]{index=5}
        rb.velocity = currentDir * moveSpeed;

        // Фиксируем высоту
        Vector3 pos = transform.position;
        pos.y = initialY;
        transform.position = pos;

        // Плавный поворот по направлению движения
        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(currentDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed));
        }
    }

    private void TrySeek(Vector3 desiredDir)
    {
        // Пробуем прямую сферическую проверку пути
        if (!Physics.SphereCast(transform.position, probeRadius, desiredDir, out RaycastHit hit, probeDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            currentDir = Vector3.Lerp(currentDir, desiredDir, 0.2f).normalized;
            return;
        }

        // При столкновении переходим в WallFollowing
        wallNormal = hit.normal;
        Vector3 slideA = Vector3.Cross(wallNormal, Vector3.up).normalized;
        Vector3 slideB = -slideA;
        wallFollowDir = (Vector3.Dot(slideA, desiredDir) > Vector3.Dot(slideB, desiredDir)) ? slideA : slideB;
        wallFollowTimer = wallFollowTimeout;
        currentState = State.WallFollowing;
    }
    private void DoWallFollow(Vector3 desiredDir)
    {
        if (wallFollowTimer > 0f)
        {
            wallFollowTimer -= Time.fixedDeltaTime;
            currentDir = Vector3.Lerp(currentDir, wallFollowDir, 0.2f).normalized;
            return;
        }

        // По таймауту проверяем прямую видимость к игроку
        if (!Physics.SphereCast(transform.position, probeRadius, desiredDir, out RaycastHit hit, probeDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            currentState = State.Seeking;
            currentDir = Vector3.Lerp(currentDir, desiredDir, 0.2f).normalized;
            return;
        }

        // Если стена поворачивается — обновляем направление скольжения
        if (Physics.SphereCast(transform.position, probeRadius, wallFollowDir, out RaycastHit hit2, probeDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            wallNormal = hit2.normal;
            Vector3 slideA = Vector3.Cross(wallNormal, Vector3.up).normalized;
            Vector3 slideB = -slideA;
            wallFollowDir = (Vector3.Dot(slideA, desiredDir) > Vector3.Dot(slideB, desiredDir)) ? slideA : slideB;
        }
        currentDir = Vector3.Lerp(currentDir, wallFollowDir, 0.2f).normalized;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, probeDistance);
    }
}