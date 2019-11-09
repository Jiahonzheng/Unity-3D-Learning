using UnityEngine;

namespace Patrol
{
    public class SoldierCollider : MonoBehaviour
    {
        public void OnCollisionEnter(Collision collision)
        {
            // 当巡逻兵与玩家碰撞时。
            if (collision.gameObject.tag == "Player")
            {
                GameEventManager.GetInstance().SoldierCollideWithPlayer();
            }
        }
    }
}