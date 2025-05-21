// SpawnTrigger.cs
using UnityEngine;
using System;

namespace RoomSystem
{
    [RequireComponent(typeof(Collider))]
    public class SpawnTrigger : MonoBehaviour
    {
        public event Action OnPlayerEnter;

        private void Awake()
        {
            var col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"SpawnTrigger: OnTriggerEnter with {other.name}");
            if (!other.CompareTag("Player")) return;
            //Debug.Log("SpawnTrigger: Player entered!");
            OnPlayerEnter?.Invoke();
            OnPlayerEnter = null;
        }
    }
}