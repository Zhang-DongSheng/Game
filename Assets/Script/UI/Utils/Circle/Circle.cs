using System.Collections.Generic;
using UnityEngine.Sprites;

namespace UnityEngine.UI
{
    public class Circle : Image
    {
        [SerializeField] private int segements = 100;

        [SerializeField] private int showPercent = 1;

        [SerializeField] private bool accurateRaycast;

        private readonly Color32 GRAY = new Color32(60, 60, 60, 255);

        private readonly List<Vector3> vertexs = new List<Vector3>();

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            helper.Clear(); vertexs.Clear();

            AddVertex(helper);

            AddTriangle(helper);
        }

        private void AddVertex(VertexHelper helper)
        {
            //获取图形的宽和高
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            int realSegements = (int)(segements * showPercent);//真实要显示三角形的个数

            Vector4 uv = overrideSprite != null ? DataUtility.GetInnerUV(overrideSprite) : Vector4.zero;//获取sprite uv
            
            //计算uv宽和高
            float uvWidth = uv.z - uv.x;
            float uvHeight = uv.w - uv.y;
            Vector2 uvCenter = new Vector2((uv.x + uv.z) * 0.5f, (uv.y + uv.w) * 0.5f);//uv中心点坐标 
            Vector2 convertRatio = new Vector2(uvWidth / width, uvHeight / height);//uv和UI坐标的转化率

            //计算每块对应的弧度和计算半径
            float radian = (2 * Mathf.PI) / segements;//弧度 一个整圆弧度是2π 
            float radius = width * 0.5f;//半径
                                        //计算每个三角形定点坐标 1.计算圆心点信息(所有三角形公用顶点) 2.由圆心顶点计算三角形其他顶点信息 利用sin@ cos@ 函数

            Vector2 originPos = new Vector2((0.5f - rectTransform.pivot.x) * width, (0.5f - rectTransform.pivot.y) * height);//圆心顶点UI坐标 默认轴心点(0.5,0.5) 默认坐标(0,0)
            Vector2 originuvPos = Vector2.zero;//圆心顶点uv坐标
            Color32 colorTemp = GetOriginColor();

            UIVertex origin = GetUIVertex(colorTemp, originPos, originuvPos, uvCenter, convertRatio); //圆心的定点信息
            helper.AddVert(origin);

            int vertexCount = realSegements + 1;//真实顶点个数
            if (showPercent == 0)//显示比例0 要显示顶点个数==0
                vertexCount = 0;

            float curRadian = 0;//当前的弧度
            Vector2 posTermp = Vector2.zero;
            for (int i = 0; i < segements + 1; i++)//显示所有顶点
            {
                float x = Mathf.Cos(curRadian) * radius;
                float y = Mathf.Sin(curRadian) * radius;
                curRadian += radian;//计算下个弧度

                if (i < vertexCount)
                {
                    colorTemp = color;
                }
                else//不显示部分 顶点颜色设置变灰
                {
                    colorTemp = GRAY;
                }
                posTermp = new Vector2(x, y);//设置顶点UI坐标
                UIVertex vertexTemp = GetUIVertex(colorTemp, posTermp + originPos, posTermp, uvCenter, convertRatio);//计算下个顶点
                helper.AddVert(vertexTemp);
                if (accurateRaycast)
                    vertexs.Add(posTermp + originPos);
            }
        }

        private void AddTriangle(VertexHelper helper)
        {
            int index = 0;

            for (int i = 0; i < segements; i++)
            {
                index++;

                if (index == segements)
                {
                    helper.AddTriangle(index, 0, 1);
                }
                else
                {
                    helper.AddTriangle(index, 0, index + 1);
                }
            }
        }

        /// <summary>
        /// 得到圆心顶点的颜色
        /// </summary>
        /// <returns></returns>
        private Color32 GetOriginColor()
        {
            Color32 colorTemp = (Color.white - GRAY) * showPercent;
            colorTemp = new Color32((byte)(GRAY.r + colorTemp.r), (byte)(GRAY.g + colorTemp.g), (byte)(GRAY.b + colorTemp.b), 255);
            return colorTemp;
        }

