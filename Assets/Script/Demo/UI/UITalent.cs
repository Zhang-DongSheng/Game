using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UITalent : MonoBehaviour
    {
        [SerializeField] private TalentSystem data;

        [SerializeField] private S2CTalent server;

        [SerializeField] private int target;

        [SerializeField] private Transform parent;

        [SerializeField] private GameObject prefabSkill;

        [SerializeField] private GameObject prefabLink;

        private readonly List<ItemTalentSkill> skills = new List<ItemTalentSkill>();

        private readonly List<ItemTalentLink> links = new List<ItemTalentLink>();

        private readonly List<int> preview = new List<int>();

        private void Start()
        {
            Init(); Refresh();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Refresh();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                server.list.Clear();

                server.list = server.str.SplitToList<int>(',');
            }
            else if (Input.GetKeyDown(KeyCode.F1))
            {
                server.list.AddRange(preview);

                Refresh();
            }
        }

        private void Init()
        {
            ItemTalentSkill main = CreateSkill(data.trunk);

            skills.Add(main);

            int count = data.branches.Count;

            for (int i = 0; i < count; i++)
            {
                skills.Add(CreateSkill(data.branches[i]));
            }

            count = data.links.Count;

            for (int i = 0; i < count; i++)
            {
                links.Add(CreateLink(data.links[i]));
            }
        }

        public void Refresh()
        {
            int count = skills.Count;

            for (int i = 0; i < count; i++)
            {
                skills[i].Refresh(server.list, preview);
            }

            count = links.Count;

            for (int i = 0; i < count; i++)
            {
                links[i].Refresh(server.list, preview);
            }
        }

        private ItemTalentSkill CreateSkill(TalentSkill talent)
        {
            GameObject go = GameObject.Instantiate(prefabSkill, parent);

            ItemTalentSkill item = go.GetComponent<ItemTalentSkill>();

            item.Initialize(talent, OnClickTalent);

            return item;
        }

        private ItemTalentLink CreateLink(TalentLink talent)
        {
            GameObject go = GameObject.Instantiate(prefabLink, parent);

            ItemTalentLink item = go.GetComponent<ItemTalentLink>();

            item.Initialize(talent, OnClickTalent);

            return item;
        }

        private void OnClickTalent(int talentID)
        {
            List<int> activated = new List<int>();

            activated.AddRange(server.list);

            for (int i = 0; i < data.trunk.children.Count; i++)
            {
                int talent = data.trunk.children[i].ID;

                if (!activated.Contains(talent))
                {
                    activated.Add(talent);
                }
            }

            List<int> route = TalentUtils.Beeline(data, talentID, activated);

            preview.Clear();

            preview.AddRange(route);

            Refresh();
        }
    }
}