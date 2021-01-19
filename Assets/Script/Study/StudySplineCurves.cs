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
    /// �������ߡ�ÿ���������߰���4�����Ƶ�
    /// </summary>
    public class SplineCurves
    {
        /// <summary>
        /// ���������ڵ�Pk��Pk+1֮�䣬�����������ɸ����㡣����"u"�����0.00F������0.05F.
        /// </summary>
        private static readonly int _samplePointCount = 20;

        /// <summary>
        /// �ڻ����㷨�е�t
        /// </summary>
        private static readonly float _tension = 0.0F;


        #region ����
        private Vector3 _startControlPoint;

        /// <summary>
        /// "Pk-1"��(��ʼ���Ƶ�)
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
        ///  "Pk"��(��ʼ��)
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
        /// "Pk+1"��(������)
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
        /// "Pk+2"��(�������Ƶ�)
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
        /// ���ߵ�(���Ƶ㼰ģ�������)
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
        /// ��ʶ��ǰ���������Ƿ��ǵ�һ���������m_startControlPoint �� m_startPoint������ͬ��
        /// ��Ϊ��Pk��Pk+1֮����Ҫ4�����������������ߣ�����������Ҫ��Pk-1��ǰ�ֶ����һ���㡣
        /// �������ǲ�����Pk-1��Pk+1֮������������ߡ�
        /// ͬ���ģ����һ���������ߵ�Pk+2���������"Pk+1"����ͬ��
        /// �������ǲ�����Pk+1��Pk+2֮������������ߡ�
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
        ///��ӹؽڡ����¿��Ƶ���ӵ����Ƶ��б��У�������ǰ����������ߡ�
        /// </summary>
        /// <param name="prevSpline">ǰһ����������</param>
        /// <param name="currentPoint">��ǰ��</param>
        public void AddJoint(SplineCurves prevSpline, Vector3 currentPoint)
        {
            //ǰһ����������(prevSpline)Ϊnull��˵�����Ƶ��б���ֻ��һ���㣬����4�����Ƶ���ͬ��
            //����2����֮��Ŀ��Ƶ���ӵ����Ƶ��б���ʱ���ǵ�1���������ߵ�Pk+1��Pk+2����Ҫ����
            if (null == prevSpline)
            {
                this._startControlPoint = currentPoint;
                this._startPoint = currentPoint;
                this._endPoint = currentPoint;
                this._endControlPoint = currentPoint;
                this._isFirst = true;
            }
            else//ǰһ���������߲�Ϊnull�����Ը���ǰһ���������ߵĿ��Ƶ��б�ͬʱ���µ�ǰ�������ߵĿ��Ƶ��б�
            {
                //ǰһ�����������ǵ�1���������ߣ���������Pk+1��Pk+2��
                if (true == prevSpline._isFirst)
                {
                    this._startControlPoint = prevSpline.StartControlPoint;
                    this._startPoint = prevSpline.StartPoint;
                    this._endPoint = currentPoint;
                    this._endControlPoint = currentPoint;
                    GenerateSamplePoint();
                    return;
                }
                else///ǰһ���������߲��ǵ�1���������ߣ�����������Pk+2��
                {
                    prevSpline.EndControlPoint = currentPoint;
                    prevSpline.GenerateSamplePoint();

                    //ģ�⵱ǰ�������ߵ�����
                    this._startControlPoint = prevSpline._startPoint;
                    this._startPoint = prevSpline._endPoint;
                    this._endPoint = currentPoint;
                    this._endControlPoint = currentPoint;
                    GenerateSamplePoint();

                }
            }
        }

        /// <summary>
        /// ʹ�û����㷨��������
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
        /// ��������ģ��㣬�õ���startPoint��endPoint֮��
        /// </summary>
        /// <param name="u">����0��1֮��ı���</param>
        /// <param name="startControlPoint">��ʼ��startPoint֮ǰ�Ŀ��Ƶ�, Э��ȷ�����ߵ����</param>
        /// <param name="startPoint">Ŀ�����ߵ���ʼ��startPoint,��u=0ʱ�����ؽ��Ϊ��ʼ��startPoint</param>
        /// <param name="endPoint">Ŀ�����ߵĽ�����endPoint, ��u=1ʱ,���ؽ��Ϊ������endPoint</param>
        /// <param name="endControlPoint">������startPoint֮��Ŀ��Ƶ�, Э��ȷ�����ߵ����</param>
        /// <returns>���ؽ���startPoint��endPoint�ĵ�</returns>
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
        /// ����������
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
        /// ������������
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
        /// ��ȡ���������ϵĵ�
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