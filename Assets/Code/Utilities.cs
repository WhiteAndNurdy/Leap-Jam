using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

public static class Utilities
{
    public static LevelFromXml LoadLevelFile(string levelName)
    {
        LevelFromXml level = new LevelFromXml();
        
        TextAsset text = Resources.Load(levelName) as TextAsset;
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
                var enemies = group.Elements("Enemies");
    
                foreach(var enemy in enemies)
                {
                    groupToAdd.enemies.Add(new Enemy());
                    //enemy.Attribute("X"),
                      //                                int.Parse(enemy.Attribute("Health").Value),
                        //                              bool.Parse(enemy.Attribute("Shield").Value)));
                }
                waveToAdd.groups.Add(groupToAdd);
            }
    
            level.waves.Add(waveToAdd);
        }

        return level;
    }
}
