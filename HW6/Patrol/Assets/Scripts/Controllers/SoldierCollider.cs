using UnityEngine;

namespace Patrol
{
    public class SoldierCollider : MonoBehaviour
    {
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameEventManager.GetInstance().PlayerCollideWithPatrol();
            }
        }
    }
}