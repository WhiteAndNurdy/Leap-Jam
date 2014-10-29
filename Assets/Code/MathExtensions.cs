using UnityEngine;
using System.Collections;

public class MathExtensions
{
    static public int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}
