using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour 
{
	public float maxHealth = 100;
	public float health = 100;
	public Texture2D texture;
	private Vector2 screenPos;
	private float height = 6;
	private float width = 30;

	void Start () 
	{
		renderer.material = Resources.Load("Materials/Healthbar") as Material;
	}
	
	void Update () 
	{
		width = maxHealth / 3;

		//Get the screenposition of the object
		screenPos = Camera.main.WorldToScreenPoint(transform.position);

		float healthCutoff = 1.0f - (health / maxHealth);
		if (healthCutoff < 0.01f)
		{
			healthCutoff = 0.01f;
		}
		renderer.material.SetFloat("_Cutoff", healthCutoff);
	}

	void OnGUI()
	{
		Rect rect = new Rect(screenPos.x - width / 2, Screen.height - (screenPos.y + height / 2), width, height);
		Graphics.DrawTexture(rect, texture, renderer.material);
	}

	void TakeDamage(int damage)
	{
		health -= damage;
	}
}
