using UnityEngine;
using System.Collections;

public class TowerLogic : EntityLogic
{

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	public override void Die()
	{
		base.Die();
		Debug.Log("Tower Died!");
	}
}
