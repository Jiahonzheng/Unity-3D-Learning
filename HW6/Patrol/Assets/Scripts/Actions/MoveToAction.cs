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
            // ±‹√‚µØ≥ˆ Zero Ã· æ°£
            if (target - gameObject.transform.position == Vector3.zero)
            {
                return;
            }
            Quaternion rotation = Quaternion.LookRotation(target - gameObject.transform.position, Vector3.up);
            gameObject.transform.rotation = rotation;
        }

        public override void Update()
        {
            if ((gameObject.transform.position - target).magnitude < 0.001f)
            {
                destroy = true;
                callback.ActionDone(this);
            }
            else
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, speed * Time.deltaTime);
            }
        }
    }
}