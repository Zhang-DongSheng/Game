using Game;
using UnityEngine;

namespace IronForce2.IF2World
{
    public class MovementArrow : MonoBehaviour
    {
        // 对 前进 后退 有用。
        public float IntervalDistance = 1;
        // 对 前进 后退 和 各种移动中转向有用
        public float OffsetForStraightAndTurn = 2;
        // 控制非自传的参数
        public float TurnRadius = 5;
        // 控制自传的参数
        public float SpinRadius = 2;

        public float SpinIntervalAngle = 20;

        private MovementDirection direction;

        private bool forward;

        public void ChangeDirection(MovementDirection direction, bool forward = false)
        {
            this.direction = direction;

            this.forward = forward;

            UpdatePresentation();

            SetActive(true);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);

            if (!active)
            {
                transform.localRotation = Quaternion.identity;
            }
        }

        private void UpdatePresentation()
        {
            float turnIntervalAngle = IntervalDistance * 360 / (2 * Mathf.PI * TurnRadius);

            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);

                switch (direction)
                {
                    case MovementDirection.Forward:
                        {
                            child.localPosition = Vector3.forward * (OffsetForStraightAndTurn + IntervalDistance * i);
                            child.localRotation = Quaternion.LookRotation(Vector3.forward);
                        }
                        break;
                    case MovementDirection.Back:
                        {
                            child.localPosition = Vector3.back * (OffsetForStraightAndTurn + IntervalDistance * i);
                            child.localRotation = Quaternion.LookRotation(Vector3.back);
                        }
                        break;
                    case MovementDirection.LeftForward:
                        {
                            Vector3 circleCenter = Vector3.left * TurnRadius + OffsetForStraightAndTurn * Vector3.forward;

                            float x;
                            float y;
                            GetPointOnEllipse(out x, out y, TurnRadius, TurnRadius, turnIntervalAngle * i * Mathf.Deg2Rad);

                            child.localPosition = new Vector3(x, 0, y) + circleCenter;
                            child.localRotation = Quaternion.LookRotation(Vector3.Cross(Vector3.down, new Vector3(x, 0, y)));
                        }
                        break;
                    case MovementDirection.RightForward:
                        {
                            Vector3 circleCenter = Vector3.right * TurnRadius + OffsetForStraightAndTurn * Vector3.forward;

                            float x;
                            float y;
                            GetPointOnEllipse(out x, out y, TurnRadius, TurnRadius, (180 - turnIntervalAngle * i) * Mathf.Deg2Rad);

                            child.localPosition = new Vector3(x, 0, y) + circleCenter;
                            child.localRotation = Quaternion.LookRotation(Vector3.Cross(Vector3.up, new Vector3(x, 0, y)));
                        }
                        break;
                    case MovementDirection.LeftBack:
                        {
                            Vector3 circleCenter = Vector3.left * TurnRadius + OffsetForStraightAndTurn * Vector3.back;

                            float x;
                            float y;
                            GetPointOnEllipse(out x, out y, TurnRadius, TurnRadius, -turnIntervalAngle * i * Mathf.Deg2Rad);

                            child.localPosition = new Vector3(x, 0, y) + circleCenter;
                            child.localRotation = Quaternion.LookRotation(Vector3.Cross(Vector3.up, new Vector3(x, 0, y)));
                        }
                        break;
                    case MovementDirection.RightBack:
                        {
                            Vector3 circleCenter = Vector3.right * TurnRadius + OffsetForStraightAndTurn * Vector3.back;

                            float x;
                            float y;
                            GetPointOnEllipse(out x, out y, TurnRadius, TurnRadius, (180 + turnIntervalAngle * i) * Mathf.Deg2Rad);

                            child.localPosition = new Vector3(x, 0, y) + circleCenter;
                            child.localRotation = Quaternion.LookRotation(Vector3.Cross(Vector3.down, new Vector3(x, 0, y)));
                        }
                        break;
                    case MovementDirection.Clockwise:
                        {
                            if (forward)
                            {
                                float x;
                                float y;
                                float totleAngle = SpinIntervalAngle * childCount;

                                GetPointOnEllipse(out x, out y, SpinRadius, SpinRadius,
                                    (180 + totleAngle / 2 - SpinIntervalAngle * i) * Mathf.Deg2Rad);

                                child.localPosition = new Vector3(x, 0, y);
                                child.localRotation = Quaternion.LookRotation(Vector3.Cross(Vector3.up, new Vector3(x, 0, y)));
                            }
                            else
                            {
                                float x;
                                float y;
                                float totleAngle = SpinIntervalAngle * childCount;

                                GetPointOnEllipse(out x, out y, SpinRadius, SpinRadius,
                                    (-totleAngle / 2 + SpinIntervalAngle * i) * Mathf.Deg2Rad);

                                child.localPosition = new Vector3(x, 0, y);
                                child.localRotation = Quaternion.LookRotation(Vector3.Cross(Vector3.up, new Vector3(x, 0, y)));
                            }
                        }
                        break;
                    case MovementDirection.Counterclockwise:
                        {
                            if (forward)
                            {
                                float x;
                                float y;
                                float totleAngle = SpinIntervalAngle * childCount;

                                GetPointOnEllipse(out x, out y, SpinRadius, SpinRadius,
                                    (180 + totleAngle / 2 - SpinIntervalAngle * i) * Mathf.Deg2Rad);

                                child.localPosition = new Vector3(x, 0, y);
                                child.localRotation = Quaternion.LookRotation(Vector3.Cross(Vector3.down, new Vector3(x, 0, y)));
                            }
                            else
                            {
                                float x;
                                float y;
                                float totleAngle = SpinIntervalAngle * childCount;

                                GetPointOnEllipse(out x, out y, SpinRadius, SpinRadius,
                                    (-totleAngle / 2 + SpinIntervalAngle * i) * Mathf.Deg2Rad);

                                child.localPosition = new Vector3(x, 0, y);
                                child.localRotation = Quaternion.LookRotation(Vector3.Cross(Vector3.down, new Vector3(x, 0, y)));
                            }
                        }
                        break;
                }
            }
        }

        private void GetPointOnEllipse(out float x, out float y, float rightRadius, float forwardRadius, float angle)
        {
            x = rightRadius * Mathf.Cos(angle);
            y = forwardRadius * Mathf.Sin(angle);
        }
    }
}