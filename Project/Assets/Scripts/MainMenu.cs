using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MapLoader");
    }

    public void PlayMapEditor()
    {
        SceneManager.LoadScene("MapEditor");
    }

    public void PlayCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void PlayQuit()
    {
        Application.Quit();
    }

}
