using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public class GameController : MonoBehaviour, ISceneController
    {
        void Awake()
        {
            Director.GetInstance().OnSceneWake(this);
        }

        public void LoadResources()
        {
            GameObject map = Instantiate(Resources.Load<GameObject>("Prefabs/Map"));
            GameObject fence = Instantiate(Resources.Load<GameObject>("Prefabs/Fence"));
            fence.transform.parent = map.transform;
        }
    }
}
