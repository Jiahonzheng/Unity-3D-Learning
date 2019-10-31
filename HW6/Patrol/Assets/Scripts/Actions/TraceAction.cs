using UnityEngine;

namespace Patrol
{
    public class TraceAction : Action
    {
        public GameObject target;
        public float speed;

        public static TraceAction GetAction(GameObject gameObject, IActionCallback callback, GameObject target, float speed)
        {
            TraceAction action = CreateInstance<TraceAction>();
            action.gameObject = gameObject;
            action.transform = gameObject.transform;
            action.callback = callback;
            action.target = target;
            action.speed = speed;
            return action;
        }

        public override void Start() {}

        public override void Update()
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, 1.5f * speed * Time.deltaTime);
            if (gameObject.GetComponent<Soldier>().isFollowing == false || (gameObject.transform.position - target.transform.position).sqrMagnitude < 0.00001f)
            {
                destroy = true;
                callback.ActionDone(this);
            }
            else
            {
                Quaternion rotation = Quaternion.LookRotation(target.transform.position - gameObject.transform.position, Vector3.up);
                gameObject.transform.rotation = rotation;
            }
        }
    }
}