using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UITalent : MonoBehaviour
    {
        [SerializeField] private TalentSystem system;

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
            ItemTalentSkill main = CreateSkill(system.trunk);

            skills.Add(main);

            int count = system.branches.Count;

            for (int i = 0; i < count; i++)
            {
                skills.Add(CreateSkill(system.branches[i]));
            }

            count = system.links.Count;

            for (int i = 0; i < count; i++)
            {
                links.Add(CreateLink(system.links[i]));
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

            item.callback = OnClickSkill;

            return item;
        }

        private ItemTalentLink CreateLink(TalentLink talent)
        {
            GameObject go = GameObject.Instantiate(prefabLink, parent);

            ItemTalentLink item = go.GetComponent<ItemTalentLink>();

            item.Initialize(talent, OnClickTalent);

            return item;
        }

        private List<int> Activated()
        {
            List<int> activated = new List<int>();

            activated.AddRange(server.list);

            for (int i = 0; i < system.trunk.children.Count; i++)
            {
                int talent = this.system.trunk.children[i].ID;

                if (!activated.Contains(talent))
                {
                    activated.Add(talent);
                }
            }
            return activated;
        }

        private void OnClickTalent(int talentID)
        {
            preview.Clear();

            if (system.trunk.children.Exists(x => x.ID == talentID))
            {
                preview.Add(talentID);
            }
            else
            {
                List<int> route = TalentUtils.Search(system, talentID, Activated());

                if (route == null)
                {
                    return;
                }
                preview.AddRange(route);
            }
            Refresh();
        }

        private void OnClickSkill(TalentSkill skill)
        {
            preview.Clear();

            if (skill.ID == system.trunk.ID)
            {
                for (int i = 0; i < skill.children.Count; i++)
                {
                    if (!server.list.Contains(skill.children[i].ID))
                    {
                        preview.Add(skill.children[i].ID);
                    }
                }
                preview.Add(skill.ID);
            }
            else
            {
                List<int> route = TalentUtils.Search(system, skill, Activated());

                if (route == null)
                {
                    return;
                }
                preview.AddRange(route);
            }
            Refresh();
        }
    }
}