using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class UFOFactory
    {
        public enum Color
        {
            Red,
            Green,
            Blue
        }

        private static UFOFactory factory;
        private List<GameObject> inUsed = new List<GameObject>();
        private List<GameObject> notUsed = new List<GameObject>();
        private readonly Vector3 invisible = new Vector3(0,   -100, 0);

        public static UFOFactory GetInstance()
        {
            return factory ?? (factory = new UFOFactory());
        }

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

            Material material = Object.Instantiate(Resources.Load<Material>("Materials/" + color.ToString("G")));
            ufo.GetComponent<MeshRenderer>().material = material;

            var rigidbody = ufo.GetComponent<Rigidbody>();
            rigidbody.WakeUp();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            inUsed.Add(ufo);
            return ufo;
        }

        public void Put(GameObject ufo)
        {
            var rigidbody = ufo.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.useGravity = false;
            ufo.transform.position = invisible;

            inUsed.Remove(ufo);
            notUsed.Add(ufo);
        }
    }
}