using Game;
using System;
using UnityEngine;

namespace IronForce2.IF2World
{
	public class MovementArrowManager : MonoBehaviour
	{
		public Bounds CurrentBounds = new Bounds(Vector3.zero, Vector3.one);
		public float SpinRotateSpeed = 45;

		[Range(0, 1)] public float Curvature01 = 1;
		public float MaxTurnRadius = 3.5f;
		public float MinTurnRadius = 17;

		public MovementArrow Indicator0;

		public MovementArrow Indicator1;

        private MovementDirection direction;

        void Awake()
		{
			direction = MovementDirection.None;
		}

		void Update()
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
					SetActive(Indicator0.gameObject, false);
					SetActive(Indicator1.gameObject, false);
					break;
				case MovementDirection.Forward:
					SetActive(Indicator0.gameObject, true);
					SetActive(Indicator1.gameObject, false);
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.Forward);
					Indicator0.transform.localRotation = Quaternion.identity;
					Indicator1.transform.localRotation = Quaternion.identity;
					break;
				case MovementDirection.Back:
					SetActive(Indicator0.gameObject, true);
					SetActive(Indicator1.gameObject, false);
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.Back);
					Indicator0.transform.localRotation = Quaternion.identity;
					Indicator1.transform.localRotation = Quaternion.identity;
					break;
				case MovementDirection.LeftForward:
					SetActive(Indicator0.gameObject, true);
					SetActive(Indicator1.gameObject, false);
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.LeftForward);
					Indicator0.transform.localRotation = Quaternion.identity;
					Indicator1.transform.localRotation = Quaternion.identity;
					break;
				case MovementDirection.RightForward:
					SetActive(Indicator0.gameObject, true);
					SetActive(Indicator1.gameObject, false);
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.RightForward);
					Indicator0.transform.localRotation = Quaternion.identity;
					Indicator1.transform.localRotation = Quaternion.identity;
					break;
				case MovementDirection.LeftBack:
					SetActive(Indicator0.gameObject, true);
					SetActive(Indicator1.gameObject, false);
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.LeftBack);
					Indicator0.transform.localRotation = Quaternion.identity;
					Indicator1.transform.localRotation = Quaternion.identity;
					break;
				case MovementDirection.RightBack:
					SetActive(Indicator0.gameObject, true);
					SetActive(Indicator1.gameObject, false);
					Indicator0.OffsetForStraightAndTurn = CurrentBounds.extents.z + 1;
					Indicator0.ChangeDirection(MovementDirection.RightBack);
					Indicator0.transform.localRotation = Quaternion.identity;
					Indicator1.transform.localRotation = Quaternion.identity;
					break;
				case MovementDirection.Clockwise:
					SetActive(Indicator0.gameObject, true);
					SetActive(Indicator1.gameObject, true);
					Indicator0.ChangeDirection(MovementDirection.Clockwise, true);
					Indicator1.ChangeDirection(MovementDirection.Clockwise);
					Indicator0.SpinRadius = CurrentBounds.extents.z;
					Indicator1.SpinRadius = CurrentBounds.extents.z;
					Indicator0.transform.Rotate(0, SpinRotateSpeed * Time.deltaTime, 0);
					Indicator1.transform.Rotate(0, SpinRotateSpeed * Time.deltaTime, 0);
					break;
				case MovementDirection.Counterclockwise:
					SetActive(Indicator0.gameObject, true);
					SetActive(Indicator1.gameObject, true);
					Indicator0.ChangeDirection(MovementDirection.Counterclockwise, true);
					Indicator1.ChangeDirection(MovementDirection.Counterclockwise);
					Indicator0.SpinRadius = CurrentBounds.extents.z;
					Indicator1.SpinRadius = CurrentBounds.extents.z;
					Indicator0.transform.Rotate(0, -SpinRotateSpeed * Time.deltaTime, 0);
					Indicator1.transform.Rotate(0, -SpinRotateSpeed * Time.deltaTime, 0);
					break;
				default:
					throw new ArgumentOutOfRangeException("CurrentMoveType", direction, null);
			}
		}

		private static void SetActive(GameObject go, bool active)
		{
			if (go.activeInHierarchy != active)
			{
				go.SetActive(active);
			}
		}
	}
}