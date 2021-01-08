using Game;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class SearchPathDemo : MonoBehaviour
    {
        private SearchPath path_ctr;

        private Dictionary<Vector2, Renderer> m_map = new Dictionary<Vector2, Renderer>();

        private void Awake()
        {
            path_ctr = new SearchPath();
        }

        private void Start()
        {
            path_ctr.InitMap(100, 100, false);

            for (int i = 0; i < 60; i++)
            {
                path_ctr.Obstacle(new Vector2(60, i));
            }

            for (int i = 10; i < 60; i++)
            {
                path_ctr.Obstacle(new Vector2(i, 60));
            }

            for (int i = 0; i < 50; i++)
            {
                path_ctr.Obstacle(new Vector2(i, 30));
            }

            Node[,] nodes = path_ctr.GetNodes();

            InitMap(nodes);

            Search(new Vector2(3, 3), new Vector2(80, 74));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Vector2 start = new Vector2(Random.Range(1, 30), Random.Range(1, 100));

                Vector2 end = new Vector2(Random.Range(70, 100), Random.Range(1, 100));

                Search(start, end);
            }
        }

        private void Search(Vector2 start, Vector2 end)
        {
            bool result = path_ctr.Search(start, end);

            if (result)
            {
                Debug.Log("<color=green>成功寻找到路径!</color>" + path_ctr.Output.Count);
            }
            else
            {
                Debug.LogFormat("<color=red>未寻找到路径，起始点：{0} 结束点{1}</color>", start, end);
            }

            Node[,] nodes = path_ctr.GetNodes();

            RefreshMap(nodes);
        }

        public void InitMap(Node[,] nodes)
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    GameObject curCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    curCube.transform.position = new Vector3(i, j, 0);
                    m_map.Add(new Vector2(i, j), curCube.GetComponent<Renderer>());
                }
            }
        }

        public void RefreshMap(Node[,] nodes)
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    Vector2 key = new Vector2(i, j);

                    if (m_map.ContainsKey(key))
                    {
                        if (nodes[i, j].type == NodeType.Boundary)
                        {
                            m_map[key].material.SetColor("_Color", Color.black);
                        }
                        else if (nodes[i, j].type == NodeType.Obstacle)
                        {
                            m_map[key].material.SetColor("_Color", Color.red);
                        }
                        else if (nodes[i, j].type == NodeType.Route)
                        {
                            m_map[key].material.SetColor("_Color", Color.yellow);
                        }
                        else
                        {
                            m_map[key].material.SetColor("_Color", Color.white);
                        }
                    }
                }
            }
        }
    }
}