using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionType
{
    North = 0,
    East = 1,
    South = 2,
    West = 3,
}

[System.Serializable]
public class EdgeSpawnPoint
{
    public DirectionType Direction;
    public Transform Container;
}

public class RoomPartObject : MonoBehaviour
{
    public EdgeSpawnPoint[] SpawnPoints;

    public void AssignWall(GameObject wallObject, DirectionType direction)
    {
        foreach (var spawnPoint in SpawnPoints)
        {
            if (spawnPoint.Direction == direction)
            {
                wallObject.transform.SetParent(spawnPoint.Container);

                wallObject.transform.localPosition = Vector3.zero;
                wallObject.transform.localEulerAngles = Vector3.zero;

                Debug.Log($"{wallObject.name} {wallObject.transform.position}");

                break;
            }
        }
    }
}