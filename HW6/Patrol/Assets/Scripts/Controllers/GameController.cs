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

        void Awake()
        {
            Director.GetInstance().OnSceneWake(this);
            actionManager = gameObject.AddComponent<GameActionManager>();
        }

        void Update()
        {
            MovePlayer();
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
        }

        public void Restart()
        {
            player.GetComponent<Animator>().Play("New State");
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

        private void LoadSoldiers()
        {
            GameObject soldierPrefab = Resources.Load<GameObject>("Prefabs/Soldier");
            for (int i = 0; i < 9; ++i)
            {
                GameObject soldier = Instantiate(soldierPrefab);
                soldier.AddComponent<Patrol>().area = i;
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
