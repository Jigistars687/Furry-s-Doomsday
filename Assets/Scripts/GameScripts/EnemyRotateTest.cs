using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRotateTest : MonoBehaviour
{
    [SerializeField] private Transform _Player;
    [SerializeField] private float _MaxDistance = 10f; // ������������ ���������, �� ������� ������ ����� ��������� �� �������

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

            // ���������, ��������� �� ����� � �������� _MaxDistance � ���� �� ������ ���������
            if (distanceToPlayer <= _MaxDistance && HasLineOfSight())
            {
                agent.SetDestination(_Player.position);
            }
            else
            {
                // ������������� �������� ������
                agent.ResetPath();
            }
        }
    }

    bool HasLineOfSight()
    {
        RaycastHit hit;
        // ������� ��� �� ������� � ������
        if (Physics.Raycast(transform.position, (_Player.position - transform.position).normalized, out hit, _MaxDistance))
        {
            // ���������, ��� ��� ��������� ������, � �� �����
            return hit.transform == _Player;
        }

        return false;
    }
}


