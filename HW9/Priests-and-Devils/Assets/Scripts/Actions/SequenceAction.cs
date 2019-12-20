using System.Collections.Generic;
using UnityEngine;

namespace PriestsAndDevils
{
    public class SequenceAction : Action, IActionCallback
    {
        // 用于存储多个顺序执行的动作。
        public List<Action> sequence;
        // 指明动作执行次数，若为负数则表示该动作重复执行。
        public int repeat = 1;
        // 表示当前进行的动作。
        public int currentActionIndex = 0;

        // 创建 SequenceAction 。
        public static SequenceAction GetAction(IActionCallback callback, List<Action> sequence, int repeat = 1, int currentActionIndex = 0)
        {
            SequenceAction action = CreateInstance<SequenceAction>();
            action.callback = callback;
            action.sequence = sequence;
            action.repeat = repeat;
            action.currentActionIndex = currentActionIndex;
            return action;
        }

        // 设置每个子动作的 callback ，使得子动作完成时，SequenceAction 可切换至下一动作。
        public override void Start()
        {
            foreach (Action action in sequence)
            {
                action.callback = this;
                action.Start();
            }
        }

        // 执行子动作。
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

        // 子动作完成时的钩子函数，用于切换下一子动作。
        public void ActionDone(Action action)
        {
            action.destroy = false;
            currentActionIndex++;
            if (currentActionIndex >= sequence.Count)
            {
                currentActionIndex = 0;
                // 判断是否需要重复执行。
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

        // 响应 Object 被销毁的事件。
        void OnDestroy()
        {
            foreach (Action action in sequence)
            {
                Destroy(action);
            }
        }
    }
}