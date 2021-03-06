﻿using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour 
{
	public Texture2D texture;
	public float AlphaCutoffEpsilon = 0.01f;

	private float maxHealth;
	private float health;
	private Vector2 screenPos;
	private float height = 6;
	private float width = 30;

	void Start () 
	{
		renderer.material = Resources.Load("Materials/Healthbar") as Material;
		maxHealth = transform.parent.GetComponent<EntityProperties>().HealthPoints;
		health = maxHealth;
	}
	
	void Update () 
	{
		width = maxHealth / 3;

		//Get the screenposition of the object
		screenPos = Camera.main.WorldToScreenPoint(transform.position);

		float healthCutoff = 1.0f - (health / maxHealth);
		if (healthCutoff < AlphaCutoffEpsilon)
		{
			healthCutoff = AlphaCutoffEpsilon;
		}
		renderer.material.SetFloat("_Cutoff", healthCutoff);
	}

	void OnGUI()
	{
		Rect rect = new Rect(screenPos.x - width / 2, Screen.height - (screenPos.y + height / 2), width, height);
		Graphics.DrawTexture(rect, texture, renderer.material);
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			transform.parent.GetComponent<EntityLogic>().Die();
		}
	}
}
