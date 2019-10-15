using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class GameController : MonoBehaviour, ISceneController
    {
        private GameModel model = new GameModel();
        private GameGUI view;
        // 管理飞碟的颜色、分数、颜色、个数。
        private Ruler ruler;
        // 管理飞碟的运动学模型：物理运动、运动学（变换）。
        private IActionManager actionManager;
        // 预设：飞碟点击爆炸效果。
        public GameObject explosionPrefab;

        private List<GameObject> UFOs = new List<GameObject>();

        void Awake()
        {
            Director.GetInstance().OnSceneWake(this);
            view = gameObject.AddComponent<GameGUI>();
            view.onPressRestartButton += delegate
            {
                model.Reset();
            };
            view.onPressNextRoundButton += delegate
            {
                if (model.game == GameState.Running)
                {
                    model.NextRound();
                }
            };
            view.onPressNextTrialButton += delegate
            {
                if (model.game == GameState.Running)
                {
                    model.NextTrial();
                }
            };
            // 使用“运动学（变换）运动”模型。
            actionManager = new CCActionManager();
            ruler = new Ruler(model.currentRound, actionManager);
            // 更新游戏画面。
            model.onRefresh += delegate
            {
                view.state = model.game;
                view.round = model.currentRound;
                view.trial = model.currentTrial;
                view.score = model.score;
            };
            // 更新 Ruler 。
            model.onEnterNextRound += delegate
            {
                ruler = new Ruler(model.currentRound, actionManager);
            };
        }

        void Update()
        {
            var invisibleUFOs = UFOs.FindAll(x => x.transform.position.y <= -10f);
            foreach (var ufo in invisibleUFOs)
            {
                OnMissUFO(ufo);
            }

            if (model.game == GameState.Running)
            {
                if (model.scene == SceneState.Shooting && Input.GetButtonDown("Fire1"))
                {
                    // 光标拾取单个游戏对象。
                    // 构建射线。
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // 当射线与飞碟碰撞时，即说明我们想用鼠标点击此飞碟。
                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.tag == "UFO")
                    {
                        OnHitUFO(hit.collider.gameObject);
                    }
                }
                // 发射飞碟。
                if (model.scene == SceneState.Waiting && Input.GetKeyDown("space"))
                {
                    model.scene = SceneState.Shooting;
                    model.NextTrial();
                    // 添加此判断的原因：对于最后一次按下空格键，若玩家满足胜利条件，则不发射飞碟。
                    if (model.game == GameState.Win)
                    {
                        return;
                    }
                    UFOs.AddRange(ruler.GetUFOs());
                }

                if (UFOs.Count == 0)
                {
                    model.scene = SceneState.Waiting;
                }
            }

        }

        // 由于场景并无需要初始化的资源，故函数体为空。
        public void LoadResources() { }

        // 该协程用于控制飞碟爆炸效果。
        private IEnumerator DestroyExplosion(GameObject ufo)
        {
            // 实例化预制。
            GameObject explosion = Instantiate(explosionPrefab);
            // 设置爆炸效果的位置。
            explosion.transform.position = ufo.transform.position;
            // 回收飞碟对象。
            DestroyUFO(ufo);
            // 爆炸效果持续 1.2 秒。
            yield return new WaitForSeconds(1.2f);
            // 销毁爆炸效果对象。
            Destroy(explosion);
        }

        // 在用户成功点击飞碟后被触发。
        private void OnHitUFO(GameObject ufo)
        {
            // 增加分数。
            model.AddScore(ufo.GetComponent<UFOModel>().score);
            // 创建协程，用于控制飞碟爆炸效果的延续时间。
            StartCoroutine("DestroyExplosion", ufo);
        }

        // 在用户错失飞碟后被触发。
        private void OnMissUFO(GameObject ufo)
        {
            // 扣除分数。
            model.SubScore();
            // 回收飞碟对象。
            DestroyUFO(ufo);
        }

        // 回收飞碟对象。
        private void DestroyUFO(GameObject ufo)
        {
            UFOs.Remove(ufo);
            // 调用工厂模式的回收方法。
            UFOFactory.GetInstance().Put(ufo);
        }
    }
}