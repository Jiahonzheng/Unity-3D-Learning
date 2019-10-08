using UnityEngine;

namespace HitUFO
{
    public class UFOModel : MonoBehaviour
    {
        public int score;

        public Vector3 startPosition = new Vector3(-3, 2, -15);

        public Vector3 startSpeed = new Vector3(3, 11, 8);

        public Vector3 localScale = new Vector3(1, 0.08f, 1);

        private int leftOrRight;

        public Vector3 GetSpeed()
        {
            Vector3 v = startSpeed;
            v.x *= leftOrRight;
            return v;
        }

        public void SetSide(int lr, float dy)
        {
            Vector3 v = startPosition;
            v.x *= lr;
            v.y += dy;
            transform.position = v;
            leftOrRight = lr;
        }

        public void SetLocalScale(float x, float y, float z)
        {
            Vector3 lc = localScale;
            lc.x *= x;
            lc.y *= y;
            lc.z *= z;
            transform.localScale = lc;
        }
    }
}