using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

public class LevelExport : EditorWindow 
{
    [MenuItem("Custom Editor/Export Level")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LevelExport));
    }

    Vector2 scrollPosition = Vector2.zero;
    string fileName = "LevelX.xml";
    
    int selectedWave = 0;
    int selectedGroup = 0;

    string type;
    int health;
    bool hasShield;

    XDocument xDoc;
    List<Wave> waves = new List<Wave>();

    void Export()
    {
        xDoc = new XDocument();
        xDoc.Add(new XElement(fileName));
        XElement elements = xDoc.Element(fileName);

        //HIERZO
        //ik moet elk wave-object hebben dat aangemaak wordt
        //var waves = GameObject.FindGameObjectsWithTag("Wave");

        //foreach(var wave in waves)
        //{
        //    XElement xElemWave = new XElement("Wave");
        //    //zelfde shit. Elk group object van een wave...
        //    //var groups = GameObject.FindGameObjectsWithTag("Group");
        //    
        //    foreach (var group in groups)
        //    {
        //        XElement xElemGroup = new XElement("Group");
        //        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //    
        //        foreach (var enemy in enemies)
        //        {
        //            XElement xElemEnemy = new XElement("Enemy");
        //           //XAttribute attrType = new Attribute("Type", enemy.type);
        //           //XAttribute attrHealth = new Attribute("Health", enemy.health);
        //           //XAttribute attrShield = new Attribute("Shield", enemy.hasShield);
        //           // xElemEnemy.Add(attrType, attrHealth, attrShield);
        //    
        //            xElemGroup.Add(xElemEnemy);
        //        }
        //        xElemWave.Add(xElemGroup);
        //    }
        //    xElemWaves.Add(xElemWave);
        //}

        if (EditorUtility.DisplayDialog("Save confirmation", "Are you sure you want to save the level " + fileName + "?", "OK", "Cancel"))
        {
            xDoc.Save("Assets/Levels/" + fileName);
            EditorUtility.DisplayDialog("Saved", fileName + " saved!", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("NOT Saved", fileName + "not saved!", "YOU DID NOT SAVE");
        }
    }

    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUILayout.LabelField("Total waves created:" + waves.Count);
        
        for (int waveIndex = 0; waveIndex < waves.Count; ++waveIndex)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Wave " + (waveIndex + 1));
            if (GUILayout.Button("Delete wave" + (waveIndex + 1)))
            {
                waves.RemoveAt(waveIndex);
            }
            EditorGUILayout.EndHorizontal();

            for (int groupIndex = 0; groupIndex < waves[waveIndex].groups.Count; ++groupIndex)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("\t Group " + (groupIndex + 1));              
                if (GUILayout.Button("Delete group" + (groupIndex + 1)))
                {
                    waves[waveIndex].groups.RemoveAt(groupIndex);
                }
                EditorGUILayout.EndHorizontal();

                for (int enemyIndex = 0; enemyIndex < waves[waveIndex].groups[groupIndex].enemies.Count; ++enemyIndex)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("\t \t Enemy " + (enemyIndex + 1));

                    if (GUILayout.Button("Delete enemy" + (enemyIndex + 1)))
                    {
                        waves[waveIndex].groups[groupIndex].enemies.RemoveAt(enemyIndex);
                    }
                    EditorGUILayout.EndHorizontal();
                }

            }
        }
        EditorGUILayout.EndScrollView();


        if (waves.Count >= 1)
        {
            if (selectedWave != 0)
            {
                if (waves[selectedWave - 1].groups.Count >= 1)
                {
                    EditorGUILayout.LabelField("Add a new enemy", EditorStyles.boldLabel);
                    selectedGroup = EditorGUILayout.IntSlider("Selected Group", selectedGroup, 1, waves[selectedWave - 1].groups.Count);
                    if (GUILayout.Button("Add a new Enemy"))
                    {
                        waves[selectedWave - 1].groups[selectedGroup - 1].enemies.Add(new Enemy());
                    }
                }
            }

            EditorGUILayout.LabelField("Add a new group", EditorStyles.boldLabel);
            selectedWave = EditorGUILayout.IntSlider("Selected Wave", selectedWave, 1, waves.Count);
            if (GUILayout.Button("Add new Group"))
            {
                waves[selectedWave - 1].groups.Add(new Group());
            }     
        }

        EditorGUILayout.LabelField("Add a new wave", EditorStyles.boldLabel);
        if (GUILayout.Button("Add new Wave"))
        {
            waves.Add(new Wave());
        }

        EditorGUILayout.LabelField("Export Level", EditorStyles.boldLabel);
        fileName = EditorGUILayout.TextField("Filename:", fileName);
        if (GUILayout.Button("Export"))
        {
            Export();
        }
    }
}
