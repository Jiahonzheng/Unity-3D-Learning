using UnityEngine;

namespace Smoke
{
    public class MainController : MonoBehaviour
    {
        private GameObject car;

        private void Awake()
        {
            // 装载资源。
            LoadResources();
        }

        // 显示损坏设置按钮。
        void OnGUI()
        {
            var labelStyle = new GUIStyle() { fontSize = 40 };
            var buttonStyle = new GUIStyle("button") { fontSize = 30 };
            GUI.Label(new Rect(160, 30, 200, 100), "Damage", labelStyle);
            // 设置为 0 损坏。
            if (GUI.Button(new Rect(160, 80, 100, 50), "0%", buttonStyle))
            {
                car.GetComponent<CarCollider>().SetDamage(0);
            }
            // 设置为 50% 损坏。
            if (GUI.Button(new Rect(160, 130, 100, 50), "50%", buttonStyle))
            {
                car.GetComponent<CarCollider>().SetDamage(100);
            }
            // 设置为 100% 损坏。
            if (GUI.Button(new Rect(160, 180, 100, 50), "100%", buttonStyle))
            {
                car.GetComponent<CarCollider>().SetDamage(200);
            }

        }

        // 装载地图资源。
        private void LoadResources()
        {
            // 加载地图平面。
            var plane = Instantiate(Resources.Load<GameObject>("Prefabs/Plane"));
            plane.name = "Plane";

            // 加载车辆资源。
            car = Instantiate(Resources.Load<GameObject>("Prefabs/Car"));
            car.name = "Car";

            // 加载边界预制。
            var wallPrefab = Resources.Load<GameObject>("Prefabs/Wall");
            // 绘制地图边界。
            {
                GameObject wall = Instantiate(wallPrefab);
                wall.transform.position = new Vector3(-25, 2, 0);
                wall.transform.localScale = new Vector3(1, 4, 100);
            }
            {
                GameObject wall = Instantiate(wallPrefab);
                wall.transform.position = new Vector3(25, 2, 0);
                wall.transform.localScale = new Vector3(1, 4, 100);
            }
            {
                GameObject wall = Instantiate(wallPrefab);
                wall.transform.position = new Vector3(0, 2, 50);
                wall.transform.localScale = new Vector3(50, 4, 1);
            }
            {
                GameObject wall = Instantiate(wallPrefab);
                wall.transform.position = new Vector3(0, 2, -50);
                wall.transform.localScale = new Vector3(50, 4, 1);
            }
        }
    }
}