using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    public float restartDelay = 5f;


    public bool isInTransition;

    public GameObject MenuPanel;

    public GameObject playButton;

    public GameObject quitButton;

    public GameObject replayButton;

    public GameObject resumeButton;



    public GameObject closingCircle;

    public int Level;
    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("bgmusic");

        FindObjectOfType<AudioManager>().Play("wind");

        FindObjectOfType<PlayerMovement>().gameObject.layer = 22;

        ShowStartMenu();

        Level = 1;
    }

    public bool escAvailable;
    private void Update()
    {
        if (escAvailable)
        {//outside the pause thing
            if (Input.GetButtonDown("Cancel"))//where you press esc, where esc becomes unavailable
            {
                ShowResumeMenu();

                FindObjectOfType<PlayerMovement>().playerControl = false;

                FindObjectOfType<PlayerMovement>().gameObject.layer = 22;
            }
        }
        else
        {
            //inside of  the pause
            if (Input.GetButtonDown("Cancel"))
            {
                ResumeButton();
            }

        }
    }

    //call at starting scene
    public void PlayButton()
    {
        //player able to move
        FindObjectOfType<AudioManager>().Play("button");

        FindObjectOfType<PlayerMovement>().playerControl = true;

        FindObjectOfType<PlayerMovement>().gameObject.layer = 12;

        //turn menu off
        MenuPanel.SetActive(false);

        escAvailable = true;
        
    }

    public void RePlayButton()
    {
        FindObjectOfType<AudioManager>().Play("button");
        //RELOAD
        Restart();
    }

    public void ResumeButton()
    {
        PlayButton();

        
    }


    public void QuitButton()
    {
        FindObjectOfType<AudioManager>().Play("button");
        QuitGame();
    }


    //call when player hit deadzone or got eaten
    public void EndGame()
    {
        if (!gameHasEnded)
        {
            gameHasEnded = true;

            closingCircle.SetActive(true);

            //change closing circle here? before update function
            closingCircle.GetComponent<ClosingCircle>().ChangeScale();

            closingCircle.GetComponent<ClosingCircle>().IsClosing = true;

            FindObjectOfType<AudioManager>().Play("death");

            Invoke("Restart", restartDelay);

        }
    }

 

    public void ShowStartMenu()
    {
        MenuPanel.SetActive(true);

        quitButton.SetActive(true);

        playButton.SetActive(true);

        replayButton.SetActive(false);

        resumeButton.SetActive(false);

        escAvailable = false;
    }

    public void ShowRestartMenu()
    {
        MenuPanel.SetActive(true);

        quitButton.SetActive(true);

        playButton.SetActive(false); 

        replayButton.SetActive(true);

        resumeButton.SetActive(false);

        escAvailable = false;

        
    }

    public void ShowResumeMenu()
    {
        MenuPanel.SetActive(true);

        quitButton.SetActive(true);

        playButton.SetActive(false);

        replayButton.SetActive(false);

        resumeButton.SetActive(true);

        escAvailable = false;


    }

    //call at player die
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void PauseGame()
    {

    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
