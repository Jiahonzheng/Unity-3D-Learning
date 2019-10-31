using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public class GameActionManager : ActionManager, IActionCallback
    {
        private int currentArea = -1;
        Dictionary<int, MoveToAction> moveToActions = new Dictionary<int, MoveToAction>();

        // 巡逻兵追随玩家。
        public void Trace(GameObject patrol, GameObject player)
        {
            var area = patrol.GetComponent<Soldier>().area;
            // 防止重入。
            if (area == currentArea)
            {
                return;
            }
            currentArea = area;
            if (moveToActions.ContainsKey(area))
            {
                moveToActions[area].destroy = true;
            }
            TraceAction action = TraceAction.GetAction(patrol, this, player, 1.5f);
            AddAction(action);
        }

        // 巡逻兵自主巡逻。
        public void GoAround(GameObject patrol)
        {
            var area = patrol.GetComponent<Soldier>().area;
            if (moveToActions.ContainsKey(area))
            {
                return;
            }
            var target = GetGoAroundTarget(patrol);
            MoveToAction action = MoveToAction.GetAction(patrol, this, target, 1.5f, area);
            moveToActions.Add(area, action);
            AddAction(action);
        }

        public void Stop()
        {
            foreach (var x in moveToActions.Values)
            {
                x.destroy = true;
            }
            moveToActions.Clear();
            currentArea = -1;
        }

        public new void ActionDone(Action action)
        {
            var area = action.gameObject.GetComponent<Soldier>().area;
            if (moveToActions.ContainsKey(area))
            {
                moveToActions.Remove(area);
            }
        }

        private Vector3 GetGoAroundTarget(GameObject patrol)
        {
            Vector3 pos = patrol.transform.position;
            var area = patrol.GetComponent<Soldier>().area;
            // 计算当前区域的边界。
            float x_down = -15 + (area % 3) * 10;
            float x_up = x_down + 10;
            float z_down = -15 + (area / 3) * 10;
            float z_up = z_down + 10;
            // 随机生成运动。
            var move = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
            var next = pos + move;
            int tryCount = 0;
            // 边界判断。
            while (!(next.x > x_down + 0.1f && next.x < x_up - 0.1f && next.z > z_down + 0.1f && next.z < z_up - 0.1f) || next == pos)
            {
                move = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
                next = pos + move;
                // 当无法获取到符合要求的 target 时，抛出异常。
                if ((++tryCount) > 100)
                {
                    Debug.LogFormat("point {0}, area({1}, {2}, {3}, {4}, {5})", pos, area, x_down, x_up, z_down, z_up);
                    throw new System.Exception("Too many loops for finding a target");
                }
            }
            return next;
        }
    }
}