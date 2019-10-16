using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archery
{
    public interface IUserAction
    {
        void GetArrow();
        void MoveArrow(Vector3 direction);
        void ShootArrow(Vector3 direction);
    }

    public class GameController : MonoBehaviour, ISceneController, IUserAction
    {
        public GameModel model = new GameModel();
        public GameGUI view;

        // 箭工厂
        private ArrowFactory arrowFactory;
        // 箭对象列表
        private List<GameObject> arrows = new List<GameObject>();
        // 箭
        private GameObject holdingArrow;
        // 箭靶
        private GameObject target;

        void Awake()
        {
            // 获取箭工厂实例。
            arrowFactory = ArrowFactory.GetInstance();
            Director.GetInstance().OnSceneWake(this);
            view = gameObject.AddComponent<GameGUI>();
            model.onGameModelChanged += (sender, e) =>
            {
                view.ShowScore(e.score);
                view.ShowTips(e.delta);
            };
        }

        void Update()
        {
            // 回收箭。
            ReuseArrow();
            // 玩家按下空格键，获取箭。
            if (model.scene == SceneState.WaitToGetArrow && Input.GetKeyDown(KeyCode.Space))
            {
                GetArrow();
                return;
            }
            if (model.scene == SceneState.WaitToShootArrow)
            {
                var direction = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
                // 控制箭的方向为鼠标指针方向。
                MoveArrow(direction);
                // 玩家按下鼠标左键，发射箭。
                if (Input.GetMouseButtonDown(0))
                {
                    ShootArrow(direction);
                }
                return;
            }

        }

        public void LoadResources()
        {
            // 实例化箭靶。
            target = Instantiate(Resources.Load<GameObject>("Prefabs/Target"));
        }

        // 获取箭。
        public void GetArrow()
        {
            holdingArrow = arrowFactory.Get();
            arrows.Add(holdingArrow);
            // 设置游戏状态。
            model.scene = SceneState.WaitToShootArrow;
        }

        // 控制箭的指向。
        public void MoveArrow(Vector3 direction)
        {
            holdingArrow.transform.LookAt(30 * direction);
        }

        // 发射箭。
        public void ShootArrow(Vector3 direction)
        {
            var collider = holdingArrow.GetComponentInChildren<ArrowCollider>();
            collider.Reset();
            collider.onArrowHitTarget = (sender, e) =>
            {
                OnArrowHitTarget(e);
            };

            var rigidbody = holdingArrow.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(30 * direction, ForceMode.Impulse);
            holdingArrow = null;
            // 设置游戏状态。
            model.scene = SceneState.Shooting;
        }

        // 回收箭。
        private void ReuseArrow()
        {
            if (model.scene != SceneState.Shooting)
            {
                return;
            }
            var invisibleArrows = arrows.FindAll(x => x.transform.position.y <= -8f || x.GetComponentInChildren<ArrowCollider>().isHitTarget == false);
            foreach (var arrow in invisibleArrows)
            {
                arrows.Remove(arrow);
                arrowFactory.Put(arrow);
            }
            // 恢复游戏状态为"WaitToGetArrow"。
            if (arrows.Count == 0)
            {
                model.scene = SceneState.WaitToGetArrow;
            }
        }

        private void ResetSceneStatus()
        {
            if (model.scene != SceneState.Shooting)
            {
                return;
            }
            var invisibleArrows = arrows.FindAll(x => x.transform.position.y <= -8f);
            if (invisibleArrows.Count == 0)
            {
                model.scene = SceneState.WaitToGetArrow;
                return;
            }
        }

        void OnArrowHitTarget(ArrowHitTargetEvent e)
        {
            model.AddScore(e.target);
            if (e.target != 0)
            {
                arrows.Remove(e.arrow);
                model.scene = SceneState.WaitToGetArrow;
            }
        }
    }
}