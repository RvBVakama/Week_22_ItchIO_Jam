using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
public class MapLoader : MonoBehaviour
{
    public MapObject[] mapObjects;
    public string[] mapDirs;
    public int curDir = 0;
    string curMapString;
    GameManager gm;
    public Text mapName;
    public void Start()
    {
        mapDirs = Directory.GetFiles("Levels/");
        NextMap();
    }

    public void RestartMap()
    {
        LoadMap(curMapString);
    }


    public void NextMap()
    {
        if (curDir > mapDirs.Length - 1)
        {
            Application.Quit();
        }
        else
        {
            gm = GameObject.FindObjectOfType<GameManager>();
            curMapString = mapDirs[curDir];
            LoadMap(mapDirs[curDir]);
            curDir++;
        }
    }

    public void LoadMap(string mapDir)
    {
        gm.LevelSpawn = false;

        string name = mapDir.Split('/')[1].Split('.')[0];
        mapName.text = name;

        try
        {
            foreach (MapObject g in GameObject.FindObjectsOfType<MapObject>())
            {
                Destroy(g.gameObject);
            }
        }
        catch
        {

        }
        foreach(Enemy e in gm.enemy)
        {
            if (e == null)
            {
                //RestartMap();
                continue;
            }
            else
            {
                Destroy(e.gameObject);
            }
        }
        gm.enemy.Clear();
        List<byte> b = new List<byte>();
        using (StreamReader sr = new StreamReader(mapDir))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                b.Add(byte.Parse(line));
            }
            sr.Close();
        }

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(b.ToArray());
        MapData MD = (MapData)bf.Deserialize(ms);

        for (int i = 0; i < MD.objectData.Count; i++)
        {
            GameObject g = (GameObject)Instantiate(mapObjects[MD.ID[i]].gameObject, new Vector2(MD.objectData[i].x, MD.objectData[i].y), Quaternion.identity);
        }
        gm.LevelSpawn = true;
        gm.FindEverything();
        Time.timeScale = 1;
       
    }
}
