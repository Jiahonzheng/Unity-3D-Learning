using UnityEngine;

namespace PriestsAndDevils
{
    public class Moveable : MonoBehaviour
    {
        public float speed = 20;
        private int status;
        private Vector3 destination;
        private Vector3 middle;

        public void SetDestination(Vector3 position)
        {
            destination = position;
            middle = position;
            if (System.Math.Abs(position.y - transform.position.y) < 0.00001)
            {
                status = 2;
            }
            else if (position.y < transform.position.y)
            {
                middle.y = transform.position.y;
            }
            else
            {
                middle.x = transform.position.x;
            }
            status = 1;
        }

        void Update()
        {
            if (status == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, middle, speed * Time.deltaTime);
                if (transform.position == middle)
                {
                    status = 2;
                }
            }
            if (status == 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                if (transform.position == destination)
                {
                    status = 0;
                }
            }
        }

        public void Reset()
        {
            status = 0;
        }
    }
}