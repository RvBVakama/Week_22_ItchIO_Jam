﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //A list of all the white platforms in the current level
    public List<GameObject> WhitePlatforms;
    //A list of all the black platforms in the current level
    public List<GameObject> BlackPlatforms;
    //A list of all enemys in the current level
    Enemy[] enemy;
    //Is the scene dark?
    public bool isDark = false;

	// Use this for initialization
	void Start () {
        //Get all enemys in the level
        enemy = GameObject.FindObjectsOfType<Enemy>();
        //Get all white platforms in the level
        GameObject[] wP = GameObject.FindGameObjectsWithTag("PlatformWhite");
        //Get all black platforms in the level
        GameObject[] bP = GameObject.FindGameObjectsWithTag("PlatformBlack");

        //Add the white platforms to our list
        foreach(GameObject w in wP)
        {
            WhitePlatforms.Add(w);
        }
        //Add the black platforms to our list
        foreach (GameObject b in bP)
        {
            BlackPlatforms.Add(b);
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Toggle dark mode
		if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            isDark = !isDark;
        }
        if(isDark)
        {
            //Set the camera background to black if isDark = true
            Camera.main.backgroundColor = Color.black;
            //Set all enemys to false
            foreach (Enemy e in enemy)
            {
                e.gameObject.SetActive(false);
            }
            //Enable all black platforms    
            foreach(GameObject b in BlackPlatforms)
            {
                b.GetComponent<Collider2D>().enabled = true;
            }
            //Disable all white platforms
            foreach (GameObject w in WhitePlatforms)
            {
                w.GetComponent<Collider2D>().enabled = false;
            }
        }
        if(!isDark)
        {
            //If is not dark then make the camera background white
            Camera.main.backgroundColor = Color.white;
            //Set the enemy to true
            foreach (Enemy e in enemy)
            {
                e.gameObject.SetActive(true);
            }
            //Disable all black platforms
            foreach (GameObject b in BlackPlatforms)
            {
                b.GetComponent<Collider2D>().enabled = false;
            }
            //Enable all white platforms
            foreach (GameObject w in WhitePlatforms)
            {
                w.GetComponent<Collider2D>().enabled = true;
            }
        }
	}
}
