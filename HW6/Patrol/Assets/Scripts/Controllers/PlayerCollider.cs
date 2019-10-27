using UnityEngine;

namespace Patrol
{
    public class PlayerCollider : MonoBehaviour
    {
        public void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                GameEventManager.GetInstance().PlayerCollideWithPatrol();
            }
        }
    }
}