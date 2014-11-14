using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

public static class Utilities
{
    public static LevelFromXML LoadLevelFile(string levelName)
    {
        LevelFromXML level = new LevelFromXML();
        string levelFilePath = "Levels/" + levelName;

        TextAsset text = Resources.Load(levelFilePath) as TextAsset;
        XDocument xdoc = XDocument.Parse(text.text);

        XElement mainElement = xdoc.Element(levelName);
        
        //Read stuff in
        var waves = mainElement.Element("Waves").Elements("Wave");
        foreach(var wave in waves)
        {
            var waveToAdd = new Wave();
            var groups = wave.Elements("Group");    
       
            foreach(var group in groups)
            {
                var groupToAdd = new Group();
                var enemies = group.Elements("Enemy");
    
                foreach(var enemy in enemies)
                {
                    string type = enemy.Attribute("type").Value;
                    int health = int.Parse(enemy.Attribute("health").Value);
                    bool hasShield = bool.Parse(enemy.Attribute("hasShield").Value);

                    var enemyDataToAdd = new EnemyData(type, health, hasShield);

                    groupToAdd.enemies.Add(enemyDataToAdd);
                    
                }
                waveToAdd.groups.Add(groupToAdd);
            }
            level.waves.Add(waveToAdd);
        }
        return level;
    }
}
