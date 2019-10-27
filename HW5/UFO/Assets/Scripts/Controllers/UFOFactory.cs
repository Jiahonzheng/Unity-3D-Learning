using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class UFOFactory
    {
        // 定义飞碟颜色。
        public enum Color
        {
            Red,
            Green,
            Blue
        }

        // 单例。
        private static UFOFactory factory;
        // 维护正在使用的飞碟对象。
        private List<GameObject> inUsed = new List<GameObject>();
        // 维护未被使用的飞碟对象。
        private List<GameObject> notUsed = new List<GameObject>();
        // 空闲飞碟对象的空间位置。
        public static Vector3 invisible = new Vector3(0, -100, 0);

        // 使用单例模式。
        public static UFOFactory GetInstance()
        {
            return factory ?? (factory = new UFOFactory());
        }

        // 获取特定颜色的飞碟。
        public GameObject Get(Color color)
        {
            GameObject ufo;
            if (notUsed.Count == 0)
            {
                ufo = Object.Instantiate(Resources.Load<GameObject>("Prefabs/UFO"), invisible, Quaternion.identity);
                ufo.AddComponent<UFOModel>();
            }
            else
            {
                ufo = notUsed[0];
                notUsed.RemoveAt(0);
            }

            // 设置 Material 属性（颜色）。
            Material material = Object.Instantiate(Resources.Load<Material>("Materials/" + color.ToString("G")));
            ufo.GetComponent<MeshRenderer>().material = material;

            // 添加对象至 inUsed 列表。
            inUsed.Add(ufo);
            return ufo;
        }

        // 回收飞碟对象。
        public void Put(GameObject ufo)
        {
            // 设置飞碟对象的空间位置和刚体属性。
            var rigidbody = ufo.GetComponent<Rigidbody>();
            // 以下两行代码很关键！我们需要设置对象速度为零！
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.useGravity = false;
            ufo.transform.position = invisible;
            // 维护 inUsed 和 notUsed 列表。
            inUsed.Remove(ufo);
            notUsed.Add(ufo);
        }
    }
}