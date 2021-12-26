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

        private const int INF = 32767;    //INF��ʾ��
        private int MAXV;

        struct VertexType
        {
            public int no;                        //������
            public int info;                    //����������Ϣ
        };                               //��������
        struct MGraph                    //ͼ�Ķ���
        {
            public int[,] edges;       //�ڽӾ���
            public int n, e;             //������,����
            public VertexType[] vexs;          //��Ŷ�����Ϣ
        };

        void Floyd(MGraph g)
        {
            int[,] A = new int[MAXV, MAXV];//A���ڴ�ŵ�ǰ����֮������·������,����A[i][j]��ʾ��ǰ����vi������vj�����·�����ȡ�
            int[,] path = new int[MAXV, MAXV];//�Ӷ���vi������vj��·�����������Ķ����Ų�����k�����·�����ȡ�
            int i, j, k;
            for (i = 0; i < g.n; i++)
            {
                for (j = 0; j < g.n; j++)//�Ը����ڵ��ʼ�Ѿ�֪����·���;���
                {
                    A[i, j] = g.edges[i, j];
                    path[i, j] = -1;
                }
            }
            for (k = 0; k < g.n; k++)
            {
                for (i = 0; i < g.n; i++)
                    for (j = 0; j < g.n; j++)
                        if (A[i, j] > A[i, k] + A[k, j])//��i��j��·���ȴ�i����k��j��·����
                        {
                            A[i, j] = A[i, k] + A[k, j];//����·������
                            path[i, j] = k;//����·����Ϣ����k
                        }
            }

            Dispath(A, path, g.n);   //������·��
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
                            Debug.LogFormat("��{0}��{1}û��·��\n", i, j);
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

        void Ppath(int[,] path, int i, int j, List<int> route)  //ǰ��ݹ����·���ϵĶ���
        {
            int k;

            k = path[i, j];

            if (k == -1) return;    //�ҵ�������򷵻�

            Ppath(path, i, k, route);    //�Ҷ���i��ǰһ������k

            route.Add(talents[k].ID);

            Ppath(path, k, j, route);    //�Ҷ���k��ǰһ������j
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