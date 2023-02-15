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
            //获取物体的屏幕坐标
            position = Camera.main.WorldToScreenPoint(model.transform.position);
            //获取物体与鼠标在屏幕上的偏移量
            offset = position - Input.mousePosition;
        }

        private void OnDrag()
        {
            //将拖拽后的物体屏幕坐标还原为世界坐标
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