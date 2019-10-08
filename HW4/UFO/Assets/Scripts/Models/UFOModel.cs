using UnityEngine;

namespace HitUFO
{
    public class UFOModel : MonoBehaviour
    {
        // 记录当前飞碟的分数。
        public int score;
        // 记录飞碟在左边的初始位置。
        public static Vector3 startPosition = new Vector3(-3, 2, -15);
        // 记录飞碟在左边的初始速度。
        public static Vector3 startSpeed = new Vector3(3, 11, 8);
        // 记录飞碟的初始缩放比例。
        public static Vector3 localScale = new Vector3(1, 0.08f, 1);
        // 表示飞碟的位置（左边、右边）。
        private int leftOrRight;

        // 获取实际初速度。
        public Vector3 GetSpeed()
        {
            Vector3 v = startSpeed;
            v.x *= leftOrRight;
            return v;
        }

        // 设置实际初位置。
        public void SetSide(int lr, float dy)
        {
            Vector3 v = startPosition;
            v.x *= lr;
            v.y += dy;
            transform.position = v;
            leftOrRight = lr;
        }

        // 设置实际缩放比例。
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