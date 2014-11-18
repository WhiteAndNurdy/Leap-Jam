using UnityEngine;
using System.Collections;

public class GroupPath : AIPath 
{
	private GroupLogic logic;

	protected override void Awake()
	{
		base.Awake();
		logic = transform.parent.GetComponent<GroupLogic>();
	}

	public override void Update()
	{
		if (!canMove) { return; }

		if (logic != null && logic.MovementState != AIMovementState.Moving) { return; }
		Vector3 dir = CalculateVelocity(GetFeetPosition());

		//Rotate towards targetDirection (filled in by CalculateVelocity)
		RotateTowards(targetDirection);

		if (navController != null)
		{
		}
		else if (controller != null)
		{
			controller.SimpleMove(dir);
		}
		else if (rigid != null)
		{
			rigid.AddForce(dir);
		}
		else
		{
			transform.Translate(dir * Time.deltaTime, Space.World);
		}
	}
}
