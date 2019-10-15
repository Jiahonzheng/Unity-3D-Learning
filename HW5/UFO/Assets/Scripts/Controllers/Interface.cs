using UnityEngine;

namespace HitUFO
{
    public interface ISceneController
    {
        void LoadResources();
    }

    public interface IActionManager
    {
        void SetAction(GameObject ufo);
    }
}