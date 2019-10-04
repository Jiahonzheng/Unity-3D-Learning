using System.Collections.Generic;
using UnityEngine;

namespace PriestsAndDevils
{
    public class SequenceAction : Action, IActionCallback
    {
        public List<Action> sequence;
        public int repeat = 1;
        public int currentActionIndex = 0;

        public static SequenceAction GetAction(IActionCallback callback, List<Action> sequence, int repeat = 1, int currentActionIndex = 0)
        {
            SequenceAction action = CreateInstance<SequenceAction>();
            action.callback = callback;
            action.sequence = sequence;
            action.repeat = repeat;
            action.currentActionIndex = currentActionIndex;
            return action;
        }

        public override void Start()
        {
            foreach (Action action in sequence)
            {
                action.callback = this;
                action.Start();
            }
        }

        public override void Update()
        {
            if (sequence.Count == 0)
            {
                return;
            }
            if (currentActionIndex < sequence.Count)
            {
                sequence[currentActionIndex].Update();
            }
        }

        public void ActionDone(Action action)
        {
            action.destroy = false;
            currentActionIndex++;
            if (currentActionIndex >= sequence.Count)
            {
                currentActionIndex = 0;
                if (repeat > 0)
                {
                    repeat--;
                }
                if (repeat == 0)
                {
                    destroy = true;
                    callback?.ActionDone(this);
                }
            }
        }

        void OnDestroy()
        {
            foreach (Action action in sequence)
            {
                Destroy(action);
            }
        }
    }
}