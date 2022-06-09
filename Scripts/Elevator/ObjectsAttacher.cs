using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class ObjectsAttacher : MonoBehaviour
    {
        [SerializeField] LayerMask collidableLayer;

        private void OnTriggerEnter(Collider collider)
        {
            if (IsObjectsMaskCollidable(collider.gameObject.layer)) collider.transform.parent = transform;
        }

        private void OnTriggerExit(Collider collider)
        {
            if (IsObjectsMaskCollidable(collider.gameObject.layer)) collider.transform.parent = null;
        }

        private bool IsObjectsMaskCollidable(int objectsLayer) => (collidableLayer.value & (1 << objectsLayer)) > 0;
    }
}
