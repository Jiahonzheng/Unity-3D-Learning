using UnityEngine;

namespace Patrol
{
    public class Patrol : MonoBehaviour
    {
        public int area;
        public bool isFollowing = false;

        void Start()
        {
            var rigidbody = gameObject.GetComponent<Rigidbody>();
            if (rigidbody)
            {
                rigidbody.freezeRotation = true;
            }
        }

        void Update()
        {
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