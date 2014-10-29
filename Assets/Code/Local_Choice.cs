using UnityEngine;
using System.Collections;

public class Local_Choice : Base_Choice
{
    private Choice localChoice;
    private string stringChoice;

    
    // Use this for initialization
    protected override void Start () 
    {
        base.Start();
        stringChoice = "";
    }
    
    // Update is called once per frame
    protected override void Update() 
    {
        base.Update();
    }

    bool Choose (string stringChoice)
    {
        if (string.Compare(stringChoice, "Rock", true) == 0)
        {
            localChoice = Choice.Rock;
        }
        else if (string.Compare(stringChoice, "Paper", true) == 0)
        {
            localChoice = Choice.Paper;
        }
        else if (string.Compare(stringChoice, "Scissors", true) == 0)
        {
            localChoice = Choice.Scissors;
        }
        else
        {
            return false;
        }
        _ChoiceMade = true;
        return true;
    }

    void OnGUI ()
    {
        stringChoice = GUI.TextField(new Rect(10, 10, 200, 20), stringChoice);
        if (GUI.Button(new Rect(220, 10, 50, 20), "Go!"))
        {
            if (!Choose(stringChoice))
            {
                stringChoice = "INVALID INPUT!";
            }
        }
    }

    public override Choice Draw ()
    {
        return localChoice;
    }
}
