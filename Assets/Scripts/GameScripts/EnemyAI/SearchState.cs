//using UnityEngine;

//public class SearchState : IEnemyState
//{
//    private EnemyAI ai;
//    private float searchTimer = 0f;

//    public SearchState(EnemyAI ai)
//    {
//        this.ai = ai;
//    }

//    public void Enter()
//    {
//        ai.agent.SetDestination(ai.lastKnownPlayerPos);
//        searchTimer = 0f;
//    }

//    public void Tick()
//    {
//        searchTimer += Time.deltaTime;

//        if (ai.CanSeePlayer())
//        {
//            ai.SwitchState<ChaseState>();
//            return;
//        }

//        if (!ai.agent.pathPending && ai.agent.remainingDistance < 0.3f)
//        {
//            if (searchTimer >= ai.config.maxSearchTime)
//            {
//                ai.SwitchState<PatrolState>();
//            }
//        }
//    }

//    public void Exit() { }
//}