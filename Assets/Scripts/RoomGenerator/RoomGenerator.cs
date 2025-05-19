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
        [Tooltip("Префаб части комнаты")]
        public RoomPartObject RoomPartObjectPrefab;
        [Tooltip("Префаб стены")]
        public GameObject WallPrefab;
        [Tooltip("Префаб двери (выход)")]
        public GameObject DoorPrefab;
        [Tooltip("Префаб триггера для спавна следующей комнаты")]
        public GameObject SpawnRoomTriggerPrefab;
        [Tooltip("Начальная точка спавна комнаты")]
        public Transform SpawnPoints;
        [Tooltip("Blueprint-точки для установки стен/дверей относительно центра комнаты")]
        public RoomPartPoint[] PartPoints;

        [Header("NavMesh")]
        [Tooltip("Ссылка на главный NavMeshSurface сцены")]
        public NavMeshSurface navMeshSurface;
        [Tooltip("Радиус локального запекания после спавна")]
        public float bakeRadius = 50f;

        private const int RoomsBeforeTurn = 3;
        private int currentDirectionRoomCount;
        private NavMeshDataInstance navMeshDataInstance;

        private void Awake()
        {
            rooms = new List<Room>();
            roomCounter = 0;
            currentDirectionRoomCount = 0;
        }

        void Start()
        {
            // если не задано в инспекторе — найдём первый на сцене
            if (navMeshSurface == null)
            {
                navMeshSurface = FindObjectOfType<NavMeshSurface>();
                if (navMeshSurface == null)
                    Debug.LogError("RoomGenerator: не найден ни один NavMeshSurface на сцене!");
            }

            // Теперь можно смело вызывать первый SpawnRoom
            SpawnRoom(true);
            SpawnRoom();
        }


        private void SpawnRoom(bool initial = false)
        {
            roomCounter++;

            // --- Логика расположения и генерации комнаты ---
            Vector3 spawnPos = SpawnPoints.position;
            DirectionType? forcedEntranceDir = null;

            if (rooms.Count > 0)
            {
                var prevRoom = rooms[rooms.Count - 1];
                spawnPos = prevRoom.RoomContainer.transform.position
                         - GetDirectionVector(currentSpawnDirection) * RoomSpacing;
                forcedEntranceDir = GetOppositeDirection(currentSpawnDirection);
            }

            // Создаём пустой контейнер для новой комнаты
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

            // Инстанциируем части комнаты и добавляем двери/стены
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

            // Если есть несколько вариантов выхода, оставляем только один
            if (doorParts.Count > 0)
            {
                int idx = UnityEngine.Random.Range(0, doorParts.Count);
                var sel = doorParts[idx];
                var replWall = Instantiate(WallPrefab);
                sel.AssignWall(replWall, nextSpawnDirection);
            }

            rooms.Add(newRoom);

            // Спавн триггера для следующей комнаты (если не initial)
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

            // Обновляем направление и удаляем старые комнаты по необходимости
            currentSpawnDirection = nextSpawnDirection;
            nextSpawnDirection = (roomCounter % RoomsBeforeTurn == 0)
                ? GetTurnDirection(currentSpawnDirection)
                : currentSpawnDirection;
            UpdateRooms();
            // --- Конец логики спавна ---

            // Локальное запекание NavMesh вокруг центра новой комнаты
            BakeLocalNavMesh(spawnPos);
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

        private void BakeLocalNavMesh(Vector3 center)
        {
            if (navMeshSurface == null)
            {
                Debug.LogWarning("BakeLocalNavMesh отменён — отсутствует navMeshSurface");
                return;
            }
            // ... остальной код без изменен

        // Определяем область запекания
        var bounds = new Bounds(center, Vector3.one * bakeRadius * 2f);

            // Собираем источники и разметки
            var sources = new List<NavMeshBuildSource>();
            var markups = new List<NavMeshBuildMarkup>();
            NavMeshBuilder.CollectSources(
                bounds,
                navMeshSurface.layerMask,
                navMeshSurface.useGeometry == NavMeshCollectGeometry.RenderMeshes
                    ? NavMeshCollectGeometry.RenderMeshes
                    : NavMeshCollectGeometry.PhysicsColliders,
                navMeshSurface.defaultArea,
                markups,
                sources
            );

            // Строим новые данные NavMesh для этой области
            var buildSettings = navMeshSurface.GetBuildSettings();
            var data = NavMeshBuilder.BuildNavMeshData(
                buildSettings,
                sources,
                bounds,
                center,
                Quaternion.identity
            );

            // Удаляем предыдущий инстанс (если он был)
            if (navMeshDataInstance.valid)
                navMeshDataInstance.Remove();

            // Добавляем новую поверхность и сохраняем инстанс
            navMeshDataInstance = NavMesh.AddNavMeshData(data);
            navMeshSurface.navMeshData = data;
        }
    }
}
