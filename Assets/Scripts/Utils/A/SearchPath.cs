using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game
{
    public enum NodeType
    {
        Movable,        //可移动区域
        Obstacle,       //障碍物
        Boundary,       //边界
        Route           //路径
    }

    public enum NodeState
    {
        None,           //默认
        Open,           //开放列表
        Close           //封闭列表
    }

    public class SearchPath
    {
        private readonly Dictionary<Vector2, Node> m_nodes = new Dictionary<Vector2, Node>();

        private readonly List<Node> list_close = new List<Node>();
        private readonly List<Node> list_open = new List<Node>();

        private Vector2 position_target;

        private Node[,] t_nodes;

        /// <summary>
        /// 初始化地图信息
        /// </summary>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="slant">斜向关联</param>
        /// <returns>地图数据</returns>
        public void InitMap(int width, int height, bool slant = false)
        {
            t_nodes = new Node[width, height];

            m_nodes.Clear();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    t_nodes[i, j] = new Node(i, j);

                    if (i == 0 || i == width - 1 ||
                        j == 0 || j == height - 1)
                    {
                        t_nodes[i, j].type = NodeType.Boundary;
                    }

                    m_nodes.Add(new Vector2(i, j), t_nodes[i, j]);
                }
            }

            Vector2 key;

            //关联周边节点
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (slant)
                    {
                        for (int h = -1; h <= 1; h++)
                        {
                            for (int v = -1; v <= 1; v++)
                            {
                                if (h != 0 || v != 0)
                                {
                                    key = new Vector2(i + h, j + v);

                                    if (m_nodes.ContainsKey(key))
                                    {
                                        t_nodes[i, j].neighbour.Add(m_nodes[key]);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int k = -1; k <= 1; k++)
                        {
                            if (k != 0)
                            {
                                key = new Vector2(i + k, j);

                                if (m_nodes.ContainsKey(key))
                                {
                                    t_nodes[i, j].neighbour.Add(m_nodes[key]);
                                }

                                key = new Vector2(i, j + k);

                                if (m_nodes.ContainsKey(key))
                                {
                                    t_nodes[i, j].neighbour.Add(m_nodes[key]);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置障碍物
        /// </summary>
        /// <param name="points">位置</param>
        public void Obstacle(params Vector2[] points)
        {
            foreach (var key in points)
            {
                if (m_nodes.ContainsKey(key))
                {
                    m_nodes[key].type = NodeType.Obstacle;
                }
            }
        }

        /// <summary>
        /// 寻路
        /// </summary>
        /// <param name="nodes">地图信息</param>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <returns>路径是否存在</returns>
        public bool Search(Vector2 start, Vector2 end)
        {
            bool result = false;

            if (!m_nodes.ContainsKey(start) || !m_nodes.ContainsKey(end))
            {
                return result;
            }
            if (m_nodes[start].type != NodeType.Movable || m_nodes[end].type != NodeType.Movable)
            {
                return result;
            }

            //设置终点
            position_target = end;

            //重置路径
            for (int i = 0; i < t_nodes.GetLength(0); i++)
            {
                for (int j = 0; j < t_nodes.GetLength(1); j++)
                {
                    t_nodes[i, j].Reset();
                }
            }

            list_close.Clear();
            list_open.Clear();

            Node A = t_nodes[(int)start.x, (int)start.y];
            A.G = 0;
            A.H = Vector2.Distance(position_target, A.position);
            A.F = A.G + A.H;
            A.parent = null;
            A.state = NodeState.Close;

            list_close.Add(A);

            do
            {
                if (list_open.Count > 0)
                {
                    A = list_open[0];
                }
                for (int i = 0; i < list_open.Count; i++)
                {
                    if (list_open[i].F < A.F)
                    {
                        A = list_open[i];
                    }
                }

                if (A.Compare(position_target))
                {
                    result = true;
                }

                Node B = Search(A);

                if (B != null)
                {
                    do
                    {
                        B.type = NodeType.Route;
                        B = B.parent;
                    }
                    while (B != null);
                }
                list_close.Add(A);
                list_open.Remove(A);
                A.state = NodeState.Close;
            }
            while (list_open.Count > 0);

            return result;
        }

        private Node Search(Node A)
        {
            Node B;

            for (int i = 0; i < A.neighbour.Count; i++)
            {
                if (A.neighbour[i] != null &&
                    A.neighbour[i].type == NodeType.Movable)
                {
                    B = A.neighbour[i];

                    if (B.state == NodeState.None)//更新B的父节点为A，并相应更新B.G; 计算B.F,B.H; B加入OpenList
                    {
                        B.parent = A;
                        B.G = Vector2.Distance(A.position, B.position) + A.G;
                        B.H = Vector2.Distance(B.position, position_target);
                        B.F = B.G + B.H;
                        B.state = NodeState.Open;

                        list_open.Add(B);

                        if (B.H < Mathf.Epsilon)//B的所有父节点既是路径
                        {
                            return B;
                        }
                    }
                    else if (B.state == NodeState.Open)
                    {
                        float curG = Vector2.Distance(A.position, B.position);

                        if (B.G > curG + A.G)//更新B的父节点为A，并相应更新B.G,B.H
                        {
                            B.parent = A;
                            B.G = curG + A.G;
                            B.F = B.G + B.H;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 路径数据
        /// </summary>
        public List<Vector2> Output
        {
            get
            {
                List<Vector2> route = new List<Vector2>();

                if (m_nodes.ContainsKey(position_target))
                {
                    Node node = m_nodes[position_target];

                    while (node != null)
                    {
                        route.Add(node.position);
                        node = node.parent;
                    }
                }
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < route.Count; i++)
                {
                    sb.Append(route[i].ToString());
                    sb.Append("&");
                }
                Debuger.Log(Author.Utility, $"<color=yellow>{sb.ToString()}</color>");

                return route;
            }
        }

        public Node[,] GetNodes()
        {
            return t_nodes;
        }
    }

    public class Node
    {
        public Vector2 position;

        public NodeState state;

        public NodeType type;

        public float F;         // F = G + H
        public float G;         //从起点移动到指定方格的移动代价
        public float H;         //从指定方格移动到终点的移动代价

        public Node parent;

        public List<Node> neighbour = new List<Node>();

        public Node(int x, int y)
        {
            position = new Vector2(x, y);
        }

        public void Reset()
        {
            F = G = H = 0;

            parent = null;

            state = NodeState.None;

            if (type.Equals(NodeType.Route))
            {
                type = NodeType.Movable;
            }
        }

        public bool Compare(Vector2 position)
        {
            return this.position.x == position.x &&
                   this.position.y == position.y;
        }
    }
}