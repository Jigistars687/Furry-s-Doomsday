using UnityEngine;

public class ChaseState : IEnemyState
{
    private EnemyAI ai;

    public ChaseState(EnemyAI ai)
    {
        this.ai = ai;
    }

    public void Enter()
    {
        ai.agent.SetDestination(ai.Player.position);
    }
    public void Tick()
    {
        if (ai.CanSeePlayer())
        {
            ai.lastKnownPlayerPos = ai.Player.position;
            ai.agent.SetDestination(ai.Player.position);
        }
        else
        {
            ai.SwitchState<SearchState>();
        }
    }

    public void Exit() { }
}