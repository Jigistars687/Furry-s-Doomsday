using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [Header("Настройки")]
    public Transform player;
    public float moveSpeed = 5f;
    public float viewDistance = 10f;
    public float viewAngle = 45f;

    private Vector3 lastKnownPlayerPos;
    private bool canSeePlayer;

    void Update()
    {
        canSeePlayer = CheckPlayerInSight();

        if (canSeePlayer)
        {
            lastKnownPlayerPos = player.position;
            MoveTowards(lastKnownPlayerPos);
        }
        else if (lastKnownPlayerPos != Vector3.zero)
        {
            MoveTowards(lastKnownPlayerPos);
        }
    }

    private bool CheckPlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (distanceToPlayer <= viewDistance && angleToPlayer <= viewAngle / 2f)
        {
            if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out RaycastHit hit, viewDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Поворот в сторону цели
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
    }
}
