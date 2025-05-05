using UnityEngine;

[CreateAssetMenu(menuName = "AI/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("��������� ��������")]
    public float moveSpeed = 3.5f;
    public float stoppingDistance = 0.5f;

    [Header("��������� ������")]
    public float viewDistance = 20f;
    [Range(0, 360)] public float viewAngle = 90f;
    public LayerMask obstacleMask;

//    [Header("�������")]
//    public Transform[] waypoints;

    [Header("�����")]
    public float searchRadius = 5f;
    public float maxSearchTime = 5f;
}