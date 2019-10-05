using UnityEngine;

namespace PriestsAndDevils
{
    public interface IActionCallback
    {
        // 用于通知更高级对象动作已执行完毕。
        void ActionDone(Action action);
    }
}