using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;  // для NavMeshBuilder, NavMeshBuildSource, NavMeshBuildMarkup

namespace RoomSystem
{
    public enum DirectionType
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3,
    }

    [Serializable]
    public class RoomPartPoint
    {
        public Transform Point;
        public DirectionType[] OutsideDirections;
    }

    public class Room
    {
        public GameObject RoomContainer;
        public List<RoomPartObject> RoomParts;
        public DirectionType DoorDirection;
        public GameObject TriggerObject;
    }

    public class RoomGenerator : MonoBehaviour
    {
        private DirectionType nextSpawnDirection = DirectionType.North;
        private DirectionType currentSpawnDirection = DirectionType.North;
        private int roomCounter;
        private List<Room> rooms;
        private const float RoomSpacing = 30f;

        [Header("Настройки и префабы")]
        public RoomPartObject RoomPartObjectPrefab;
        public GameObject WallPrefab;
        public GameObject DoorPrefab;
        public GameObject SpawnRoomTriggerPrefab;
        public Transform SpawnPoints;
        public RoomPartPoint[] PartPoints;

        [Header("NavMesh")]
        [Tooltip("NavMeshSurface, который будем перестраивать после каждого спавна")]
        public NavMeshSurface navMeshSurface;

        private const int RoomsBeforeTurn = 3;
        private int currentDirectionRoomCount;
        private NavMeshDataInstance navMeshDataInstance;

        private void Awake()
        {
            rooms = new List<Room>();
            roomCounter = 0;
            currentDirectionRoomCount = 0;
        }

        private void Start()
        {
            if (navMeshSurface == null)
            {
                navMeshSurface = FindObjectOfType<NavMeshSurface>();
                //if (navMeshSurface == null)
                    //Debug.LogError("RoomGenerator: не найден NavMeshSurface на сцене!");
            }

            SpawnRoom(initial: true);
            SpawnRoom();
        }

        private void SpawnRoom(bool initial = false)
        {
            roomCounter++;

            // ------ Ваша логика спавна комнаты ------
            Vector3 spawnPos = SpawnPoints.position;
            DirectionType? forcedEntranceDir = null;

            if (rooms.Count > 0)
            {
                var prev = rooms[rooms.Count - 1];
                spawnPos = prev.RoomContainer.transform.position
                           - GetDirectionVector(currentSpawnDirection) * RoomSpacing;
                forcedEntranceDir = GetOppositeDirection(currentSpawnDirection);
            }

            var roomContainer = new GameObject($"Room_{roomCounter}");
            roomContainer.transform.position = spawnPos;

            var newRoom = new Room
            {
                RoomContainer = roomContainer,
                RoomParts = new List<RoomPartObject>(),
                DoorDirection = currentSpawnDirection,
                TriggerObject = null
            };

            var doorParts = new List<RoomPartObject>();

            foreach (var partPoint in PartPoints)
            {
                if (partPoint.Point == null) continue;
                Vector3 partPos = spawnPos + partPoint.Point.localPosition;
                var roomPart = Instantiate(RoomPartObjectPrefab, partPos, Quaternion.identity, roomContainer.transform);
                newRoom.RoomParts.Add(roomPart);

                foreach (var outDir in partPoint.OutsideDirections)
                {
                    if (forcedEntranceDir.HasValue && outDir == forcedEntranceDir.Value)
                        continue;

                    if (outDir == nextSpawnDirection)
                    {
                        var doorObj = Instantiate(DoorPrefab);
                        roomPart.AssignWall(doorObj, outDir);
                        doorParts.Add(roomPart);
                    }
                    else
                    {
                        var wallObj = Instantiate(WallPrefab);
                        roomPart.AssignWall(wallObj, outDir);
                    }
                }
            }

            if (doorParts.Count > 0)
            {
                int idx = UnityEngine.Random.Range(0, doorParts.Count);
                var sel = doorParts[idx];
                var replWall = Instantiate(WallPrefab);
                sel.AssignWall(replWall, nextSpawnDirection);
            }

            rooms.Add(newRoom);

            if (!initial && SpawnRoomTriggerPrefab != null)
            {
                var triggerObj = Instantiate(SpawnRoomTriggerPrefab, roomContainer.transform);
                triggerObj.transform.localPosition = Vector3.zero;
                var triggerComp = triggerObj.GetComponent<SpawnTrigger>();
                if (triggerComp != null)
                {
                    triggerComp.OnPlayerEnter += () =>
                    {
                        currentDirectionRoomCount++;
                        if (currentDirectionRoomCount >= RoomsBeforeTurn)
                        {
                            nextSpawnDirection = GetTurnDirection(currentSpawnDirection);
                            currentDirectionRoomCount = 0;
                        }
                        else
                        {
                            nextSpawnDirection = currentSpawnDirection;
                        }
                        if (rooms.Count > 3)
                            DeleteRoom();
                        SpawnRoom();
                    };
                }
                newRoom.TriggerObject = triggerObj;
            }

            currentSpawnDirection = nextSpawnDirection;
            nextSpawnDirection = (roomCounter % RoomsBeforeTurn == 0)
                ? GetTurnDirection(currentSpawnDirection)
                : currentSpawnDirection;

            UpdateRooms();
            // ------ Конец логики спавна ------

            // Перезапекаем локальный NavMesh между двух последних комнат, область 150×150×150
            if (rooms.Count >= 2)
                BakeLocalNavMeshBetweenLastTwo(150f);
        }

        private void UpdateRooms()
        {
            if (rooms.Count > 4)
                DeleteRoom();
        }

        private void DeleteRoom()
        {
            var old = rooms[0];
            foreach (var part in old.RoomParts)
                Destroy(part.gameObject);
            if (old.TriggerObject != null)
                Destroy(old.TriggerObject);
            Destroy(old.RoomContainer);
            rooms.RemoveAt(0);
        }

        private Vector3 GetDirectionVector(DirectionType dir)
        {
            switch (dir)
            {
                case DirectionType.North: return Vector3.forward;
                case DirectionType.South: return Vector3.back;
                case DirectionType.East: return Vector3.right;
                case DirectionType.West: return Vector3.left;
                default: return Vector3.zero;
            }
        }

        private DirectionType GetOppositeDirection(DirectionType dir)
        {
            switch (dir)
            {
                case DirectionType.North: return DirectionType.South;
                case DirectionType.South: return DirectionType.North;
                case DirectionType.East: return DirectionType.West;
                case DirectionType.West: return DirectionType.East;
                default: return dir;
            }
        }

        private DirectionType GetTurnDirection(DirectionType prevDir)
        {
            if (prevDir == DirectionType.North || prevDir == DirectionType.South)
                return (UnityEngine.Random.Range(0, 2) == 0)
                    ? DirectionType.East
                    : DirectionType.West;
            else
                return (UnityEngine.Random.Range(0, 2) == 0)
                    ? DirectionType.North
                    : DirectionType.South;
        }

        private void BakeLocalNavMeshBetweenLastTwo(float size)
        {
            if (navMeshSurface == null || rooms.Count < 2) return;

            Vector3 a = rooms[rooms.Count - 2].RoomContainer.transform.position;
            Vector3 b = rooms[rooms.Count - 1].RoomContainer.transform.position;
            Vector3 center = (a + b) * 0.25f;

            // Bounds размером size×size×size
            var bounds = new Bounds(center, Vector3.one * size);

            // Сборка источников и разметок
            var sources = new List<NavMeshBuildSource>();
            var markups = new List<NavMeshBuildMarkup>();
            NavMeshBuilder.CollectSources(
                bounds,
                navMeshSurface.layerMask,
                navMeshSurface.useGeometry,
                navMeshSurface.defaultArea,
                markups,
                sources
            );

            // Построение данных NavMesh
            var data = NavMeshBuilder.BuildNavMeshData(
                navMeshSurface.GetBuildSettings(),
                sources,
                bounds,
                center,
                Quaternion.identity
            );

            if (data == null)
            {
                //Debug.LogError("RoomGenerator: BuildNavMeshData returned null");
                return;
            }

            // Удаляем старый NavMeshDataInstance
            if (navMeshDataInstance.valid)
                navMeshDataInstance.Remove();

            // Добавляем новый и сохраняем инстанс
            navMeshDataInstance = NavMesh.AddNavMeshData(data);
            navMeshSurface.navMeshData = data;
        }
    }
}
