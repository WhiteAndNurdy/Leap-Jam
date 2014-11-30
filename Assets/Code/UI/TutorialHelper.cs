using UnityEngine;
using System;
using System.Collections;

enum TutorialState
{
	Start,
	Intro,
	Aim,
	Spell,
	Air,
	Water,
	Earth,
	Fire,
	Finished,
	End
}

public class TutorialHelper : MonoBehaviour {

	private string text;
	private GUIText guiText;
	private TutorialState state;
	private HandController handController;


	void Awake()
	{
		guiText = GetComponent<GUIText>();
		state = TutorialState.Start;
		handController = GameObject.Find("RECORDEDCONTROLLER").GetComponent<HandController>();
	}

	// Use this for initialization
	void Start () 
	{
		Next();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Next();
		}
	}

	void Next()
	{
		if(state == TutorialState.End)
		{
			return;
		}
		state++;
		switch (state)
		{
			case TutorialState.Intro:
				text = "Welcome to the tutorial. Press Return to go to the next step in the tutorial.";
				handController.StopRecording();
				handController.DestroyAllHands();
				handController.gameObject.SetActive(false);
				break;
			case TutorialState.Aim:
				text = "Use your ";
				text += Convert.ToBoolean(PlayerPrefs.GetInt("Leftie"))? "right" : "left";
				text += " hand to aim";
				handController.recordingAsset = Resources.Load("Recordings/aim.bytes", typeof(TextAsset)) as TextAsset;
				handController.gameObject.SetActive(true);
				handController.PlayRecording();
				break;
			case TutorialState.Spell:
				text = "Use your ";
				text += Convert.ToBoolean(PlayerPrefs.GetInt("Leftie")) ? "left" : "right";
				text += " hand to cast spells";
				text += "\n Press return to start learning spells";
				handController.DestroyAllHands();				
				handController.StopRecording();
				handController.gameObject.SetActive(false);
				break;
			case TutorialState.Air:
				text = "Cast air spell:";
				break;
			case TutorialState.Water:
				text = "Cast water spell:";
				break;
			case TutorialState.Earth:
				text = "Cast earth spell:";
				break;
			case TutorialState.Fire:
				text = "Cast water spell:";
				break;
			case TutorialState.Finished:
				text = "That's it! Get ready to play!";
				break;
		}
		guiText.text = text;
	}
}
