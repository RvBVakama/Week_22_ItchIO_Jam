using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public MapObject[] mapObjects;
    public string[] mapDirs;
    public int curDir = 0;
    string curMapString;
    GameManager gm;
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

        foreach (MapObject g in GameObject.FindObjectsOfType<MapObject>())
        {
            Destroy(g.gameObject);
        }
        foreach(Enemy e in gm.enemy)
        {
            Destroy(e.gameObject);
        }

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
