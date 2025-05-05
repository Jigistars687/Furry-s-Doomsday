using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [Header("Точки патруля в сцене")]
    public Transform[] waypoints;

    [Header("Конфигурация врага")]
    public EnemyConfig config;

    [HideInInspector] public NavMeshAgent agent;

    [SerializeField] private Transform player;
    public Transform Player => player;

    public Vector3 lastKnownPlayerPos;

    private IEnemyState currentState;
    private Dictionary<System.Type, IEnemyState> states;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        states = new Dictionary<System.Type, IEnemyState>()
        {
            { typeof(PatrolState), new PatrolState(this) },
            { typeof(ChaseState),  new ChaseState(this) },
            { typeof(SearchState), new SearchState(this) },
        };
    }

    void Start()
    {
        ApplyConfig();
        SwitchState<PatrolState>();
    }

    void Update()
    {
        currentState?.Tick();
    }

    public void SwitchState<T>() where T : IEnemyState
    {
        currentState?.Exit();
        currentState = states[typeof(T)];
        currentState.Enter();
    }

    private void ApplyConfig()
    {
        agent.speed = config.moveSpeed;
        agent.stoppingDistance = config.stoppingDistance;
    }

    public bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (Player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle < config.viewAngle / 2f)
        {
            if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer, out RaycastHit hit, config.viewDistance, config.obstacleMask))
            {
                if (hit.transform == Player)
                {
                    return true;
                }
            }
        }

        return false;
    }
}