        /// <summary>
        /// 获取UV顶点
        /// </summary>
        private UIVertex GetUIVertex(Color32 col, Vector3 pos, Vector2 uvPos, Vector2 uvCenter, Vector2 uvScale)
        {
            UIVertex vertex = new UIVertex();
            vertex.color = col;//设置顶点颜色
            vertex.position = pos;//设置顶点位置
                                  //计算顶点位置对应的图片的uv坐标--顶点pos--uv==》转化公式 ：uv.x=pos.x*ratio.x+uvCenter.x --》uvCenter.x 防止轴心点改变 图片偏移
            vertex.uv0 = new Vector2(uvPos.x * uvScale.x + uvCenter.x, uvPos.y * uvScale.y + uvCenter.y);//设置顶点uv
            return vertex;
        }

        /// <summary>
        /// 重写射线检测方法
        /// </summary>
        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (accurateRaycast)//开启精准射线检测
            {
                //将点击屏幕的点-->局部坐标
                Vector2 clickPos = Vector2.zero;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out clickPos);
                //判断点击的点 是否有效   
                //算法原理：图形上相邻的2点组成一条直线  判断点击的点向右发射的射线 与直线相交点 是不是奇数 奇数代表 点在图形内部 偶数代表 点在图形外部
                bool flag = GetCrossPointNum(clickPos, vertexs) % 2 == 1 ? true : false;
                return flag;
            }
            else//默认射线检测
            {
                return base.IsRaycastLocationValid(screenPoint, eventCamera);
            }
        }

        /// <summary>
        /// 得到点与直线相交点的个数
        /// </summary>
        /// <param name="clickPos">点击屏幕的点的局部坐标</param>
        /// <param name="_vertexList">组成的图形的所有点(除了圆形点)</param>
        /// <returns></returns>
        private int GetCrossPointNum(Vector2 clickPos, List<Vector3> _vertexList)
        {
            int crossCount = 0; //相交点个数
            Vector3 vert1 = Vector3.zero;
            Vector3 vert2 = Vector3.zero;
            int vertexCount = _vertexList.Count;//组成图形顶点个数
                                                //遍历所有顶点 找所有到由相邻2个点组成的直线
            for (int i = 0; i < vertexCount; i++)
            {
                vert1 = _vertexList[i];
                vert2 = _vertexList[(i + 1) % vertexCount];//取余 防止越界 和 点收尾相连
                if (IsYInRang(clickPos, vert1, vert2))//点击的点 在直线Y轴范围内
                {
                    float x = GetX(vert1, vert2, clickPos.y);
                    if (clickPos.x < x)//满足向右的射线
                    {
                        crossCount++;
                    }
                }
            }
            return crossCount;
        }

        /// <summary>
        /// 点击的点是否在Y轴有效范围内
        /// </summary>
        private bool IsYInRang(Vector2 clickPos, Vector3 vert1, Vector3 vert2)
        {
            if (vert1.y > vert2.y)
            {
                return clickPos.y < vert1.y && clickPos.y > vert2.y;
            }
            else
            {
                return clickPos.y > vert1.y && clickPos.y < vert2.y;
            }
        }

        /// <summary>
        /// 得到相交的坐标的X点 Y点已知
        /// </summary>
        private float GetX(Vector3 vert1, Vector3 vert2, float y)
        {
            //根据直线公式 斜截式：y=k*x+b    k=(y1-y2)/(x1-x2)-->k已知 又已知一点(x1,y1) 和y--》可求x
            float k = (vert1.x - vert2.x) / (vert1.y - vert2.y);//计算斜率k
                                                                //有2点 (x,y) （vert2.x，vert2.y）
            float x = (k / (y - vert2.y)) + vert2.x;
            return x;
        }
    }
}
