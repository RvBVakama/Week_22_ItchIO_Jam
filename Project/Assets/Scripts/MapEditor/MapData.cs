using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData {

    public string mapName = "Test";

    public List<int> ID = new List<int>();

    public List<ObjectData> objectData =  new List<ObjectData>();
    //public List<Vector2> pos =  new List<Vector2>();

    //public List<Quaternion> rot =  new List<Quaternion>();
}

[System.Serializable]
public class ObjectData
{
    public float x;
    public float y;
}
