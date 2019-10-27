using System;
using System.Collections.Generic;

namespace Patrol
{
    public class GameEventManager
    {
        // Singleton instance.
        private static GameEventManager instance;

        public delegate void OnPlayerEnterArea(int area);
        public static event OnPlayerEnterArea onPlayerEnterArea;

        public delegate void OnPlayerCollideWithPatrol();
        public static event OnPlayerCollideWithPatrol onPlayerCollideWithPatrol;

        public static GameEventManager GetInstance()
        {
            return instance ?? (instance = new GameEventManager());
        }

        public void PlayerEnterArea(int area)
        {
            onPlayerEnterArea?.Invoke(area);
        }

        public void PlayerCollideWithPatrol()
        {
            onPlayerCollideWithPatrol?.Invoke();
        }
    }
}