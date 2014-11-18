using UnityEngine;
using System.Collections;

public class EnemyPath : AIPath 
{
	private GroupLogic logic;

	protected override void Awake()
	{
		base.Awake();
		logic = GetComponent<GroupLogic>();
	}

	public override void Update()
	{
		if (!canMove) { return; }

		Vector3 dir = CalculateVelocity(GetFeetPosition());

		//Rotate towards targetDirection (filled in by CalculateVelocity)
		RotateTowards(targetDirection);

		if (navController != null)
		{
		}
		else if (logic != null)
		{
			if(transform.GetComponent<GroupLogic>().MovementState == AIMovementState.Moving)
				logic.Move(dir);
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
