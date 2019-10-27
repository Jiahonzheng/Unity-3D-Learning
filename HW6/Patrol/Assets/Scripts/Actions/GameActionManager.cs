using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public class GameActionManager : ActionManager, IActionCallback
    {
        Dictionary<int, MoveToAction> moveToActions = new Dictionary<int, MoveToAction>();

        public void Trace(GameObject patrol, GameObject player)
        {
            var area = patrol.GetComponent<Patrol>().area;
            if (moveToActions.ContainsKey(area))
            {
                moveToActions[area].destroy = true;
            }
            TraceAction action = TraceAction.GetAction(patrol, this, player, 0.8f);
            AddAction(action);
        }

        public void GoAround(GameObject patrol)
        {
            var target = GetGoAroundTarget(patrol);
            var area = patrol.GetComponent<Patrol>().area;
            MoveToAction action = MoveToAction.GetAction(patrol, this, target, 0.6f, area);
            AddAction(action);
        }

        public void ParticleSystemStopAction()
        {
            foreach (var x in moveToActions.Values)
            {
                x.destroy = true;
            }
            moveToActions.Clear();
        }

        public new void ActionDone(Action action)
        {
            var area = action.gameObject.GetComponent<Patrol>().area;
            if (moveToActions.ContainsKey(area))
            {
                moveToActions.Remove(area);
            }
        }

        private Vector3 GetGoAroundTarget(GameObject patrol)
        {
            Vector3 pos = patrol.transform.position;
            var area = patrol.GetComponent<Patrol>().area;
            float x_down = -12.5f + (area % 3) * 10;
            float x_up = x_down + 10;
            float z_down = -15 + (area / 3) * 10;
            float z_up = z_down + 10;
            var move = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            var next = pos + move;
            while (!(next.x > x_down && next.x < x_up && next.z > z_down && next.z < z_up))
            {
                move = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
                next = pos + move;
            }
            return next;
        }
    }
}