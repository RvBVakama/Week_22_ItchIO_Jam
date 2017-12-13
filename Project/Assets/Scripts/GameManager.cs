using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //A list of all the white platforms in the current level
    public List<GameObject> WhitePlatforms;

    //A list of all the black platforms in the current level
    public List<GameObject> BlackPlatforms;

    // A list of all the fragments in the current level
    public List<GameObject> RemFragments;

    //A list of all enemys in the current level
    Enemy[] enemy;

    //Is the scene dark?
    public bool isDark = false;

    public bool LevelSpawn = false;

    // OSTs
    public AudioClip MusicNightmare;
    public AudioClip MusicSweetDreams;
    private AudioSource audioSource;

    // Number of fragments that the player has
    [HideInInspector]
    public int FragmentCount = 0;

    // REM Fragment count
    public Text FragmentsText = null;

    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        FindEverything();
    }

    public void FindEverything()
    {
        WhitePlatforms.Clear();
        BlackPlatforms.Clear();
        RemFragments.Clear();
        //Get all enemys in the level
        enemy = GameObject.FindObjectsOfType<Enemy>();
        //Get all white platforms in the level
        GameObject[] wP = GameObject.FindGameObjectsWithTag("PlatformWhite");
        //Get all black platforms in the level
        GameObject[] bP = GameObject.FindGameObjectsWithTag("PlatformBlack");
        //Get all RemFragments in the level
        GameObject[] Fragment = GameObject.FindGameObjectsWithTag("Fragment");

        //Add the white platforms to our list
        foreach (GameObject w in wP)
        {
            WhitePlatforms.Add(w);
        }
        //Add the black platforms to our list
        foreach (GameObject b in bP)
        {
            BlackPlatforms.Add(b);
        }
        //Add the Fragments to our list
        foreach (GameObject f in Fragment)
        {
            RemFragments.Add(f);
        }

        LevelSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Updating how many fragments the player has (visually)
        if (SceneManager.GetActiveScene().name != "MapEditor")
            FragmentsText.text = "Fragments " + FragmentCount;

        //Toggle dark mode
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isDark = !isDark;
        }
        if (WhitePlatforms == null || BlackPlatforms == null || enemy == null || RemFragments == null)
            return;
        if (isDark)
        {
            if (!audioSource.isPlaying)
            {
                // Play happy funtime music
                audioSource.clip = MusicSweetDreams;
                audioSource.Play();
            }
            if (audioSource.isPlaying && audioSource.clip == MusicNightmare)
            {
                // Play unhappy non-funtime music
                audioSource.Stop();
                audioSource.clip = MusicSweetDreams;
                audioSource.Play();
            }
            //Set the camera background to black if isDark = true
            Camera.main.backgroundColor = Color.black;
            //Set all enemys to false
            foreach (Enemy e in enemy)
            {
                e.gameObject.SetActive(false);
            }
            //Enable all black platforms  
            foreach (GameObject b in BlackPlatforms)
            {
                if (b == null)
                {
                    BlackPlatforms.Clear();
                    FindEverything();
                    return;
                }
                else
                {
                    b.GetComponent<Collider2D>().enabled = true;
                }
            }
            //Disable all white platforms
            foreach (GameObject w in WhitePlatforms)
            {
                if (w == null)
                {
                    WhitePlatforms.Clear();
                    FindEverything();
                    return;
                }
                else
                {
                    w.GetComponent<Collider2D>().enabled = false;
                }
            }
        }
        if (!isDark)
        {
            if (audioSource.isPlaying && audioSource.clip == MusicSweetDreams)
            {
                // Play unhappy non-funtime music
                audioSource.Stop();
                audioSource.clip = MusicNightmare;
                audioSource.Play();
            }

            //If is not dark then make the camera background white
            Camera.main.backgroundColor = Color.white;
            //Set the enemy to true
            foreach (Enemy e in enemy)
            {
                if (e == null)
                {
                    FindEverything();
                    return;
                }
                else
                {
                    e.gameObject.SetActive(true);
                }
            }
            //Disable all black platforms
            foreach (GameObject b in BlackPlatforms)
            {
                if (b == null)
                {
                    BlackPlatforms.Clear();
                    FindEverything();
                    return;
                }
                else
                {
                    b.GetComponent<Collider2D>().enabled = false;
                }
            }
            //Enable all white platforms
            foreach (GameObject w in WhitePlatforms)
            {
                if (w == null)
                {
                    WhitePlatforms.Clear();
                    FindEverything();
                    return;
                }
                else
                {
                    w.GetComponent<Collider2D>().enabled = true;
                }
            }
        }
    }
}
