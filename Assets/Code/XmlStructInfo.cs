using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelFromXml : MonoBehaviour 
{
    public List<Wave> waves;

    public LevelFromXml()
    {
        waves = new List<Wave>();
    }
}

public class Wave : MonoBehaviour
{
    public List<Group> groups;

    public Wave()
    {
        groups = new List<Group>();
    }
}

public class Group : MonoBehaviour
{
    public List<Enemy> enemies;

    public Group()
    {
        enemies = new List<Enemy>();
    }
}

public class Enemy : MonoBehaviour
{
    public Enemy()
    {
    }
}