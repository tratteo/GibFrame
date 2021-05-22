using System;
using UnityEngine;
using UnityEngine.Events;

namespace GibFrame
{
    public class CollisionBroadcaster2D : MonoBehaviour
    {
        [SerializeField] private ColliderEvent2D OnTriggerEnterBroadcast2D;
        [SerializeField] private ColliderEvent2D OnTriggerStayBroadcast2D;
        [SerializeField] private ColliderEvent2D OnTriggerExitBroadcast2D;
        [SerializeField] private CollisionEvent2D OnCollisionEnterBroadcast2D;
        [SerializeField] private CollisionEvent2D OnCollisionStayBroadcast2D;
        [SerializeField] private CollisionEvent2D OnCollisionExitBroadcast2D;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEnterBroadcast2D?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            OnTriggerStayBroadcast2D?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnTriggerExitBroadcast2D?.Invoke(other);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEnterBroadcast2D?.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            OnCollisionStayBroadcast2D?.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            OnCollisionExitBroadcast2D?.Invoke(collision);
        }

        [Serializable]
        private class ColliderEvent2D : UnityEvent<Collider2D>
        {
        }

        [Serializable]
        private class CollisionEvent2D : UnityEvent<Collision2D>
        {
        }
    }
}
