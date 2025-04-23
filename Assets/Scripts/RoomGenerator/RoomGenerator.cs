using System.Collections.Generic;
using UnityEngine;

namespace RoomSystem
{
    public enum DirectionType
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3,
    }

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
        // Направление двери этой комнаты определяет, куда спавнится следующая комната.
        public DirectionType DoorDirection;
    }

    public class RoomGenerator : MonoBehaviour
    {
        private DirectionType nextSpawnDirection = DirectionType.North;
        private DirectionType currentSpawnDirection = DirectionType.North;
        private int roomCounter = 0;
        private List<Room> rooms; // Declare the rooms list
        private const float RoomSpacing = 30f; // Declare RoomSpacing constant

        [Header("Настройки и префабы")]
        [Tooltip("Префаб части комнаты")]
        public RoomPartObject RoomPartObjectPrefab;
        [Tooltip("Префаб стены")]
        public GameObject WallPrefab;
        [Tooltip("Префаб двери (выход)")]
        public GameObject DoorPrefab;
        [Tooltip("Начальная точка спавна первой комнаты")]
        public Transform SpawnPoints;
        [Tooltip("Blueprint‑точки для установки стен/дверей относительно центра комнаты")]
        public RoomPartPoint[] PartPoints;

        private const int RoomsBeforeTurn = 3;
        private int currentDirectionRoomCount = 0;

        private void Awake()
        {
            rooms = new List<Room>(); // Initialize rooms
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Increment the counter for rooms spawned in the current direction
                currentDirectionRoomCount++;

                // Determine if a turn is needed
                if (currentDirectionRoomCount >= RoomsBeforeTurn)
                {
                    nextSpawnDirection = GetTurnDirection(currentSpawnDirection);
                    currentDirectionRoomCount = 0; // Reset counter after the turn
                }
                else
                {
                    nextSpawnDirection = currentSpawnDirection; // Continue in the same direction
                }

                Debug.Log($"Next direction: {nextSpawnDirection}, Current direction: {currentSpawnDirection}");

                // Spawn the room
                SpawnRoom();
            }
        }

        private DirectionType GetTurnDirection(DirectionType previousDirection)
        {
            // Ensure the new direction is perpendicular, not forward or backward
            if (previousDirection == DirectionType.North || previousDirection == DirectionType.South)
            {
                return (Random.Range(0, 2) == 0) ? DirectionType.East : DirectionType.West;
            }
            else // If previousDirection == East or West
            {
                return (Random.Range(0, 2) == 0) ? DirectionType.North : DirectionType.South;
            }
        }

        private void SpawnRoom()
        {
            roomCounter++;

            // Compute the position and forced entrance direction
            Vector3 spawnPos = SpawnPoints.position;
            DirectionType? forcedEntranceDir = null;

            if (rooms.Count > 0)
            {
                Room previousRoom = rooms[rooms.Count - 1];
                spawnPos = previousRoom.RoomContainer.transform.position
                           - GetDirectionVector(currentSpawnDirection) * RoomSpacing;

                forcedEntranceDir = GetOppositeDirection(currentSpawnDirection);
            }

            // Create the room
            GameObject roomContainer = new GameObject("RoomContainer");
            roomContainer.transform.position = spawnPos;

            Room newRoom = new Room
            {
                RoomContainer = roomContainer,
                RoomParts = new List<RoomPartObject>(),
                DoorDirection = currentSpawnDirection
            };

            List<RoomPartObject> doorObjects = new List<RoomPartObject>(); // Track placed doors

            // Generate room parts and assign walls/doors
            foreach (RoomPartPoint partPoint in PartPoints)
            {
                if (partPoint.Point == null) continue;

                Vector3 partPos = spawnPos + partPoint.Point.localPosition;
                RoomPartObject roomPart = Instantiate(RoomPartObjectPrefab, partPos, Quaternion.identity, roomContainer.transform);
                newRoom.RoomParts.Add(roomPart);

                foreach (DirectionType outDir in partPoint.OutsideDirections)
                {
                    if (forcedEntranceDir.HasValue && outDir == forcedEntranceDir.Value) continue;

                    if (outDir == nextSpawnDirection)
                    {
                        // Place a door in the direction of the next room
                        GameObject doorObj = Instantiate(DoorPrefab);
                        roomPart.AssignWall(doorObj, outDir);
                        doorObjects.Add(roomPart); // Track the door
                    }
                    else
                    {
                        // Place walls for other directions
                        GameObject wallObj = Instantiate(WallPrefab);
                        roomPart.AssignWall(wallObj, outDir);
                    }
                }
            }

            // Randomly replace one door with a wall if any doors exist
            if (doorObjects.Count > 0)
            {
                int randomIndex = Random.Range(0, doorObjects.Count);
                RoomPartObject selectedDoorPart = doorObjects[randomIndex];

                // Replace door with a wall
                GameObject replacementWall = Instantiate(WallPrefab);
                selectedDoorPart.AssignWall(replacementWall, nextSpawnDirection);
            }

            rooms.Add(newRoom);

            // Precompute the next spawn direction for the future room
            currentSpawnDirection = nextSpawnDirection;
            nextSpawnDirection = (roomCounter % RoomsBeforeTurn == 0)
                ? GetTurnDirection(currentSpawnDirection)
                : currentSpawnDirection;

            // Manage visible rooms
            UpdateRooms();
        }







        private void UpdateRooms()
        {
            // Если комнат больше 3 – удаляем самую старую, чтобы не захламлять сцену.
            if (rooms.Count > 4)
            {
                DeleteRoom();
            }
        }

        private void DeleteRoom()
        {
            Room roomToDelete = rooms[0];
            foreach (RoomPartObject part in roomToDelete.RoomParts)
            {
                Destroy(part.gameObject);
            }
            Destroy(roomToDelete.RoomContainer);
            rooms.RemoveAt(0);
        }

        // Возвращает единичный вектор по направлению (смещение комнаты).
        private Vector3 GetDirectionVector(DirectionType direction)
        {
            switch (direction)
            {
                case DirectionType.North: return new Vector3(0, 0, 1);
                case DirectionType.South: return new Vector3(0, 0, -1);
                case DirectionType.East: return new Vector3(1, 0, 0);
                case DirectionType.West: return new Vector3(-1, 0, 0);
                default: return Vector3.zero;
            }
        }

        // Возвращает противоположное направление.
        private DirectionType GetOppositeDirection(DirectionType direction)
        {
            switch (direction)
            {
                case DirectionType.North: return DirectionType.South;
                case DirectionType.South: return DirectionType.North;
                case DirectionType.East: return DirectionType.West;
                case DirectionType.West: return DirectionType.East;
                default: return direction;
            }
        }
    }
}
