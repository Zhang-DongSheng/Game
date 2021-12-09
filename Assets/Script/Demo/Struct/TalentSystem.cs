using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "talent.asset", menuName = "Talent")]
    public class TalentSystem : ScriptableObject
    {
        public TalentSkill trunk;

        public List<TalentSkill> branches;

        public List<TalentLink> links;

        public List<TalentQueue> queues;

        private readonly List<Talent> talents = new List<Talent>();

        private readonly List<TalentPossible> possibles = new List<TalentPossible>();

        private void OnValidate()
        {
            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i] != null && branches[i].children.Count != TalentConfig.CHILDREN)
                {
                    branches[i] = new TalentSkill();
                }
            }
        }

        [ContextMenu("自动生成ID")]
        protected void MenuRebuildID()
        {
            trunk.ID = 101;

            for (int i = 0; i < trunk.children.Count; i++)
            {
                trunk.children[i].ID = trunk.ID * 10 + i;
            }

            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i] != null)
                {
                    branches[i].ID = 102 + i;

                    for (int j = 0; j < branches[i].children.Count; j++)
                    {
                        branches[i].children[j].ID = branches[i].ID * 10 + j;
                    }
                }
            }

            for (int i = 0; i < links.Count; i++)
            {
                links[i].ID = 201 + i;
            }
        }

        [ContextMenu("关联定位")]
        protected void MenuUpdatePosition()
        {
            trunk.Init();

            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i] != null)
                {
                    branches[i].Init();
                }
            }

            for (int i = 0; i < links.Count; i++)
            {
                TalentSkill root = links[i].skills.x == trunk.ID ? trunk : branches.Find(x => x.ID == links[i].skills.x);

                root.Update(TalentUtils.Index(links[i].direction, true), links[i].ID);

                for (int j = 0; j < branches.Count; j++)
                {
                    if (branches[j].ID == links[i].skills.y)
                    {
                        links[i].Init(root, branches[j]);
                        branches[j].Update(TalentUtils.Index(links[i].direction, false), links[i].ID);
                        break;
                    }
                }
            }
        }

        [ContextMenu("生成所有可能路径")]
        protected void MenuCreatePossibles()
        {
            queues.Clear(); talents.Clear();

            int count = branches.Count;

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < branches[i].children.Count; j++)
                {
                    talents.Add(branches[i].children[j]);
                }
            }

            count = links.Count;

            for (int i = 0; i < count; i++)
            {
                talents.Add(links[i]);
            }

            count = branches.Count;

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < branches[i].children.Count; j++)
                {
                    queues.Add(new TalentQueue()
                    {
                        key = branches[i].children[j].ID,
                        possibles = Possibles(branches[i].children[j].ID)
                    });
                }
            }
        }

        private List<TalentPossible> Possibles(int index)
        {
            this.possibles.Clear();

            Possible(new List<int> { index });

            List<TalentPossible> possibles = new List<TalentPossible>();

            possibles.AddRange(this.possibles);

            return possibles;
        }

        private void Possible(List<int> routes)
        {
            int last = routes[routes.Count - 1];

            if (trunk.children.Exists(x => x.ID == last))
            {
                possibles.Add(new TalentPossible()
                {
                    routes = routes,
                });
            }

            Talent talent = talents.Find(x => x.ID == last);

            if (talent == null) return;

            if (talent.neighbours.x != 0 && !routes.Contains(talent.neighbours.x))
            {
                if (talent.neighbours.y != 0 && !routes.Contains(talent.neighbours.y))
                {
                    if (talent.neighbours.z != 0 && !routes.Contains(talent.neighbours.z))
                    {
                        List<int> ZR = new List<int>(routes);

                        ZR.Add(talent.neighbours.z);

                        Possible(ZR);
                    }

                    List<int> YR = new List<int>(routes);

                    YR.Add(talent.neighbours.y);

                    Possible(YR);
                }
                else if (talent.neighbours.z != 0 && !routes.Contains(talent.neighbours.z))
                {
                    List<int> ZR = new List<int>(routes);

                    ZR.Add(talent.neighbours.z);

                    Possible(ZR);
                }
                routes.Add(talent.neighbours.x); Possible(routes);
            }
            else if (talent.neighbours.y != 0 && !routes.Contains(talent.neighbours.y))
            {
                if (talent.neighbours.z != 0 && !routes.Contains(talent.neighbours.z))
                {
                    List<int> ZR = new List<int>(routes);

                    ZR.Add(talent.neighbours.z);

                    Possible(ZR);
                }
                routes.Add(talent.neighbours.y); Possible(routes);
            }
            else if (talent.neighbours.z != 0 && !routes.Contains(talent.neighbours.z))
            {
                routes.Add(talent.neighbours.z); Possible(routes);
            }
        }
    }
}