using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public GameManager gm;
    public MapObject[] mapObjects;
    public string[] mapDirs;
    public InputField saveText;
    public Dropdown mapList;
    public Dropdown mapObjectsList;
    public bool togglePlay = false;
    public float CameraMoveSpeed;
    EventSystem es;
    public Canvas canvas;
    public List<MapObject> forSaveObjects = new List<MapObject>();
    private void Start()
    {
        Time.timeScale = 0;

        es = GameObject.FindObjectOfType<EventSystem>();
        gm = GameObject.FindObjectOfType<GameManager>();
        mapDirs = Directory.GetFiles("Levels/");
        List<Dropdown.OptionData> oData = new List<Dropdown.OptionData>();
        List<Dropdown.OptionData> mObjects = new List<Dropdown.OptionData>();

        foreach (string d in mapDirs)
        {
            oData.Add(new Dropdown.OptionData(d));
        }
        for (int i = 0; i < mapObjects.Length; i++)
        {
            mObjects.Add(new Dropdown.OptionData(mapObjects[i].name));
        }
        mapObjectsList.AddOptions(mObjects);
        mapList.AddOptions(oData);
    }

    private void FixedUpdate()
    {
        /*if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.gameObject.transform.Translate(Vector2.up * CameraMoveSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.gameObject.transform.Translate(Vector2.down * CameraMoveSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.gameObject.transform.Translate(Vector2.left * CameraMoveSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.gameObject.transform.Translate(Vector2.right * CameraMoveSpeed);
        }*/
    }

    private void Update()
    {
        if (es.IsPointerOverGameObject())
            return;
        if (togglePlay)
        {
            MapObject[] all = GameObject.FindObjectsOfType<MapObject>();
            foreach (MapObject g in all)
            {
                if (g.GetComponent<Rigidbody2D>() == true)
                {
                    //g.GetComponent<Rigidbody2D>().simulated = true;

                }
            }
            Time.timeScale = 1;
        }
        else
        {
            MapObject[] all = GameObject.FindObjectsOfType<MapObject>();
            foreach (MapObject g in all)
            {
                if (g.GetComponent<Rigidbody2D>() == true)
                {
                    g.gameObject.transform.position = g.StartTransform.position;
                    //g.GetComponent<Rigidbody2D>().simulated = false;

                }

            }
            Time.timeScale = 0;
        }

        if (togglePlay)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject obj = (GameObject)Instantiate(mapObjects[mapObjectsList.value].gameObject, pos, Quaternion.identity);
            obj.GetComponent<MapObject>().ElementID = mapObjectsList.value;
        }
    }

    public void BackToMainMenu(){SceneManager.LoadScene("MainMenu");}

    public void TogglePlay()
    {

        gm.FindEverything();
        togglePlay = !togglePlay;
    }

    public void SaveAll()
    {
        MapData MD = new MapData();
        foreach(MapObject mo in GameObject.FindObjectsOfType<MapObject>())
        {
            forSaveObjects.Add(mo);
        }
        
        //MapObject[] mapRaw = GameObject.FindObjectsOfType<MapObject>();
        foreach (MapObject mr in forSaveObjects)
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

        using (StreamWriter sw = new StreamWriter("Levels/" + saveText.text + ".map", false))
        {
            foreach (byte b in ms.ToArray())
            {
                sw.WriteLine(b);
            }
            sw.Close();
        }
        ms.Close();
        forSaveObjects.Clear();
    }

    public void LoadMap()
    {
        forSaveObjects.Clear();
        foreach (MapObject g in GameObject.FindObjectsOfType<MapObject>())
        {
            Destroy(g.gameObject);
        }
        string mapDir = mapList.captionText.text;

        string name = mapDir.Split('/')[1].Split('.')[0];
        //string[] name2 = name[1].Split('.');
        string normName = name;
        saveText.text = normName;

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
        //SEMI WORKING
        /*int i = 0;
        foreach(ObjectData o in MD.objectData)
        {
            GameObject g = (GameObject)Instantiate(mapObjects[MD.ID[i]].gameObject, new Vector2(o.x, o.y), Quaternion.identity);
            i++;
        }*/
        for (int i = 0; i < MD.objectData.Count; i++)
        {
            GameObject g = (GameObject)Instantiate(mapObjects[MD.ID[i]].gameObject, new Vector2(MD.objectData[i].x, MD.objectData[i].y), Quaternion.identity);
            g.GetComponent<MapObject>().ElementID = MD.ID[i];
        }
        ms.Close();
    }
}
