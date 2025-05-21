using UnityEngine;

namespace RoomSystem
{
    [System.Serializable]
    public class EdgeSpawnPoint
    {
        public DirectionType Direction; // Используем общий enum из RoomSystem
        public Transform Container;
    }

    public class RoomPartObject : MonoBehaviour
    {
        public EdgeSpawnPoint[] SpawnPoints;

        // Привязывает переданный объект (дверь или стену) к соответствующей точке.
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

            //Debug.LogWarning($"Не найден SpawnPoint для направления {direction} на {gameObject.name}");
        }
    }
}
