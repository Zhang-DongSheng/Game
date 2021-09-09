using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// B样条曲线
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
        /// 添加关节。将新控制点添加到控制点列表中，并更新前面的样条曲线。
        /// </summary>
        /// <param name="spline">上一条样条曲线</param>
        /// <param name="point">当前点</param>
        public void AddJoint(SplineCurves spline, Vector3 point)
        {
            //前一根样条曲线不为null，所以更新前一根样条曲线的控制点列表，同时更新当前样条曲线的控制点列表。
            if (spline != null)
            {
                //前一根样条曲线是第1根样条曲线，更新它的Pk+1和Pk+2点
                if (spline.First)
                {
                    origin = spline.origin;
                    start = spline.start;
                    end = point;
                    destination = point;
                    GenerateSamplePoint();
                    return;
                }
                //前一根样条曲线不是第1根样条曲线，仅更新它的Pk+2点
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
            //前一根样条曲线spline为null，说明控制点列表中只有一个点，所以4个控制点样同。
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
        /// 使用基数算法生成样点
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
        /// 生成曲线模拟点，该点在start和end之间
        /// </summary>
        /// <param name="origin">起始点start之前的控制点, 协助确定曲线的外观</param>
        /// <param name="start">目标曲线的起始点start,当step=0时，返回结果为起始点start</param>
        /// <param name="end">目标曲线的结束点end, 当step=1时,返回结果为结束点end</param>
        /// <param name="destination">在起结点start之后的控制点, 协助确定曲线的外观</param>
        /// <param name="step">介于0和1之间的变量</param>
        /// <returns>返回介于start和end的点</returns>
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
        /// 计算轴坐标
        /// </summary>
        private float CalculateAxisCoordinate(float o, float s, float e, float d, float r, float t)
        {
            return o * (2 * r * t * t - r * t * t * t - r * t)
                   + s * ((2 - r) * t * t * t + (r - 3) * t * t + 1)
                   + e * ((r - 2) * t * t * t + (3 - 2 * r) * t * t + r * t)
                   + d * (r * t * t * t - r * t * t);
        }
        /// <summary>
        /// 获取样条曲线
        /// </summary>
        /// <param name="points">基准点</param>
        /// <returns>样条曲线</returns>
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