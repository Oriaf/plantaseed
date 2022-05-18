using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; 

    public GameObject pauseMenuUI;

    private GameObject[] joysticks;

    void Awake(){
        joysticks = GameObject.FindGameObjectsWithTag("JoyStick");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume(); //if paused then resume
            } else
            {
                Pause(); //of not paused then pause 
            }
        }
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        foreach (GameObject j in joysticks){
            j.SetActive(true);
        }
    }
    public void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // no time is passing in the game
        GameIsPaused = true;
        foreach (GameObject j in joysticks){
            j.SetActive(false);
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }

}