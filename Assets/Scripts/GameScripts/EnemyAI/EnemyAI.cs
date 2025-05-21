using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Настройки преследования")]
    public float maxFollowDistance = 50f;  

    private Transform player;          
    private NavMeshAgent agent;

    private Enemy_Stats _here_stats;
    private Shotgun_stats Shotgun_damage;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _here_stats = new Enemy_Stats();
        agent.speed = 5f;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.stoppingDistance = 0.5f;
    }
    void OnEnable()
    {
        NavMeshHit hit;
        if (agent != null && NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
    }

    void Start()
    {
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
            agent.SetDestination(player.position);
        }
        else if (agent.hasPath)
        {
            agent.ResetPath();
        }

        if(_here_stats.Health < 0)
        {
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider Bullet)
    {
        if (Bullet.gameObject.TryGetComponent<Pellet>(out var _))
        {
            _here_stats.TakeDamage(Shotgun_damage.DamagePerPellet);
        }
    }
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || agent == null || !agent.hasPath) return;

        Gizmos.color = Color.green;
        var path = agent.path;
        for (int i = 0; i < path.corners.Length - 1; i++)
            Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
    }
}
