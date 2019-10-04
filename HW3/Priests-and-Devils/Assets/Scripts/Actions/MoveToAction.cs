using UnityEngine;

namespace PriestsAndDevils
{
    public class MoveToAction : Action
    {
        public Vector3 destination;
        public float speed;

        public static MoveToAction GetAction(GameObject gameObject, IActionCallback callback, Vector3 destination, float speed)
        {
            MoveToAction action = CreateInstance<MoveToAction>();
            action.gameObject = gameObject;
            action.transform = gameObject.transform;
            action.callback = callback;
            action.destination = destination;
            action.speed = speed;
            return action;
        }

        public override void Start() { }

        public override void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            if (transform.position == destination)
            {
                destroy = true;
                callback?.ActionDone(this);
            }
        }
    }
}