using UnityEngine;

namespace HitUFO
{
    public class PhysicActionManager : IActionManager
    {
        public void SetAction(GameObject ufo)
        {
            var model = ufo.GetComponent<UFOModel>();
            var rigidbody = ufo.GetComponent<Rigidbody>();
            // 对物体添加 Impulse 力。
            rigidbody.AddForce(0.2f * model.GetSpeed(), ForceMode.Impulse);
            rigidbody.useGravity = true;
        }
    }
}