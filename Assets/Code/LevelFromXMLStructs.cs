﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelFromXML
{
    public List<Wave> waves;
    public LevelFromXML()
    {
        waves = new List<Wave>();
    }

}

public class Wave
{
    public List<Group> groups;
    public Wave()
    {
        groups = new List<Group>();
    }
}

public class Group
{
    public List<EnemyData> enemies;
    public Group()
    {
        enemies = new List<EnemyData>();
    }
}

public class EnemyData
{
    public string type;
    public int health;
    public bool hasShield;

    public EnemyData(string type, int health, bool hasShield)
    {
        this.type = type;
        this.health = health;
        this.hasShield = hasShield;
    }
}