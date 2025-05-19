using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Настройки преследования")]
    public float maxFollowDistance = 50f;  // максимальная дистанция преследования

    private Transform player;              // найденная ссылка на игрока
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // Настройки агента (можно подправить по вкусу)
        agent.speed = 5f;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.stoppingDistance = 0.5f;
    }
    // EnemyAI.cs (фрагмент после спавна врага)
    void OnEnable()
    {
        // Телепортируем агента на ближайшую точку NavMesh, чтобы избежать SetDestination-error
        NavMeshHit hit;
        if (agent != null && NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
    }

    void Start()
    {
        // Один раз ищем в сцене объект с тегом "Player"
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("EnemyAI: не найден объект с тегом \"Player\"!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= maxFollowDistance)
        {
            // Агент сам проложит путь к игроку по NavMesh
            agent.SetDestination(player.position);
        }
        else if (agent.hasPath)
        {
            // Если игрок слишком далеко — останавливаемся
            agent.ResetPath();
        }
    }

    // Для отладки: показываем траекторию в редакторе
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || agent == null || !agent.hasPath) return;

        Gizmos.color = Color.green;
        var path = agent.path;
        for (int i = 0; i < path.corners.Length - 1; i++)
            Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
    }
}
