using UnityEngine;

namespace PriestsAndDevils
{
    public class Action : ScriptableObject
    {
        public bool enable = true;
        public bool destroy = false;
        public GameObject gameObject { get; set; }
        public Transform transform { get; set; }
        public IActionCallback callback;

        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}