using UnityEngine;
using System.Collections;

public class Base_Choice : MonoBehaviour {

    protected bool _ChoiceMade = false;
    public bool ChoiceMade
    {
        get { return _ChoiceMade; }
    }

    public void Reset ()
    {
        _ChoiceMade = false;
    }

    // Use this for initialization
    protected virtual void Start () {
    
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
    
    }

    public virtual Choice Draw()
    {
        // return invalid output
        return Choice.Count;
    }
}
