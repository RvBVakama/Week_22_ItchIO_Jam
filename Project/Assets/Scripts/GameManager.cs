using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<GameObject> WhitePlatforms;
    public List<GameObject> BlackPlatforms;
    Enemy enemy;
    public bool isDark = false;

	// Use this for initialization
	void Start () {
        enemy = GameObject.FindObjectOfType<Enemy>();

        GameObject[] wP = GameObject.FindGameObjectsWithTag("PlatformWhite");
        GameObject[] bP = GameObject.FindGameObjectsWithTag("PlatformBlack");

        foreach(GameObject w in wP)
        {
            WhitePlatforms.Add(w);
        }
        foreach (GameObject b in bP)
        {
            BlackPlatforms.Add(b);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            isDark = !isDark;
        }
        if(isDark)
        {
            Camera.main.backgroundColor = Color.black;
            enemy.gameObject.SetActive(false);
            foreach(GameObject b in BlackPlatforms)
            {
                b.GetComponent<Collider2D>().enabled = true;
            }
            foreach (GameObject w in WhitePlatforms)
            {
                w.GetComponent<Collider2D>().enabled = false;
            }
        }
        if(!isDark)
        {
            Camera.main.backgroundColor = Color.white;
            enemy.gameObject.SetActive(true);
            foreach (GameObject b in BlackPlatforms)
            {
                b.GetComponent<Collider2D>().enabled = false;
            }
            foreach (GameObject w in WhitePlatforms)
            {
                w.GetComponent<Collider2D>().enabled = true;
            }
        }
	}
}
