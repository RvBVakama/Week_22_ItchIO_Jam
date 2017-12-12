﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MapLoader : MonoBehaviour {

    public MapObject[] mapObjects;
    public string[] mapDirs;
    public int curDir;
    public void Start()
    {
        mapDirs = Directory.GetFiles("Levels/");
    }

    public void LoadMap(string mapDir)
    {
        foreach (MapObject g in GameObject.FindObjectsOfType<MapObject>())
        {
            Destroy(g.gameObject);
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
    }
}
