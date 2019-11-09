using UnityEngine;

namespace Smoke
{
    public class CarCollider : MonoBehaviour
    {
        // 记录车辆损坏情况。
        private float damage = 0;

        // 返回车辆损坏情况。
        public float GetDamage()
        {
            return damage;
        }

        // 设置车辆损坏情况。
        public void SetDamage(float d)
        {
            damage = d;
        }

        // 当车辆与墙体碰撞时，增加其损坏系数。
        private void OnCollisionEnter(Collision collision)
        {
            damage += 4.5f * gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        }
    }
}
