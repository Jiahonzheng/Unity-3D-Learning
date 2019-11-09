using UnityEngine;

namespace Patrol
{
    public class Soldier : MonoBehaviour
    {
        // 记录所处的区域号。
        public int area;
        public bool isFollowing = false;

        void Awake()
        {
            gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        }

        void Update()
        {
            // 抑制碰撞造成的旋转。
            if (this.gameObject.transform.localEulerAngles.x != 0 || gameObject.transform.localEulerAngles.z != 0)
            {
                gameObject.transform.localEulerAngles = new Vector3(0, gameObject.transform.localEulerAngles.y, 0);
            }
            if (gameObject.transform.position.y != 0)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            }
        }
    }
}