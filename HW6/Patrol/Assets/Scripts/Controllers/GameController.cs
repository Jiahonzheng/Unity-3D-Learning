using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public class GameController : MonoBehaviour, ISceneController
    {
        public GameModel model = new GameModel();

        private GameActionManager actionManager;
        private GameObject player;
        private List<GameObject> soldiers = new List<GameObject>();
        private int currentArea = 4;

        void Awake()
        {
            actionManager = gameObject.AddComponent<GameActionManager>();
            Director.GetInstance().OnSceneWake(this);
        }

        void Update()
        {
            if (model.state == GameState.RUNNING)
            {
                var dx = Input.GetAxis("Horizontal");
                var dz = Input.GetAxis("Vertical");
                if (dx != 0 || dz != 0)
                {
                    player.GetComponent<Animator>().SetBool("isRunning", true);
                }
                else
                {
                    player.GetComponent<Animator>().SetBool("isRunning", false);
                }
                player.transform.Translate(0, 0, dz * 4f * Time.deltaTime);
                player.transform.Rotate(0, dx * 50f * Time.deltaTime, 0);
            }
        }

        void OnEnable()
        {
            GameEventManager.onPlayerEnterArea += OnPlayerEnterArea;
            GameEventManager.onPlayerCollideWithPatrol += OnPlayerCollideWithPatrol;
        }

        public void LoadResources()
        {
            Map.LoadPlane();
            Map.LoadWalls();
            Map.LoadFences();
            Map.LoadAreaColliders();
            LoadSoldiers();
            LoadPlayer();
            Restart();
        }

        public void Restart()
        {
            currentArea = 4;
            model.state = GameState.RUNNING;
            player.transform.position = new Vector3(-4.5f, 0, -4.5f);
            player.GetComponent<Animator>().Play("Initial State");
            foreach (var p in soldiers)
            {
                if (p.GetComponent<Patrol>().area != currentArea)
                {
                    p.GetComponent<Patrol>().isFollowing = false;
                    actionManager.GoAround(p);
                }
                else
                {
                    p.GetComponent<Patrol>().isFollowing = true;
                    actionManager.Trace(p, player);
                }
            }
        }

        private void OnPlayerEnterArea(int area)
        {
            if (currentArea != area)
            {
                soldiers[currentArea].GetComponent<Patrol>().isFollowing = false;
                currentArea = area;
                soldiers[currentArea].GetComponent<Patrol>().isFollowing = true;
                actionManager.Trace(soldiers[currentArea], player);
            }
        }

        private void OnPlayerCollideWithPatrol()
        {
            model.state = GameState.LOSE;
            soldiers[currentArea].GetComponent<Patrol>().isFollowing = false;
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.GetComponent<Animator>().SetTrigger("isDead");
        }

        private void LoadSoldiers()
        {
            GameObject soldierPrefab = Resources.Load<GameObject>("Prefabs/Soldier");
            for (int i = 0; i < 9; ++i)
            {
                GameObject soldier = Instantiate(soldierPrefab);
                soldier.AddComponent<Patrol>().area = i;
                soldier.GetComponent<Animator>().SetBool("isRunning", true);
                soldier.GetComponent<Rigidbody>().freezeRotation = true;
                soldier.AddComponent<SoldierCollider>();
                soldier.name = "Soldier" + i;
                soldier.transform.position = Map.center[i];
                soldiers.Add(soldier);
            }
        }

        private void LoadPlayer()
        {
            player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            player.GetComponent<Rigidbody>().freezeRotation = true;
        }

        private void ResetSoldiers()
        {
            for (int i = 0; i < 9; ++i)
            {
                soldiers[i].transform.position = Map.center[i];
            }
        }
    }
}