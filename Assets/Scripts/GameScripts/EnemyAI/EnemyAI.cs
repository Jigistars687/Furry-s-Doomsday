using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("��������� �������������")]
    public float maxFollowDistance = 50f;  // ������������ ��������� �������������

    private Transform player;              // ��������� ������ �� ������
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // ��������� ������ (����� ���������� �� �����)
        agent.speed = 5f;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.stoppingDistance = 0.5f;
    }
    // EnemyAI.cs (�������� ����� ������ �����)
    void OnEnable()
    {
        // ������������� ������ �� ��������� ����� NavMesh, ����� �������� SetDestination-error
        NavMeshHit hit;
        if (agent != null && NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
    }

    void Start()
    {
        // ���� ��� ���� � ����� ������ � ����� "Player"
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("EnemyAI: �� ������ ������ � ����� \"Player\"!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= maxFollowDistance)
        {
            // ����� ��� �������� ���� � ������ �� NavMesh
            agent.SetDestination(player.position);
        }
        else if (agent.hasPath)
        {
            // ���� ����� ������� ������ � ���������������
            agent.ResetPath();
        }
    }

    // ��� �������: ���������� ���������� � ���������
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || agent == null || !agent.hasPath) return;

        Gizmos.color = Color.green;
        var path = agent.path;
        for (int i = 0; i < path.corners.Length - 1; i++)
            Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
    }
}
