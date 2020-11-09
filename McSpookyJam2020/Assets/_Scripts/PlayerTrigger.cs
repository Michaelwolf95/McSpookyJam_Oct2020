using System;
using UnityEngine;

namespace MichaelWolfGames
{
    public class PlayerTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            bool trigger = (other.gameObject == PlayerInstance.Instance.gameObject);
            if (!trigger && other.attachedRigidbody != null)
            {
                trigger = (other.attachedRigidbody.gameObject == PlayerInstance.Instance.gameObject);
            }
            if (trigger)
            {
                gameObject.SetActive(false);
            }
        }
    }
}