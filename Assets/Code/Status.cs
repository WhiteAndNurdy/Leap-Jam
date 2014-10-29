using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour {

	private WinStatus _status;
	public WinStatus PlayerStatus
	{
		get { return _status; }
		set { _status = value; }
	}
}
