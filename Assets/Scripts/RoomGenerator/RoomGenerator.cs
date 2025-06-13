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
        private const float CeilingHeight = 10.0f; // высота комнаты, подберите под ваш проект
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

        //private void SpawnRoom(bool initial = false)
        //{
        //    roomCounter++;

        //    Vector3 spawnPos = SpawnPoints.position;
        //    DirectionType? forcedEntranceDir = null;

        //    if (rooms.Count > 0)
        //    {
        //        var prev = rooms[rooms.Count - 1];
        //        spawnPos = prev.RoomContainer.transform.position
        //                   - GetDirectionVector(currentSpawnDirection) * RoomSpacing;
        //        forcedEntranceDir = GetOppositeDirection(currentSpawnDirection);
        //    }

        //    var roomContainer = new GameObject($"Room_{roomCounter}");
        //    roomContainer.transform.position = spawnPos;

        //    var newRoom = new Room
        //    {
        //        RoomContainer = roomContainer,
        //        RoomParts = new List<RoomPartObject>(),
        //        DoorDirection = currentSpawnDirection,
        //        TriggerObject = null
        //    };

        //    // --- Исправление: заранее определяем, где будет дверь ---
        //    DirectionType? doorDirection = null;
        //    List<RoomPartObject> doorCandidates = new List<RoomPartObject>();
        //    List<Tuple<RoomPartObject, DirectionType>> possibleDoors = new List<Tuple<RoomPartObject, DirectionType>>();

        //    foreach (var partPoint in PartPoints)
        //    {
        //        if (partPoint.Point == null) continue;
        //        Vector3 partPos = spawnPos + partPoint.Point.localPosition;
        //        var roomPart = Instantiate(RoomPartObjectPrefab, partPos, Quaternion.identity, roomContainer.transform);
        //        newRoom.RoomParts.Add(roomPart);

        //        foreach (var outDir in partPoint.OutsideDirections)
        //        {
        //            if (forcedEntranceDir.HasValue && outDir == forcedEntranceDir.Value)
        //                continue;

        //            if (outDir == nextSpawnDirection)
        //            {
        //                possibleDoors.Add(Tuple.Create(roomPart, outDir));
        //            }
        //        }
        //    }

        //    // Выбираем только одну дверь
        //    Tuple<RoomPartObject, DirectionType> selectedDoor = null;
        //    if (possibleDoors.Count > 0)
        //    {
        //        int idx = UnityEngine.Random.Range(0, possibleDoors.Count);
        //        selectedDoor = possibleDoors[idx];
        //    }

        //    // Теперь расставляем стены и одну дверь
        //    int partIdx = 0;
        //    foreach (var partPoint in PartPoints)
        //    {
        //        if (partPoint.Point == null) continue;
        //        var roomPart = newRoom.RoomParts[partIdx];
        //        partIdx++;

        //        foreach (var outDir in partPoint.OutsideDirections)
        //        {
        //            if (forcedEntranceDir.HasValue && outDir == forcedEntranceDir.Value)
        //                continue;

        //            if (selectedDoor != null && roomPart == selectedDoor.Item1 && outDir == selectedDoor.Item2)
        //            {
        //                var doorObj = Instantiate(DoorPrefab);
        //                roomPart.AssignWall(doorObj, outDir);
        //            }
        //            else
        //            {
        //                var wallObj = Instantiate(WallPrefab);
        //                roomPart.AssignWall(wallObj, outDir);
        //            }
        //        }
        //    }

        //    rooms.Add(newRoom);

        //    if (!initial && SpawnRoomTriggerPrefab != null)
        //    {
        //        var triggerObj = Instantiate(SpawnRoomTriggerPrefab, roomContainer.transform);
        //        triggerObj.transform.localPosition = Vector3.zero;
        //        var triggerComp = triggerObj.GetComponent<SpawnTrigger>();
        //        if (triggerComp != null)
        //        {
        //            triggerComp.OnPlayerEnter += () =>
        //            {
        //                currentDirectionRoomCount++;
        //                if (currentDirectionRoomCount >= RoomsBeforeTurn)
        //                {
        //                    nextSpawnDirection = GetTurnDirection(currentSpawnDirection);
        //                    currentDirectionRoomCount = 0;
        //                }
        //                else
        //                {
        //                    nextSpawnDirection = currentSpawnDirection;
        //                }
        //                if (rooms.Count > 3)
        //                    DeleteRoom();
        //                SpawnRoom();
        //            };
        //        }
        //        newRoom.TriggerObject = triggerObj;
        //    }

        //    currentSpawnDirection = nextSpawnDirection;
        //    nextSpawnDirection = (roomCounter % RoomsBeforeTurn == 0)
        //        ? GetTurnDirection(currentSpawnDirection)
        //        : currentSpawnDirection;

        //    UpdateRooms();

        //    if (rooms.Count >= 2)
        //        BakeLocalNavMeshBetweenLastTwo(150f);
        //}
        private GameObject FindBackDoorObject(Room room, Vector3 prevRoomPosition)
        {
            DoorController closestDoor = null;
            float minDist = float.MaxValue;
            int checkedDoors = 0;

            foreach (var part in room.RoomParts)
            {
                // Ищем все DoorController в иерархии RoomPartObject
                var doors = part.GetComponentsInChildren<DoorController>(true);
                foreach (var door in doors)
                {
                    checkedDoors++;
                    float dist = Vector3.Distance(door.transform.position, prevRoomPosition);
                    Debug.Log($"[RoomGen] Door found: {door.gameObject.name} at {door.transform.position}, dist={dist}");
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestDoor = door;
                    }
                }
            }
            Debug.Log($"[RoomGen] Checked doors: {checkedDoors}, Closest: {(closestDoor != null ? closestDoor.gameObject.name : "null")}");
            return closestDoor != null ? closestDoor.gameObject : null;
        }




        private void UpdateRooms()
        {
            if (rooms.Count > 4)
                DeleteRoom();
        }

        // Псевдокод:
        // 1. После спавна RoomPartObject (roomPart) для каждой части комнаты, создать копию пола как потолок.
        // 2. Потолок должен быть размещён на той же позиции, что и пол, но сдвинут вверх на высоту комнаты.
        // 3. Потолок можно развернуть на 180 градусов по оси X (или Z), если нужно "перевернуть" меш.
        // 4. Потолок сделать дочерним roomPart для удобства управления.


        private void SpawnRoom(bool initial = false)
        {
            roomCounter++;

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

            //DirectionType? doorDirection = null; ---------------------------------------------------------------------------------------------------
            List<RoomPartObject> doorCandidates = new List<RoomPartObject>();
            List<Tuple<RoomPartObject, DirectionType>> possibleDoors = new List<Tuple<RoomPartObject, DirectionType>>();

            foreach (var partPoint in PartPoints)
            {
                if (partPoint.Point == null) continue;
                Vector3 partPos = spawnPos + partPoint.Point.localPosition;
                var roomPart = Instantiate(RoomPartObjectPrefab, partPos, Quaternion.identity, roomContainer.transform);
                newRoom.RoomParts.Add(roomPart);

                // --- СПАВН ПОТОЛКА ---
                // Вместо Plane: создаём потолок так же, как пол, но на высоте CeilingHeight
                //var floor = roomPart.transform.Find("Floor");
                //if (floor != null)
                //{
                //    var ceiling = Instantiate(floor.gameObject, floor.position + Vector3.up * CeilingHeight, floor.rotation, roomPart.transform);
                //    ceiling.name = "Ceiling";
                //    // Устанавливаем физический слой "player"
                //    ceiling.layer = LayerMask.NameToLayer("Player");
                //}
                // --- КОНЕЦ СПАВНА ПОТОЛКА ---

                foreach (var outDir in partPoint.OutsideDirections)
                {
                    if (forcedEntranceDir.HasValue && outDir == forcedEntranceDir.Value)
                        continue;

                    if (outDir == nextSpawnDirection)
                    {
                        possibleDoors.Add(Tuple.Create(roomPart, outDir));
                    }
                }
            }

            Tuple<RoomPartObject, DirectionType> selectedDoor = null;
            if (possibleDoors.Count > 0)
            {
                int idx = UnityEngine.Random.Range(0, possibleDoors.Count);
                selectedDoor = possibleDoors[idx];
            }

            int partIdx = 0;
            foreach (var partPoint in PartPoints)
            {
                if (partPoint.Point == null) continue;
                var roomPart = newRoom.RoomParts[partIdx];
                partIdx++;

                foreach (var outDir in partPoint.OutsideDirections)
                {
                    if (forcedEntranceDir.HasValue && outDir == forcedEntranceDir.Value)
                        continue;

                    if (selectedDoor != null && roomPart == selectedDoor.Item1 && outDir == selectedDoor.Item2)
                    {
                        var doorObj = Instantiate(DoorPrefab);
                        roomPart.AssignWall(doorObj, outDir);
                    }
                    else
                    {
                        var wallObj = Instantiate(WallPrefab);
                        roomPart.AssignWall(wallObj, outDir);
                    }
                }
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

            if (rooms.Count >= 2)
                BakeLocalNavMeshBetweenLastTwo(150f);
        }
        private void DeleteRoom()
        {
            var old = rooms[0];

            // Закрыть дверь назад в следующей комнате
            if (rooms.Count > 1)
            {
                var nextRoom = rooms[1];
                var prevRoomPos = rooms[0].RoomContainer.transform.position;
                var backDoorObj = FindBackDoorObject(nextRoom, prevRoomPos);
                Debug.Log($"[RoomGen] Try close door in room {nextRoom.RoomContainer.name}. Found: {backDoorObj != null}");
                if (backDoorObj != null)
                {
                    var doorCtrl = backDoorObj.GetComponent<DoorController>();
                    Debug.Log($"[RoomGen] DoorController found: {doorCtrl != null}");
                    if (doorCtrl != null)
                        doorCtrl.CloseDoor();
                }
            }


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
