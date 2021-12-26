using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TalentFloyd
    {
        private readonly List<TalentQueue> queues = new List<TalentQueue>();

        private List<Talent> talents;

        public List<TalentQueue> Init(List<Talent> talents)
        {
            this.talents = talents;

            queues.Clear();

            MGraph g;

            MAXV = talents.Count;

            g.n = MAXV; g.e = 8;

            g.edges = new int[MAXV, MAXV];

            g.vexs = new VertexType[MAXV];

            for (int i = 0; i < MAXV; i++)
            {
                for (int j = 0; j < MAXV; j++)
                {
                    g.edges[i, j] = Distance(talents, i, j);
                }
            }

            Floyd(g);

            return queues;
        }

        private int Distance(List<Talent> talents, int i, int j)
        {
            if (i == j)
            {
                return 0;
            }
            else
            {
                Talent from = talents[i];

                Talent to = talents[j];

                if (from.active && to.active)
                {
                    if (from.neighbours.x == to.ID ||
                        from.neighbours.y == to.ID ||
                        from.neighbours.z == to.ID)
                    {
                        return 1;
                    }
                }
                return INF;
            }
        }

        private const int INF = 32767;    //INF表示∞
        private int MAXV;

        struct VertexType
        {
            public int no;                        //顶点编号
            public int info;                    //顶点其他信息
        };                               //顶点类型
        struct MGraph                    //图的定义
        {
            public int[,] edges;       //邻接矩阵
            public int n, e;             //顶点数,弧数
            public VertexType[] vexs;          //存放顶点信息
        };

        void Floyd(MGraph g)
        {
            int[,] A = new int[MAXV, MAXV];//A用于存放当前顶点之间的最短路径长度,分量A[i][j]表示当前顶点vi到顶点vj的最短路径长度。
            int[,] path = new int[MAXV, MAXV];//从顶点vi到顶点vj的路径上所经过的顶点编号不大于k的最短路径长度。
            int i, j, k;
            for (i = 0; i < g.n; i++)
            {
                for (j = 0; j < g.n; j++)//对各个节点初始已经知道的路径和距离
                {
                    A[i, j] = g.edges[i, j];
                    path[i, j] = -1;
                }
            }
            for (k = 0; k < g.n; k++)
            {
                for (i = 0; i < g.n; i++)
                    for (j = 0; j < g.n; j++)
                        if (A[i, j] > A[i, k] + A[k, j])//从i到j的路径比从i经过k到j的路径长
                        {
                            A[i, j] = A[i, k] + A[k, j];//更改路径长度
                            path[i, j] = k;//更改路径信息经过k
                        }
            }

            Dispath(A, path, g.n);   //输出最短路径
        }

        void Dispath(int[,] A, int[,] path, int n)
        {
            int i, j;

            for (i = 0; i < n; i++)
            {
                TalentQueue talent = new TalentQueue();

                talent.key = talents[i].ID;

                for (j = 0; j < n; j++)
                {
                    if (i == j) continue;

                    if (A[i, j] == INF)
                    {
                        if (i != j)
                        {
                            Debug.LogFormat("从{0}到{1}没有路径\n", i, j);
                        }
                    }
                    else
                    {
                        List<int> route = new List<int>();

                        route.Add(talents[i].ID);

                        Ppath(path, i, j, route);

                        route.Add(talents[j].ID);

                        if (Pass(route))
                        {
                            talent.possibles.Add(new TalentPossible()
                            {
                                final = talents[j].ID,
                                routes = route
                            });
                        }
                    }
                }

                queues.Add(talent);
            }
        }

        void Ppath(int[,] path, int i, int j, List<int> route)  //前向递归查找路径上的顶点
        {
            int k;

            k = path[i, j];

            if (k == -1) return;    //找到了起点则返回

            Ppath(path, i, k, route);    //找顶点i的前一个顶点k

            route.Add(talents[k].ID);

            Ppath(path, k, j, route);    //找顶点k的前一个顶点j
        }

        private bool Pass(List<int> route)
        {
            int count = route.Count;

            for (int i = 0; i < count; i++)
            {
                if (Mathf.FloorToInt(route[i] / 10) == 101)
                {
                    if (i < count - 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}