using UnityEngine;

namespace Game.Model
{
    public class ModelDrag : MonoBehaviour
    {
        [SerializeField] private GameObject model;

        private Vector3 offset;

        private Vector3 position;

        public ModelDragStatus status { get; private set; }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnBegin();
            }
            else if (Input.GetMouseButton(0))
            {
                OnDrag();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnEnd();
            }
        }

        private void OnBegin()
        {
            //��ȡ�������Ļ����
            position = Camera.main.WorldToScreenPoint(model.transform.position);
            //��ȡ�������������Ļ�ϵ�ƫ����
            offset = position - Input.mousePosition;
        }

        private void OnDrag()
        {
            //����ק���������Ļ���껹ԭΪ��������
            position = Camera.main.ScreenToWorldPoint(Input.mousePosition + offset);

            model.transform.position = position;
        }

        private void OnEnd()
        {

        }
    }

    public enum ModelDragStatus
    {
        Idle,
        Select,
        Drag,
        Complete,
    }
}