using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class ItemDeployForDrag : ItemBase
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private Text text;

        [SerializeField] private LayerMask layer;

        private int index, select;

        private Vector3 position;

        protected override void OnUpdate(float delta)
        {
            if (Input.GetMouseButton(0))
            {
                position = Input.mousePosition;

                select = -1;

                var ray = Camera.main.ScreenPointToRay(position);

                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layer))
                {
                    select = DeployLogic.Instance.GetIndex(hit.transform);
                }
                Refresh(position);

                DeployLogic.Instance.Trigger(select);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SetActive(false);
            }
        }

        protected void Refresh(Vector3 position)
        {
            target.localPosition = UIUtils.ScreentPointToUGUIPosition(position);
        }

        public virtual void Refresh(int index)
        {
            this.index = index;

            text.text = index.ToString();

            SetActive(true);
        }
    }
}
