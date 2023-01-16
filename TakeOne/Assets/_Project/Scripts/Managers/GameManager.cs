using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private bool isPaused;
        [SerializeField] private GameObject pausePanel;

        // Start is called before the first frame update
        void Start()
        {
            //Set the game to be unpaused on startup
            isPaused = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape) && isPaused)
            {
                //Unpause the game if we are paused (deactivate the pausePanel and set the timescale to 1)
                SetPauseState(1);
                pausePanel.SetActive(false);
                isPaused = false;
            }
            else if (Input.GetKeyUp(KeyCode.Escape) && !isPaused)
            {
                //Pause the game since we are still playing (activate the pausePanel and set the timeScale to 0)
                SetPauseState(0);
                pausePanel.SetActive(true);
                isPaused = true;
            }
        }

        //Sets the timeScale based on a gameSpd input (we can slow the game if we want)
        public void SetPauseState(int gameSpd)
        {
            Time.timeScale = gameSpd;
        }

    }
}
