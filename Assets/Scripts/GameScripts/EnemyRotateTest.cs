using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRotateTest : MonoBehaviour
{
    [SerializeField] private Transform _Player;
    [SerializeField] private float _MaxDistance = 10f; // Максимальная дистанция, на которой объект будет следовать за игроком

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (_Player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _Player.position);

            // Проверяем, находится ли игрок в пределах _MaxDistance и есть ли прямая видимость
            if (distanceToPlayer <= _MaxDistance && HasLineOfSight())
            {
                agent.SetDestination(_Player.position);
            }
            else
            {
                // Останавливаем движение агента
                agent.ResetPath();
            }
        }
    }

    bool HasLineOfSight()
    {
        RaycastHit hit;
        // Создаем луч от объекта к игроку
        if (Physics.Raycast(transform.position, (_Player.position - transform.position).normalized, out hit, _MaxDistance))
        {
            // Проверяем, что луч достигает игрока, а не стены
            return hit.transform == _Player;
        }

        return false;
    }
}


