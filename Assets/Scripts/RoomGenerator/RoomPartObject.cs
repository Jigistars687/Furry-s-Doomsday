using UnityEngine;

namespace RoomSystem
{
    [System.Serializable]
    public class EdgeSpawnPoint
    {
        public DirectionType Direction; // ���������� ����� enum �� RoomSystem
        public Transform Container;
    }

    public class RoomPartObject : MonoBehaviour
    {
        public EdgeSpawnPoint[] SpawnPoints;

        // ����������� ���������� ������ (����� ��� �����) � ��������������� �����.
        public void AssignWall(GameObject obj, DirectionType direction)
        {
            foreach (EdgeSpawnPoint spawnPoint in SpawnPoints)
            {
                if (spawnPoint.Direction == direction)
                {
                    obj.transform.SetParent(spawnPoint.Container);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localEulerAngles = Vector3.zero;
                    //Debug.Log($"{obj.name} assigned to {direction} at {spawnPoint.Container.position}");
                    return;
                }
            }

            //Debug.LogWarning($"�� ������ SpawnPoint ��� ����������� {direction} �� {gameObject.name}");
        }
    }
}
