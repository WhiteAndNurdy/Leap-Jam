using UnityEngine;
using System.Collections;

public class SpellLogic : MonoBehaviour {

	public float TEMP__LifeTime = 1.0f;
	// Use this for initialization
	void Start () {
		GameObject.Destroy(gameObject, TEMP__LifeTime);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
