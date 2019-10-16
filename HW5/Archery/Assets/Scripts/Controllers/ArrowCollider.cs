using System;
using UnityEngine;

namespace Archery
{
    public class ArrowHitTargetEvent : EventArgs
    {
        public GameObject arrow;
        public int target;

        public ArrowHitTargetEvent(GameObject arrow, int target)
        {
            this.arrow = arrow;
            this.target = target;
        }
    }

    public class ArrowCollider : MonoBehaviour
    {
        public EventHandler<ArrowHitTargetEvent> onArrowHitTarget;
        public bool isHitTarget = true;

        public void Reset()
        {
            isHitTarget = true;
        }

        void OnTriggerEnter(Collider other)
        {
            var otherObject = other.gameObject;
            var arrow = gameObject.transform.parent.gameObject;
            if (otherObject.tag == "target")
            {
                arrow.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                int target = otherObject.name[other.gameObject.name.Length - 1] - '0';
                isHitTarget = true;
                onArrowHitTarget.Invoke(this, new ArrowHitTargetEvent(arrow, target));
            } else
            {
                isHitTarget = false;
                onArrowHitTarget.Invoke(this, new ArrowHitTargetEvent(arrow, 0));
            }
        }
    }
}