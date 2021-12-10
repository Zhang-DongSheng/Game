using System.Collections.Generic;
using System.IO;
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
            TalentFloyd floyd = new TalentFloyd();

            queues.Clear(); talents.Clear();

            talents.AddRange(trunk.children);

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
            queues.AddRange(floyd.Init(talents));
        }
        [ContextMenu("写入本地")]
        protected void Write()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "queue.txt");

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamWriter writer = new StreamWriter(stream);

                for (int i = 0; i < queues.Count; i++)
                {
                    writer.WriteLine(string.Format("From: [{0}] +{1}", queues[i].key, queues[i].possibles.Count));

                    for (int j = 0; j < queues[i].possibles.Count; j++)
                    {
                        writer.WriteLine(string.Format("{0}-{1}: {2}", queues[i].key, queues[i].possibles[j].final,
                            string.Join(",", queues[i].possibles[j].routes)));
                    }
                }
                writer.Flush(); writer.Close();
            }
        }
    }
}