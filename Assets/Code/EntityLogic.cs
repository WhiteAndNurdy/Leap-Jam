using UnityEngine;
using System.Collections;

public class EntityLogic : MonoBehaviour {

	// Use this for initialization
	protected virtual void Start () 
	{
	
	}
	
	// Update is called once per frame
	protected virtual void Update()
	{
	
	}

	public virtual void Die()
	{

	}

	public virtual void Damage(float amount)
	{
		HealthBarScript healthBar = GetComponentInChildren<HealthBarScript>();
		if(healthBar != null)
			healthBar.TakeDamage(amount);
	}
}
