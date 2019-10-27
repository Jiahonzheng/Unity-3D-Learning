using UnityEngine;

namespace Patrol
{
    public class AreaCollider : MonoBehaviour
    {
        // 记录当前区域号。
        public int area;

        public void OnTriggerEnter(Collider collider)
        {
            // 当玩家进入区域时。
            if (collider.gameObject.tag == "Player")
            {
                GameEventManager.GetInstance().PlayerEnterArea(area);
            }
        }
    }
}