using Data;
using UnityEngine;

namespace Game.UI
{
    public class ItemStatus : MonoBehaviour
    {
        [SerializeField] private GameObject undone;

        [SerializeField] private GameObject available;

        [SerializeField] private GameObject claimed;

        public void Refresh(Status status)
        {
            switch (status)
            {
                case Status.Undone:
                    {
                        SetActive(undone, true);
                        SetActive(available, false);
                        SetActive(claimed, false);
                    }
                    break;
                case Status.Available:
                    {
                        SetActive(undone, false);
                        SetActive(available, true);
                        SetActive(claimed, false);
                    }
                    break;
                case Status.Claimed:
                    {
                        SetActive(undone, false);
                        SetActive(available, false);
                        SetActive(claimed, true);
                    }
                    break;
            }
        }

        private void SetActive(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
        [ContextMenu("Creator")]
        protected void EditorCreator()
        {
            Transform parent = transform;

            while (parent.childCount < 3)
            {
                GameObject child = new GameObject();

                child.AddComponent<RectTransform>();

                child.transform.SetParent(parent);

                child.transform.localPosition = Vector3.zero;

                switch (parent.childCount)
                {
                    case 1:
                        undone = child; child.name = "undone";
                        break;
                    case 2:
                        available = child; child.name = "available";
                        break;
                    case 3:
                        claimed = child; child.name = "claimed";
                        break;
                }
            }
        }
    }
}