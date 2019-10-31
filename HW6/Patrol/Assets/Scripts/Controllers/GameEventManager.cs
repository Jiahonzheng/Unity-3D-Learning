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

        // 使用单例模式。
        public static GameEventManager GetInstance()
        {
            return instance ?? (instance = new GameEventManager());
        }

        // 当玩家进入区域。
        public void PlayerEnterArea(int area)
        {
            onPlayerEnterArea?.Invoke(area);
        }

        // 当巡逻兵与玩家碰撞。
        public void SoldierCollideWithPlayer()
        {
            onSoldierCollideWithPlayer?.Invoke();
        }
    }
}