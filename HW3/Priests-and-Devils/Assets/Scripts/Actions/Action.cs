using UnityEngine;

namespace PriestsAndDevils
{
    public class Action : ScriptableObject
    {
        public bool enable = true;
        // 若为 true ，表示动作已完成。
        public bool destroy = false;
        // 表示需要进行运动的游戏对象。
        public GameObject gameObject { get; set; }
        public Transform transform { get; set; }
        // 表示在动作执行完毕后，需要通知的对象。
        public IActionCallback callback;

        // 在此方法中实现动作的初始化操作。
        public virtual void Start()
        {
            // 提示用户需要实现此方法！
            throw new System.NotImplementedException();
        }

        // 在此方法中实现动作逻辑。
        public virtual void Update()
        {
            // 提示用户需要实现此方法！
            throw new System.NotImplementedException();
        }
    }
}