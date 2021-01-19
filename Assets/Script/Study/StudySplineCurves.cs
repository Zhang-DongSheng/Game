using System.Collections.Generic;
using UnityEngine;

namespace SplineTest
{
    public class StudySplineCurves : MonoBehaviour
    {
        [SerializeField] private List<Vector3> points;

        private readonly List<SplineCurves> nodes = new List<SplineCurves>();

        private void OnValidate()
        {
            Compute();
        }

        private void Compute()
        {
            nodes.Clear();

            if (points != null && points.Count > 1)
            {
                SplineCurves spline, pre;

                for (int i = 0; i < points.Count; i++)
                {
                    if (nodes.Count == 0)
                    {
                        spline = new SplineCurves();
                        spline.AddJoint(null, points[i]);
                        nodes.Add(spline);
                    }
                    else
                    {
                        spline = new SplineCurves();
                        pre = nodes[nodes.Count - 1];
                        spline.AddJoint(pre, points[i]);
                        nodes.Add(spline);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (points == null || points.Count < 3) return;

            Gizmos.color = Color.green;

            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.DrawSphere(points[i], 1);
            }

            Gizmos.color = Color.yellow;

            for (int i = 1; i < nodes.Count; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                nodes[i].Draw();
            }
        }
    }

    /// <summary>    
    /// 样条曲线。每根样条曲线包含4个控制点
    /// </summary>
    public class SplineCurves
    {
        /// <summary>
        /// 样点数。在点Pk和Pk+1之间，将会生成若干个样点。所以"u"将会从0.00F增长到0.05F.
        /// </summary>
        private static readonly int _samplePointCount = 20;

        /// <summary>
        /// 在基数算法中的t
        /// </summary>
        private static readonly float _tension = 0.0F;


        #region 属性
        private Vector3 _startControlPoint;

        /// <summary>
        /// "Pk-1"点(起始控制点)
        /// </summary>
        public Vector3 StartControlPoint
        {
            get
            {
                return this._startControlPoint;
            }
            set
            {
                this._startControlPoint = value;
            }
        }

        private Vector3 _startPoint;
        /// <summary>
        ///  "Pk"点(起始点)
        /// </summary>
        public Vector3 StartPoint
        {
            get
            {
                return this._startPoint;
            }
            set
            {
                this._startPoint = value;
            }
        }



        private Vector3 _endPoint;
        /// <summary>
        /// "Pk+1"点(结束点)
        /// </summary>
        public Vector3 EndPoint
        {
            get
            {
                return this._endPoint;
            }
            set
            {
                this._endPoint = value;
            }
        }


        private Vector3 _endControlPoint;
        /// <summary>
        /// "Pk+2"点(结束控制点)
        /// </summary>
        public Vector3 EndControlPoint
        {
            get
            {
                return this._endControlPoint;
            }
            set
            {
                this._endControlPoint = value;
            }
        }


        private Vector3[] _ctrlPoints;
        /// <summary>
        /// 曲线点(控制点及模拟的样点)
        /// </summary>
        public Vector3[] CtrlPoints
        {
            get
            {
                return this._ctrlPoints;
            }
        }


        private bool _isFirst = false;
        /// <summary>
        /// 标识当前样条曲线是否是第一条，如果是m_startControlPoint 和 m_startPoint将会相同。
        /// 因为在Pk和Pk+1之间需要4个点来决定样条曲线，所以我们需要在Pk-1点前手动添加一个点。
        /// 这样我们才能在Pk-1和Pk+1之间绘制样条曲线。
        /// 同样的，最后一根样条曲线的Pk+2点会与它的"Pk+1"点相同，
        /// 这样我们才能在Pk+1和Pk+2之间绘制样条曲线。
        /// </summary>
        public bool IsFirst
        {
            get
            {
                return this._isFirst;
            }
            set
            {
                this._isFirst = value;
            }
        }
        #endregion

        public SplineCurves()
        {
            _startControlPoint = new Vector3();
            _startPoint = new Vector3();
            _endPoint = new Vector3();
            _endControlPoint = new Vector3();
            _ctrlPoints = new Vector3[_samplePointCount + 1];
            for (int i = 0; i < _ctrlPoints.Length; i++)
            {
                _ctrlPoints[i] = new Vector3();
            }
        }
        /// <summary>
        ///添加关节。将新控制点添加到控制点列表中，并更新前面的样条曲线。
        /// </summary>
        /// <param name="prevSpline">前一根样条曲线</param>
        /// <param name="currentPoint">当前点</param>
        public void AddJoint(SplineCurves prevSpline, Vector3 currentPoint)
        {
            //前一根样条曲线(prevSpline)为null，说明控制点列表中只有一个点，所以4个控制点样同。
            //当第2个及之后的控制点添加到控制点列表中时，那第1根样条曲线的Pk+1和Pk+2点需要更新
            if (null == prevSpline)
            {
                this._startControlPoint = currentPoint;
                this._startPoint = currentPoint;
                this._endPoint = currentPoint;
                this._endControlPoint = currentPoint;
                this._isFirst = true;
            }
            else//前一根样条曲线不为null，所以更新前一根样条曲线的控制点列表，同时更新当前样条曲线的控制点列表。
            {
                //前一根样条曲线是第1根样条曲线，更新它的Pk+1和Pk+2点
                if (true == prevSpline._isFirst)
                {
                    this._startControlPoint = prevSpline.StartControlPoint;
                    this._startPoint = prevSpline.StartPoint;
                    this._endPoint = currentPoint;
                    this._endControlPoint = currentPoint;
                    GenerateSamplePoint();
                    return;
                }
                else///前一根样条曲线不是第1根样条曲线，仅更新它的Pk+2点
                {
                    prevSpline.EndControlPoint = currentPoint;
                    prevSpline.GenerateSamplePoint();

                    //模拟当前样条曲线的样点
                    this._startControlPoint = prevSpline._startPoint;
                    this._startPoint = prevSpline._endPoint;
                    this._endPoint = currentPoint;
                    this._endControlPoint = currentPoint;
                    GenerateSamplePoint();

                }
            }
        }

        /// <summary>
        /// 使用基数算法生成样点
        /// </summary>
        public void GenerateSamplePoint()
        {
            Vector3 startControlPoint = this.StartControlPoint;
            Vector3 startPoint = this.StartPoint;
            Vector3 endPoint = this.EndPoint;
            Vector3 endControlPoint = this.EndControlPoint;
            float step = 1.0F / (float)_samplePointCount;
            float uValue = 0.00F;

            for (int i = 0; i < _samplePointCount; i++)
            {
                Vector3 pointNew = GenerateSimulatePoint(uValue, startControlPoint, startPoint, endPoint, endControlPoint);
                this.CtrlPoints[i] = pointNew;
                uValue += step;
            }
            this.CtrlPoints[_ctrlPoints.Length - 1] = endPoint;
        }
        #region GenerateSimulatePoint
        /// <summary>
        /// 生成曲线模拟点，该点在startPoint和endPoint之间
        /// </summary>
        /// <param name="u">介于0和1之间的变量</param>
        /// <param name="startControlPoint">起始点startPoint之前的控制点, 协助确定曲线的外观</param>
        /// <param name="startPoint">目标曲线的起始点startPoint,当u=0时，返回结果为起始点startPoint</param>
        /// <param name="endPoint">目标曲线的结束点endPoint, 当u=1时,返回结果为结束点endPoint</param>
        /// <param name="endControlPoint">在起结点startPoint之后的控制点, 协助确定曲线的外观</param>
        /// <returns>返回介于startPoint和endPoint的点</returns>
        private Vector3 GenerateSimulatePoint(float u,
                                Vector3 startControlPoint,
                                Vector3 startPoint,
                                Vector3 endPoint,
                                Vector3 endControlPoint)
        {
            float s = (1 - _tension) / 2;
            Vector3 resultPoint = new Vector3();
            resultPoint.x = CalculateAxisCoordinate(startControlPoint.x, startPoint.x, endPoint.x, endControlPoint.x, s, u);
            resultPoint.y = CalculateAxisCoordinate(startControlPoint.y, startPoint.y, endPoint.y, endControlPoint.y, s, u);
            return resultPoint;
        }

        /// <summary>
        /// 计算轴坐标
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        private float CalculateAxisCoordinate(float a, float b, float c, float d, float s, float u)
        {
            float result = 0.0F;
            result = a * (2 * s * u * u - s * u * u * u - s * u)
                   + b * ((2 - s) * u * u * u + (s - 3) * u * u + 1)
                   + c * ((s - 2) * u * u * u + (3 - 2 * s) * u * u + s * u)
                   + d * (s * u * u * u - s * u * u);
            return result;
        }
        /// <summary>
        /// 绘制样条曲线
        /// </summary>
        public void Draw()
        {
            for (int i = 0; i < _ctrlPoints.Length - 1; i++)
            {
                Gizmos.DrawLine(_ctrlPoints[i], _ctrlPoints[i + 1]);
            }
        }
        #endregion

        /// <summary>
        /// 获取样条曲线上的点
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="points"></param>
        public static List<Vector3> FetchPoints(Vector3[] points)
        {
            if (points == null || points.Length <= 0)
            {
                return null;
            }
            List<SplineCurves> _splines = new List<SplineCurves>();
            SplineCurves splineNew = null;
            SplineCurves lastNew = null;
            foreach (Vector3 nowPoint in points)
            {
                if (null == _splines || 0 == _splines.Count)
                {
                    splineNew = new SplineCurves();
                    splineNew.AddJoint(null, nowPoint);
                    _splines.Add(splineNew);
                }
                else
                {
                    splineNew = new SplineCurves();
                    lastNew = _splines[_splines.Count - 1] as SplineCurves;
                    splineNew.AddJoint(lastNew, nowPoint);
                    _splines.Add(splineNew);
                };
            }

            List<Vector3> _points = new List<Vector3>();
            foreach (SplineCurves spline in _splines)
            {
                if (spline.IsFirst)
                {
                    continue;
                }
                foreach (Vector3 point in spline.CtrlPoints)
                {
                    if (_points.Contains(point))
                    {
                        continue;
                    }

                    _points.Add(point);
                }
            }
            return _points;
        }
    }
}