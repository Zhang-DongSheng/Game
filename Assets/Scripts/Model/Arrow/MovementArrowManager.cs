using System;
using UnityEngine;

namespace Game.Model
{
    public class MovementArrowManager : ItemBase
    {
        public MovementArrow Indicator0;

        public MovementArrow Indicator1;

        public Bounds CurrentBounds = new Bounds(Vector3.zero, Vector3.one);

		public float SpinRotateSpeed = 45;
		
		public float MaxTurnRadius = 3.5f;

		public float MinTurnRadius = 17;

        [Range(0, 1)] public float Curvature01 = 1;

        private MovementDirection direction;

        protected override void OnAwake()
        {
			direction = MovementDirection.None;
		}

        protected override void OnUpdate(float delta)
        {
            UpdatePresentation();
        }

		public void SetCurrentInputType(MovementDirection moveType)
		{
			direction = moveType;
        }

		private void UpdatePresentation()
		{
			Indicator0.TurnRadius = Mathf.Lerp(MinTurnRadius, MaxTurnRadius, Curvature01);

			switch (direction)
			{
				case MovementDirection.None:
					Indicator0.Display(false);
					Indicator1.Display(false);
                    break;
				case MovementDirection.Forward:
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.Forward);
                    Indicator1.Display(false);
                    break;
				case MovementDirection.Back:
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.Back);
                    Indicator1.Display(false);
                    break;
				case MovementDirection.LeftForward:
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.LeftForward);
                    Indicator1.Display(false);
                    break;
				case MovementDirection.RightForward:
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.RightForward);
                    Indicator1.Display(false);
                    break;
				case MovementDirection.LeftBack:
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.LeftBack);
                    Indicator1.Display(false);
                    break;
				case MovementDirection.RightBack:
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.RightBack);
                    Indicator1.Display(false);
					break;
				case MovementDirection.Clockwise:
                    Indicator0.SpinRadius = CurrentBounds.extents.z;
                    Indicator1.SpinRadius = CurrentBounds.extents.z;
                    Indicator0.ChangeDirection(MovementDirection.Clockwise, true);
					Indicator1.ChangeDirection(MovementDirection.Clockwise);
                    Indicator0.transform.Rotate(0, SpinRotateSpeed * Time.deltaTime, 0);
                    Indicator1.transform.Rotate(0, SpinRotateSpeed * Time.deltaTime, 0);
                    break;
				case MovementDirection.Counterclockwise:
                    Indicator0.SpinRadius = CurrentBounds.extents.z;
                    Indicator1.SpinRadius = CurrentBounds.extents.z;
                    Indicator0.ChangeDirection(MovementDirection.Counterclockwise, true);
					Indicator1.ChangeDirection(MovementDirection.Counterclockwise);
                    Indicator0.transform.Rotate(0, -SpinRotateSpeed * Time.deltaTime, 0);
                    Indicator1.transform.Rotate(0, -SpinRotateSpeed * Time.deltaTime, 0);
                    break;
				default:
					throw new ArgumentOutOfRangeException("CurrentMoveType", direction, null);
			}
		}
	}
}