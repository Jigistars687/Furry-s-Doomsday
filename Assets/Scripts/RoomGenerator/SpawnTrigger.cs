// SpawnTrigger.cs
using UnityEngine;
using System;

namespace RoomSystem
{
    [RequireComponent(typeof(Collider))]
    public class SpawnTrigger : MonoBehaviour
    {
        public event Action OnPlayerEnter;

        [Tooltip("—сылка на DoorController, которую нужно открыть при входе игрока")]
        //public DoorController doorController;

        private void Awake()
        {
            var col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            //// ¬нутри метода OnTriggerEnter(Collider col)
            //if (other.gameObject.TryGetComponent<playercontrollerBETA>(out var _))
            //{
            //    // ќткрыть дверь
            //    if (doorController != null)
            //        doorController.OpenDoor();
            //}

            //Debug.Log($"SpawnTrigger: OnTriggerEnter with {other.name}");
            if (!other.CompareTag("Player")) return;
            //Debug.Log("SpawnTrigger: Player entered!");
            OnPlayerEnter?.Invoke();
            OnPlayerEnter = null;
        }
    }
}