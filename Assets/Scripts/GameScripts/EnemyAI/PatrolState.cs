using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private EnemyAI ai;
    private List<Transform> waypoints;
    private int currentWaypoint = 0;

    public PatrolState(EnemyAI ai)
    {
        this.ai = ai;
        // Копируем массив EnemyAI.waypoints в List
        waypoints = new List<Transform>(ai.waypoints);
    }

    public void Enter()
    {
        if (waypoints.Count > 0)
            ai.agent.SetDestination(waypoints[currentWaypoint].position);
    }

    public void Tick()
    {
        if (ai.CanSeePlayer())
        {
            ai.lastKnownPlayerPos = ai.Player.position;
            ai.SwitchState<ChaseState>();
            return;
        }

        if (!ai.agent.pathPending && ai.agent.remainingDistance < 0.3f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
            ai.agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    public void Exit() { }
}
