using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// B��������
    /// </summary>
    public class SplineCurves
    {
        private readonly float coefficient = 0;

        private Vector3 origin, destination;

        private Vector3 start, end;

        private readonly int multiple;

        private readonly Vector3[] points;

        public bool First { get; private set; }

        public SplineCurves(int ratio)
        {
            multiple = ratio;

            origin = new Vector3();

            destination = new Vector3();

            start = new Vector3();

            end = new Vector3();

            points = new Vector3[multiple + 1];

            for (int i = 0; i < multiple; i++)
            {
                points[i] = new Vector3();
            }
        }
        /// <summary>
        /// ��ӹؽڡ����¿��Ƶ���ӵ����Ƶ��б��У�������ǰ����������ߡ�
        /// </summary>
        /// <param name="spline">��һ����������</param>
        /// <param name="point">��ǰ��</param>
        public void AddJoint(SplineCurves spline, Vector3 point)
        {
            //ǰһ���������߲�Ϊnull�����Ը���ǰһ���������ߵĿ��Ƶ��б�ͬʱ���µ�ǰ�������ߵĿ��Ƶ��б�
            if (spline != null)
            {
                //ǰһ�����������ǵ�1���������ߣ���������Pk+1��Pk+2��
                if (spline.First)
                {
                    origin = spline.origin;
                    start = spline.start;
                    end = point;
                    destination = point;
                    GenerateSamplePoint();
                    return;
                }
                //ǰһ���������߲��ǵ�1���������ߣ�����������Pk+2��
                else
                {
                    spline.destination = point;
                    spline.GenerateSamplePoint();

                    origin = spline.start;
                    start = spline.end;
                    end = point;
                    destination = point;
                    GenerateSamplePoint();
                }
            }
            //ǰһ����������splineΪnull��˵�����Ƶ��б���ֻ��һ���㣬����4�����Ƶ���ͬ��
            else
            {
                origin = point;
                start = point;
                end = point;
                destination = point;
                First = true;
            }
        }
        /// <summary>
        /// ʹ�û����㷨��������
        /// </summary>
        private void GenerateSamplePoint()
        {
            float ratio = 1.0f / multiple;

            float step = 0;

            for (int i = 0; i < multiple; i++)
            {
                points[i] = GenerateSamplePoint(origin, start, end, destination, step);
                step += ratio;
            }
            points[points.Length - 1] = end;
        }
        /// <summary>
        /// ��������ģ��㣬�õ���start��end֮��
        /// </summary>
        /// <param name="origin">��ʼ��start֮ǰ�Ŀ��Ƶ�, Э��ȷ�����ߵ����</param>
        /// <param name="start">Ŀ�����ߵ���ʼ��start,��step=0ʱ�����ؽ��Ϊ��ʼ��start</param>
        /// <param name="end">Ŀ�����ߵĽ�����end, ��step=1ʱ,���ؽ��Ϊ������end</param>
        /// <param name="destination">������start֮��Ŀ��Ƶ�, Э��ȷ�����ߵ����</param>
        /// <param name="step">����0��1֮��ı���</param>
        /// <returns>���ؽ���start��end�ĵ�</returns>
        private Vector3 GenerateSamplePoint(Vector3 origin, Vector3 start, Vector3 end, Vector3 destination, float step)
        {
            float ratio = (1 - this.coefficient) / 2f;

            Vector3 point = new Vector3()
            {
                x = CalculateAxisCoordinate(origin.x, start.x, end.x, destination.x, ratio, step),

                y = CalculateAxisCoordinate(origin.y, start.y, end.y, destination.y, ratio, step),

                z = CalculateAxisCoordinate(origin.z, start.z, end.z, destination.z, ratio, step),
            };

            return point;
        }
        /// <summary>
        /// ����������
        /// </summary>
        private float CalculateAxisCoordinate(float o, float s, float e, float d, float r, float t)
        {
            return o * (2 * r * t * t - r * t * t * t - r * t)
                   + s * ((2 - r) * t * t * t + (r - 3) * t * t + 1)
                   + e * ((r - 2) * t * t * t + (3 - 2 * r) * t * t + r * t)
                   + d * (r * t * t * t - r * t * t);
        }
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="points">��׼��</param>
        /// <returns>��������</returns>
        public static List<Vector3> FetchCurves(int ratio, params Vector3[] points)
        {
            if (points == null || points.Length == 0) return null;

            List<SplineCurves> splines = new List<SplineCurves>();

            SplineCurves current, pre;

            foreach (var point in points)
            {
                if (splines.Count == 0)
                {
                    current = new SplineCurves(ratio);
                    current.AddJoint(null, point);
                    splines.Add(current);
                }
                else
                {
                    current = new SplineCurves(ratio);
                    pre = splines[splines.Count - 1];
                    current.AddJoint(pre, point);
                    splines.Add(current);
                }
            }

            List<Vector3> curves = new List<Vector3>();

            foreach (SplineCurves spline in splines)
            {
                if (spline.First)
                {
                    continue;
                }
                foreach (Vector3 point in spline.points)
                {
                    if (curves.Contains(point))
                    {
                        continue;
                    }
                    curves.Add(point);
                }
            }
            return curves;
        }
    }
}