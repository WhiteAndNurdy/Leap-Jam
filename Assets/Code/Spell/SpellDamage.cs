using UnityEngine;
using System.Collections;

public class SpellDamage : MonoBehaviour {

	public Elements SpellType;
	public float MinimumDamageAmount;
	public float MaximumDamageAmount;

	float m_Scale;
	SpellLogic m_SpellLogic;

	void Awake()
	{
		m_Scale = transform.localScale.x;
		m_SpellLogic = GetComponent<SpellLogic>();
	}

	void Start()
	{
		DebugUtils.Assert(m_SpellLogic != null, "Couldn't find SpellLogic component");
	}

	// Use this for initialization
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			other.gameObject.GetComponent<EnemyLogic>().Damage(CalculateDamage(), SpellType);
			Debug.Log("Actual damage dealt: " + CalculateDamage());
		}
	}

	float CalculateDamage()
	{
		float scale = (transform.localScale.x / m_Scale);
		scale = scale * (1 - m_SpellLogic.MinimumScale) + m_SpellLogic.MinimumScale;
		Debug.Log("Scale: " + scale);
		return scale * (MaximumDamageAmount - MinimumDamageAmount) + MinimumDamageAmount;
	}


}
