using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObj;
    public static bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        if (pauseMenuObj == false)
        {
            Time.timeScale = 1.0f;
            isPaused = false;
        }
        //will allow the game to restart(from the menu) and to continue like normal
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuObj.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuObj.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }


}

