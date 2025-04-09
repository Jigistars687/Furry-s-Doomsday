using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomPartPoint
{
    public Transform Point;
    public DirectionType[] OutsideDirections;
}

public class Room
{
    public GameObject RoomContainer; 
    public List<RoomPartObject> RoomParts;
    public DirectionType OutsideDirection;
}

public class RoomGenerator : MonoBehaviour
{
    private const int MaxDoorsAmountPerRoom = 2;

    public RoomPartObject RoomPartObject;
    public GameObject WallPrefab;
    public GameObject WallWithDoorPrefab;
    public Transform SpawnPoints;
    public RoomPartPoint[] PartPoints;

    private List<Room> _rooms;

    private void Awake()
    {
        _rooms = new List<Room>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRoom();
        }
    }

    private void SpawnRoom()
    {
        var lastRoomIndex = _rooms.Count - 1;
        var currentDoorsCount = 0;
        DirectionType? directionWithDoor = null;

        
        GameObject roomContainer = new GameObject("RoomContainer");
        Room newRoom = new Room { RoomContainer = roomContainer, RoomParts = new List<RoomPartObject>() };

        Room previousRoom;
        Vector3 spawnPosition;

        if (lastRoomIndex >= 0)
        {
            previousRoom = _rooms[lastRoomIndex];
            spawnPosition = previousRoom.RoomContainer.transform.position;
        }
        else
        {
            spawnPosition = SpawnPoints.position;
        }

        
        int randomDirection = UnityEngine.Random.Range(0, 4);
        if (randomDirection == 0)
        { 
            spawnPosition.x += 30f; 
        }
        else if (randomDirection == 1)
        {
            spawnPosition.x -= 30f; 
        }
        if (randomDirection == 2)
        {
            spawnPosition.z += 30f;
        }
        if (randomDirection == 3)
        { 
            spawnPosition.z -= 30f; 
        }

        roomContainer.transform.position = spawnPosition;

        for (var i = 0; i < PartPoints.Length; i++)
        {
            var partPoint = PartPoints[i];
            var roomPartObject = Instantiate(RoomPartObject, roomContainer.transform.position + partPoint.Point.localPosition, Quaternion.identity, roomContainer.transform);

            newRoom.RoomParts.Add(roomPartObject);

            foreach (var outsideDirection in partPoint.OutsideDirections)
            {
                var outsideObject = WallPrefab;

                if (currentDoorsCount < MaxDoorsAmountPerRoom && outsideDirection != directionWithDoor)
                {
                    var shouldDoorBeSpawned = UnityEngine.Random.Range(0, 10) > 5;

                    if (shouldDoorBeSpawned || i == PartPoints.Length - 1)
                    {
                        outsideObject = WallWithDoorPrefab;
                        currentDoorsCount++;
                        directionWithDoor = outsideDirection;
                        newRoom.OutsideDirection = outsideDirection;
                    }
                }

                var outsideObjectInstance = Instantiate(outsideObject);
                roomPartObject.AssignWall(outsideObjectInstance, outsideDirection);
            }
        }

        _rooms.Add(newRoom);
        UpdateRooms();
    }

    private void UpdateRooms()
    {
        if (_rooms.Count > 3)
        {
            DeleteRoom();
        }
    }

    private void DeleteRoom()
    {
        var roomToDelete = _rooms[0];

        foreach (var part in roomToDelete.RoomParts)
        {
            Destroy(part.gameObject);
        }

        Destroy(roomToDelete.RoomContainer);
        _rooms.RemoveAt(0);
    }
}
