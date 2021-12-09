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
            else if (Input.GetKeyDown(KeyCode.F))
            {
                Find();
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
                skills[i].Refresh(server.list);
            }

            count = links.Count;

            for (int i = 0; i < count; i++)
            {
                links[i].Refresh(server.list);
            }
        }

        public void Find()
        {
            if (target > 1000)
            {
                int index = Mathf.FloorToInt(target / 10);

                TalentSkill skill = data.branches.Find(x => x.ID == index);

                if (skill != null)
                {
                    TalentNode node = skill.children.Find(x => x.ID == target % 10);

                    

                    
                }
            }
            else if (target > 200)
            {

            }
            else if (target > 100)
            {

            }
            else
            {
                return;   
            }
        }

        

        private ItemTalentSkill CreateSkill(TalentSkill talent)
        {
            GameObject go = GameObject.Instantiate(prefabSkill, parent);

            ItemTalentSkill item = go.GetComponent<ItemTalentSkill>();

            item.Initialize(talent);

            return item;
        }

        private ItemTalentLink CreateLink(TalentLink talent)
        {
            GameObject go = GameObject.Instantiate(prefabLink, parent);

            ItemTalentLink item = go.GetComponent<ItemTalentLink>();

            item.Initialize(talent);

            return item;
        }
    }
}
