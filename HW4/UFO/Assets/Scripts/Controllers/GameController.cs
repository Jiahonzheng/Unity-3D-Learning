using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class GameController : MonoBehaviour, ISceneController
    {
        private GameModel model = new GameModel();
        private GameGUI view;
        private Ruler ruler;

        public GameObject explosionPrefab;

        private List<GameObject> UFOs = new List<GameObject>();

        void Awake()
        {
            Director.GetInstance().OnSceneWake(this);
            view = gameObject.AddComponent<GameGUI>();
            view.onPressRestartButton += delegate
            {
                Reset();
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
            ruler = new Ruler(model.currentRound);
            model.onRefresh += delegate
            {
                view.state = model.game;
                view.round = model.currentRound;
                view.trial = model.currentTrial;
                view.score = model.score;
            };
            model.onEnterNextRound += delegate
            {
                ruler = new Ruler(model.currentRound);
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
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.tag == "UFO")
                    {
                        OnHitUFO(hit.collider.gameObject);
                    }
                }

                if (model.scene == SceneState.Waiting && Input.GetKeyDown("space"))
                {
                    model.scene = SceneState.Shooting;
                    model.NextTrial();
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

        public void LoadResources() { }

        public void Reset()
        {
            model.Reset();
        }

        private IEnumerator DestroyExplosion(GameObject ufo)
        {
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.position = ufo.transform.position;
            yield return new WaitForSeconds(1.2f);
            Destroy(explosion);
        }

        private void OnHitUFO(GameObject ufo)
        {
            StartCoroutine("DestroyExplosion", ufo);

            model.AddScore(ufo.GetComponent<UFOModel>().score);
            DestroyUFO(ufo);
        }

        private void OnMissUFO(GameObject ufo)
        {
            model.SubScore();
            DestroyUFO(ufo);
        }

        private void DestroyUFO(GameObject ufo)
        {
            UFOs.Remove(ufo);
            UFOFactory.GetInstance().Put(ufo);
        }
    }
}