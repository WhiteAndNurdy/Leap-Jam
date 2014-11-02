using UnityEngine;
using System.Collections;

public class TowerLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Damage(float amount)
	{
		GetComponentInChildren<HealthBarScript>().TakeDamage(amount);
	}
}
