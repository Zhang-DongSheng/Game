using UnityEngine;

namespace Game.MiniGame
{
    /// <summary>
    /// ���߿�����
    /// </summary>
    public class LinesDrawer : MonoBehaviour
    {
        public PrefabTemplate prefab;

        public LayerMask cantDrawOverLayer;

        [Space(30)]
        public Gradient lineColor;
        public float linePointsMinDistance;
        public float lineWidth;

        private Line current;

        Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                BeginDraw();
            if (null != current)
                Draw();
            if (Input.GetMouseButtonUp(0))
                EndDraw();
        }

        #region �����߼�-----------------------------------------------------------------------
        /// <summary>
        /// ��ʼ����
        /// </summary>
        private void BeginDraw()
        {
            current = prefab.Create<Line>();
            // ���ò���
            current.Instantiate(lineWidth, lineColor, linePointsMinDistance, cantDrawOverLayer);
        }
        /// <summary>
        /// ����
        /// </summary>
        private void Draw()
        {
            var pos = cam.ScreenToWorldPoint(Input.mousePosition);
            //��ֹ������֮�佻��
            RaycastHit2D hit = Physics2D.CircleCast(pos, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);
            if (hit)
                EndDraw();
            else
                current.AddPoint(pos);
        }
        /// <summary>
        /// ���߽���
        /// </summary>
        void EndDraw()
        {
            if (current == null) return;

            current.Over();

            current = null;
        }
        #endregion
    }
}