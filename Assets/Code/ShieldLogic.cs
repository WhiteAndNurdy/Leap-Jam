using UnityEngine;
using System.Collections;

public class ShieldLogic : MonoBehaviour {

	public Elements[] ShieldSequence;

	private int m_Index = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Damage(Elements type)
	{
		if (ShieldSequence[m_Index] == type)
		{
			m_Index++;
		}
		else
		{
			m_Index = 0;
		}

		if (m_Index >= ShieldSequence.Length)
		{
			gameObject.SetActive(false);
		}
	}
}
