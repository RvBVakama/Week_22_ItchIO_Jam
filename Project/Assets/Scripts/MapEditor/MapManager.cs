using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour {

    public GameManager gm;
    public MapObject[] mapObjects;
    public string[] mapDirs;
    public InputField saveText;
    public Dropdown mapList;
    public Dropdown mapObjectsList;
    public bool togglePlay = false;
    EventSystem es;
    private void Start()
    {
        es = GameObject.FindObjectOfType<EventSystem>();
        gm = GameObject.FindObjectOfType<GameManager>();
        mapDirs = Directory.GetFiles("Levels/");
        List<Dropdown.OptionData> oData = new List<Dropdown.OptionData>();
        List<Dropdown.OptionData> mObjects = new List<Dropdown.OptionData>();

        foreach (string d in mapDirs)
        {
            oData.Add(new Dropdown.OptionData(d));
        }
        for(int i = 0; i < mapObjects.Length; i++)
        {
            mObjects.Add(new Dropdown.OptionData(mapObjects[i].name));
        }
        mapObjectsList.AddOptions(mObjects);
        mapList.AddOptions(oData);
    }

    private void Update()
    {
        if (es.IsPointerOverGameObject())
            return;
        if (Input.GetMouseButtonDown(0))
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            GameObject obj = (GameObject)Instantiate(mapObjects[mapObjectsList.value].gameObject, pos, Quaternion.identity);
            obj.GetComponent<MapObject>().ElementID = mapObjectsList.value;
            
        }
        if(togglePlay)
        {
            MapObject[] all = GameObject.FindObjectsOfType<MapObject>();
            foreach (MapObject g in all)
            {
                if (g.GetComponent<Rigidbody2D>() == true)
                {
                    g.GetComponent<Rigidbody2D>().simulated = true;
                }
            }
        }
        else
        {
            MapObject[] all = GameObject.FindObjectsOfType<MapObject>();
            foreach(MapObject g in all)
            {
                if (g.GetComponent<Rigidbody2D>() == true)
                {
                    g.GetComponent<Rigidbody2D>().simulated = false;
                }
            }
        }
    }

    public void TogglePlay()
    {
        gm.FindEverything();
        togglePlay = !togglePlay;
    }

    public void SaveAll()
    {
        MapData MD = new MapData();

        MapObject[] mapRaw = GameObject.FindObjectsOfType<MapObject>();
        foreach(MapObject mr in mapRaw)
        {
            MD.ID.Add(mr.ElementID);
            //MD.X.Add(mr.gameObject.transform.position.x);
            //MD.Y.Add(mr.gameObject.transform.position.y);
            ObjectData OD = new ObjectData();
            OD.x = mr.gameObject.transform.position.x;
            OD.y = mr.gameObject.transform.position.y;
            MD.objectData.Add(OD);
            //MD.rot.Add(mr.gameObject.transform.rotation);
        }

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();

        bf.Serialize(ms, MD);

        using (StreamWriter sw = new StreamWriter("Levels/"+saveText.text+".map",true))
        {
            foreach(byte b in ms.ToArray())
            {
                sw.WriteLine(b);
            }
            sw.Close();
        }
    }

    public void LoadMap()
    {
        foreach(MapObject g in GameObject.FindObjectsOfType<MapObject>())
        {
            Destroy(g.gameObject);
        }
        string mapDir = mapList.captionText.text;

        List<byte> b = new List<byte>();
        using (StreamReader sr = new StreamReader(mapDir))
        {
            string line;
            while((line = sr.ReadLine()) != null)
            {
                b.Add(byte.Parse(line));
            }
            sr.Close();
        }

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(b.ToArray());
        MapData MD = (MapData)bf.Deserialize(ms);
        //SEMI WORKING
        /*int i = 0;
        foreach(ObjectData o in MD.objectData)
        {
            GameObject g = (GameObject)Instantiate(mapObjects[MD.ID[i]].gameObject, new Vector2(o.x, o.y), Quaternion.identity);
            i++;
        }*/
        for(int i = 0; i < MD.objectData.Count; i++)
        {
            GameObject g = (GameObject)Instantiate(mapObjects[MD.ID[i]].gameObject, new Vector2(MD.objectData[i].x, MD.objectData[i].y), Quaternion.identity);
        }
    }
}
