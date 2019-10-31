using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public interface IUserAction
    {
        void Restart();
    }

    public class GameController : MonoBehaviour, ISceneController, IUserAction
    {
        public GameModel model = new GameModel();
        public GameGUI view;

        private GameEventManager eventManager = GameEventManager.GetInstance();
        private GameActionManager actionManager;
        private GameObject player;
        private List<GameObject> soldiers = new List<GameObject>();
        private int currentArea = 4;

        void Awake()
        {
            model.onRefresh += delegate
            {
                view.state = model.state;
                view.score = model.score;
            };
            view = gameObject.AddComponent<GameGUI>();
            actionManager = gameObject.AddComponent<GameActionManager>();
            // 设置游戏事件及其处理函数。
            GameEventManager.onPlayerEnterArea += OnPlayerEnterArea;
            GameEventManager.onSoldierCollideWithPlayer += OnSoldierCollideWithPlayer;
            Director.GetInstance().OnSceneWake(this);
        }

        void Update()
        {
            if (model.state == GameState.RUNNING)
            {
                var dx = Input.GetAxis("Horizontal");
                var dz = Input.GetAxis("Vertical");
                // 设置人物的运动动画。
                if (dx != 0 || dz != 0)
                {
                    player.GetComponent<Animator>().SetBool("isRunning", true);
                }
                else
                {
                    player.GetComponent<Animator>().SetBool("isRunning", false);
                }
                // 移动人物。
                player.transform.Translate(0, 0, dz * 4f * Time.deltaTime);
                // 转动人物。
                player.transform.Rotate(0, dx * 80f * Time.deltaTime, 0);

                // 设置巡逻兵动作类型。
                for (int i = 0; i < 9; ++i)
                {
                    // 不在当前区域的巡逻兵进行自主巡逻。
                    if (i != currentArea)
                    {
                        actionManager.GoAround(soldiers[i]);
                    }
                    else // 在当前区域的巡逻兵对玩家进行追随。
                    {
                        soldiers[i].GetComponent<Soldier>().isFollowing = true;
                        actionManager.Trace(soldiers[i], player);
                    }
                }
            }
        }

        public void LoadResources()
        {
            // 构造平面。
            Map.LoadPlane();
            // 构造边界篱笆。
            Map.LoadBoundaries();
            // 构造内部篱笆。
            Map.LoadFences();
            // 构造区域Collider 。
            Map.LoadAreaColliders();
            // 构造巡逻兵。
            LoadSoldiers();
            // 构造玩家。
            LoadPlayer();
        }

        public void Restart()
        {
            // 重置 Model 。
            model.Reset();
            // 重置初始玩家区域。
            currentArea = 4;
            // 重置玩家位置和动画状态。
            player.transform.position = new Vector3(-4.5f, 0, -4.5f);
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            player.GetComponent<Animator>().Play("Initial State");
            // 重置巡逻兵位置和动画状态。
            for (int i = 0; i < 9; ++i)
            {
                soldiers[i].GetComponent<Animator>().Play("Initial State");
                soldiers[i].GetComponent<Animator>().SetBool("isRunning", true);
                soldiers[i].transform.position = Map.center[i];
                soldiers[i].GetComponent<Soldier>().isFollowing = false;
                actionManager.GoAround(soldiers[i]);
            }
        }

        // 当玩家摆脱一位巡逻兵，进入新区域时。
        private void OnPlayerEnterArea(int area)
        {
            if (model.state != GameState.RUNNING)
            {
                return;
            }
            if (currentArea != area)
            {
                // 更新分数。
                model.AddScore(1);
                soldiers[currentArea].GetComponent<Soldier>().isFollowing = false;
                currentArea = area;
                soldiers[currentArea].GetComponent<Soldier>().isFollowing = true;
                actionManager.Trace(soldiers[currentArea], player);
            }
        }

        // 当巡逻兵与玩家碰撞时。
        private void OnSoldierCollideWithPlayer()
        {
            view.state = model.state = GameState.LOSE;
            player.GetComponent<Animator>().SetTrigger("isDead");
            player.GetComponent<Rigidbody>().isKinematic = true;
            soldiers[currentArea].GetComponent<Soldier>().isFollowing = false;
            actionManager.Stop();
            for (int i = 0; i < 9; ++i)
            {
                soldiers[i].GetComponent<Animator>().SetBool("isRunning", false);
            }
        }

        // 构造巡逻兵。
        private void LoadSoldiers()
        {
            GameObject soldierPrefab = Resources.Load<GameObject>("Prefabs/Soldier");
            for (int i = 0; i < 9; ++i)
            {
                GameObject soldier = Instantiate(soldierPrefab);
                soldier.AddComponent<Soldier>().area = i;
                soldier.GetComponent<Rigidbody>().freezeRotation = true;
                soldier.AddComponent<SoldierCollider>();
                soldier.name = "Soldier" + i;
                soldier.transform.position = Map.center[i];
                soldiers.Add(soldier);
            }
        }

        // 构造玩家。
        private void LoadPlayer()
        {
            player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            player.GetComponent<Rigidbody>().freezeRotation = true;
            player.transform.position = new Vector3(-4.5f, 0, -4.5f);
        }
    }
}