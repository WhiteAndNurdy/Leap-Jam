using UnityEngine;
using System.Collections;

public class AI_Choice : Base_Choice
{

    // Use this for initialization
    protected override void Start () 
    {
        base.Start();
    }
    
    // Update is called once per frame
    protected override void Update () 
    {
        _ChoiceMade = true;
        base.Update();
    }

    public override Choice Draw()
    {
        int randomNumber = Random.Range(0, (int)Choice.Count);
        return (Choice)randomNumber;
    }
}
