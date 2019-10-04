using System.Collections.Generic;
using UnityEngine;

namespace PriestsAndDevils
{
    public class ActionManager : MonoBehaviour, IActionCallback
    {
        private Dictionary<int, Action> actions = new Dictionary<int, Action>();
        private List<Action> waitToAdd = new List<Action>();
        private List<int> waitToDelete = new List<int>();

        protected void Update()
        {
            foreach (Action action in waitToAdd)
            {
                actions[action.GetInstanceID()] = action;
            }
            waitToAdd.Clear();

            foreach (KeyValuePair<int, Action> kv in actions)
            {
                Action action = kv.Value;
                if (action.destroy)
                {
                    waitToDelete.Add(action.GetInstanceID());
                }
                else if (action.enable)
                {
                    action.Update();
                }
            }

            foreach (int k in waitToDelete)
            {
                Action action = actions[k];
                actions.Remove(k);
                Destroy(action);
            }
            waitToDelete.Clear();
        }

        public void AddAction(Action action)
        {
            waitToAdd.Add(action);
            action.Start();
        }

        public void ActionDone(Action action) { }
    }
}