using System;

public class DebugUtils 
{ 
    static public void Assert(bool condition)
    {
#if UNITY_EDITOR
        if (!condition) throw new Exception(); 
#endif 
    } 
}