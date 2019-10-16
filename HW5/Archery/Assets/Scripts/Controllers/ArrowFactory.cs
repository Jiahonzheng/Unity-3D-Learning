using System.Collections.Generic;
using UnityEngine;

namespace Archery
{
    public class ArrowFactory
    {
        // 单例。
        private static ArrowFactory factory;
        // 维护正在使用的箭对象。
        private List<GameObject> inUsed = new List<GameObject>();
        // 维护未被使用的箭对象。
        private List<GameObject> notUsed = new List<GameObject>();
        // 空闲箭的空间位置。
        public static Vector3 INITIAL_POSITION = new Vector3(0, 0, -19);

        // 使用单例模式。
        public static ArrowFactory GetInstance()
        {
            return factory ?? (factory = new ArrowFactory());
        }

        // 获取一支箭。
        public GameObject Get()
        {
            GameObject arrow;
            if (notUsed.Count == 0)
            {
                arrow = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"));
                inUsed.Add(arrow);
            }
            else
            {
                arrow = notUsed[0];
                notUsed.RemoveAt(0);
                arrow.SetActive(true);
                inUsed.Add(arrow);
            }
            return arrow;
        }
        
        // 回收一支箭。
        public void Put(GameObject arrow)
        {
            arrow.GetComponent<Rigidbody>().isKinematic = true;
            arrow.SetActive(false);
            arrow.transform.position = INITIAL_POSITION;
            notUsed.Add(arrow);
            inUsed.Remove(arrow);
        }
    }
}