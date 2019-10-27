using System;
using UnityEngine;

namespace Archery
{
    public class ArrowHitObjectEvent : EventArgs
    {
        public GameObject arrow;
        public int target;

        public ArrowHitObjectEvent(GameObject arrow, int target)
        {
            this.arrow = arrow;
            this.target = target;
        }
    }

    public class ArrowCollider : MonoBehaviour
    {
        public EventHandler<ArrowHitObjectEvent> onArrowHitObject;
        public bool isHitTarget = true;

        public void Reset()
        {
            isHitTarget = true;
        }

        void OnTriggerEnter(Collider other)
        {
            var otherObject = other.gameObject;
            var arrow = gameObject.transform.parent.gameObject;
            // 当箭击中箭靶时。
            if (otherObject.tag == "target")
            {
                arrow.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                int target = int.Parse(otherObject.name);
                isHitTarget = true;
                onArrowHitObject.Invoke(this, new ArrowHitObjectEvent(arrow, target));
            }
            else // 当箭击中其他物体时。
            {
                isHitTarget = false;
                onArrowHitObject.Invoke(this, new ArrowHitObjectEvent(arrow, 0));
            }
        }
    }
}