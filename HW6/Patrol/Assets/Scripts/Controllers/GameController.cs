using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    public class GameController : MonoBehaviour, ISceneController
    {
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
            MovePlayer();
            if (player.transform.localEulerAngles.x != 0 || player.transform.localEulerAngles.z != 0)
            {
                player.transform.localEulerAngles = new Vector3(0, player.transform.localEulerAngles.y, 0);
            }
            if (player.transform.position.y <= 0)
            {
                player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            }
        }

        void OnEnable()
        {
            GameEventManager.onPlayerEnterArea += OnPlayerEnterArea;
        }

        public void LoadResources()
        {
            Map.LoadPlane();
            Map.LoadWalls();
            Map.LoadFences();
            Map.LoadAreaColliders();
            LoadSoldiers();
            player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            player.transform.position = new Vector3(-4.5f, 0, -4.5f);
            Restart();
        }

        public void Restart()
        {
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

        public void MovePlayer()
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
            //移动和旋转
            player.transform.Translate(0, 0, dz * 4f * Time.deltaTime);
            player.transform.Rotate(0, dx * 50f * Time.deltaTime, 0);
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

        private void LoadSoldiers()
        {
            GameObject soldierPrefab = Resources.Load<GameObject>("Prefabs/Soldier");
            for (int i = 0; i < 9; ++i)
            {
                GameObject soldier = Instantiate(soldierPrefab);
                soldier.AddComponent<Patrol>().area = i;
                soldier.GetComponent<Animator>().SetBool("isRunning", true);
                soldier.name = "Soldier" + i;
                soldier.transform.position = Map.center[i];
                soldiers.Add(soldier);
            }
        }

        private void ResetSoldiers()
        {
            for (int i = 0; i < 9; ++i)
            {

            }
        }
    }
}
