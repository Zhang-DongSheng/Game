using UnityEngine;

namespace Game.MiniGame
{
    /// <summary>
    /// 画线控制器
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

        #region 画线逻辑-----------------------------------------------------------------------
        /// <summary>
        /// 开始画线
        /// </summary>
        private void BeginDraw()
        {
            current = prefab.Create<Line>();
            // 设置参数
            current.Instantiate(lineWidth, lineColor, linePointsMinDistance, cantDrawOverLayer);
        }
        /// <summary>
        /// 画线
        /// </summary>
        private void Draw()
        {
            var pos = cam.ScreenToWorldPoint(Input.mousePosition);
            //防止线与线之间交叉
            RaycastHit2D hit = Physics2D.CircleCast(pos, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);
            if (hit)
                EndDraw();
            else
                current.AddPoint(pos);
        }
        /// <summary>
        /// 画线结束
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