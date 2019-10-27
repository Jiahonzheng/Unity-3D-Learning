using UnityEngine;

namespace Patrol
{
    public class MoveToAction : Action
    {
        public Vector3 target;
        public float speed;
        public int area;

        public static MoveToAction GetAction(GameObject gameObject, IActionCallback callback, Vector3 target, float speed, int area)
        {
            MoveToAction action = CreateInstance<MoveToAction>();
            action.gameObject = gameObject;
            action.transform = gameObject.transform;
            action.callback = callback;
            action.target = target;
            action.speed = speed;
            action.area = area;
            return action;
        }

        public override void Start()
        {
            Quaternion rotation = Quaternion.LookRotation(target - transform.position, Vector3.up);
            transform.rotation = rotation;
        }

        public override void Update()
        {
            if ((transform.position - target).magnitude < 0.001f)
            {
                destroy = true;
                callback.ActionDone(this);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
        }
    }
}