using System;
using UnityEngine;
using UnityEngine.Events;

namespace GibFrame
{
    public class CollisionBroadcaster : MonoBehaviour
    {
        [SerializeField] private ColliderEvent OnTriggerEnterBroadcast;
        [SerializeField] private ColliderEvent OnTriggerStayBroadcast;
        [SerializeField] private ColliderEvent OnTriggerExitBroadcast;
        [SerializeField] private CollisionEvent OnCollisionEnterBroadcast;
        [SerializeField] private CollisionEvent OnCollisionStayBroadcast;
        [SerializeField] private CollisionEvent OnCollisionExitBroadcast;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterBroadcast?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayBroadcast?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitBroadcast?.Invoke(other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterBroadcast?.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            OnCollisionStayBroadcast?.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnCollisionExitBroadcast?.Invoke(collision);
        }

        [Serializable]
        private class ColliderEvent : UnityEvent<Collider>
        {
        }

        [Serializable]
        private class CollisionEvent : UnityEvent<Collision>
        {
        }
    }
}