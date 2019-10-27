using UnityEngine;

namespace Patrol
{
    public class AreaCollider : MonoBehaviour
    {
        public int area;

        public void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                GameEventManager.GetInstance().PlayerEnterArea(area);
            }
        }
    }
}