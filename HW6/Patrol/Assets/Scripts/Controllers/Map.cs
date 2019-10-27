using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrol
{
    class Map : Object
    {
        private static GameObject planePrefab = Resources.Load<GameObject>("Prefabs/Plane");
        private static GameObject fencePrefab = Resources.Load<GameObject>("Prefabs/Fence");
        private static GameObject areaColliderPrefab = Resources.Load<GameObject>("Prefabs/AreaCollider");

        public static Vector3[] center = new Vector3[] { new Vector3(-10, 0, -10), new Vector3(0, 0, -10), new Vector3(10, 0, -10), new Vector3(-10, 0, 0), new Vector3(0, 0, 0), new Vector3(10, 0, 0), new Vector3(-10, 0, 10), new Vector3(0, 0, 10), new Vector3(10, 0, 10) };

        public static void LoadPlane()
        {
            GameObject map = Instantiate(planePrefab);
        }

        public static void LoadWalls()
        {
            for (int i = 0; i < 12; ++i)
            {
                GameObject fence = Instantiate(fencePrefab);
                fence.transform.position = new Vector3(-12.5f + 2.5f * i, 0, -15);
            }
            for (int i = 0; i < 12; ++i)
            {
                GameObject fence = Instantiate(fencePrefab);
                fence.transform.position = new Vector3(-12.5f + 2.5f * i, 0, 15);
            }
            for (int i = 0; i < 12; ++i)
            {
                GameObject fence = Instantiate(fencePrefab);
                fence.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                fence.transform.position = new Vector3(-15, 0, -15 + 2.5f * i);
            }
            for (int i = 0; i < 12; ++i)
            {
                GameObject fence = Instantiate(fencePrefab);
                fence.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                fence.transform.position = new Vector3(15, 0, -15 + 2.5f * i);
            }
        }

        public static void LoadFences()
        {
            var row = new int[2, 12] { { 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1 }, { 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0 } };
            var col = new int[2, 12] { { 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1 }, { 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0 } };
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 12; ++j)
                {
                    if (row[i, j] == 1)
                    {
                        GameObject fence = Instantiate(fencePrefab);
                        fence.transform.position = new Vector3(-12.5f + 2.5f * j, 0, -5 + 10 * i);
                    }
                }
            }
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 12; ++j)
                {
                    if (col[i, j] == 1)
                    {
                        GameObject fence = Instantiate(fencePrefab);
                        fence.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                        fence.transform.position = new Vector3(-5 + 10 * i, 0, -15 + 2.5f * j);
                    }
                }
            }
        }

        public static void LoadAreaColliders()
        {
            for (int i = 0; i < 9; ++i)
            {
                GameObject collider = Instantiate(areaColliderPrefab);
                collider.name = "AreaCollider" + i;
                collider.transform.position = center[i];
            }
            // int row = 0;
            // int col = -1;
            // for (int i = 0; i < 9; ++i)
            // {
            //     if (i == 3 || i == 6)
            //     {
            //         row++;
            //         col = 0;
            //     }
            //     else
            //     {
            //         col++;
            //     }
            //     GameObject collider = Instantiate(areaColliderPrefab);
            //     collider.name = "AreaCollider" + i;
            //     collider.transform.position = new Vector3(-10 + 10 * col, 0, -10 + 10 * row);
            // }
        }
    }
}