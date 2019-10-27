namespace Patrol
{
    public class GameEventManager
    {
        // Singleton instance.
        private static GameEventManager instance;

        public delegate void OnPlayerEnterArea(int area);
        public static event OnPlayerEnterArea onPlayerEnterArea;

        public delegate void OnSoldierCollideWithPlayer();
        public static event OnSoldierCollideWithPlayer onSoldierCollideWithPlayer;

        public static GameEventManager GetInstance()
        {
            return instance ?? (instance = new GameEventManager());
        }

        public void PlayerEnterArea(int area)
        {
            onPlayerEnterArea?.Invoke(area);
        }

        public void SoldierCollideWithPlayer()
        {
            onSoldierCollideWithPlayer?.Invoke();
        }
    }
}