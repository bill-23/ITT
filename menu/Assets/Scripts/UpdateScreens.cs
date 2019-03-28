using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateScreens : MonoBehaviour
{
    //Login and game canvas GameObjects
    public GameObject loginCanvas;
    public GameObject gameCanvas;

    //Game code text box and prompt text GameObjects
    public GameObject gameCodeInputField;
    public GameObject gameCodeText;

    //Cameras fo rthe login screen and puzzle game
    public Camera loginCamera;
    public Camera gameplayCamera;

    //Function to switch between game cameras
    public void switchCameras(bool whichCamera)
    {
        //If true
        if (whichCamera)
        {
            //Set the login camera to active, the puzzle game to inactive
            loginCamera.enabled = true;
            gameplayCamera.enabled = false;
        }
        //Otherwise
        else
        {
            //Set the puzzle game camera to active, the login camera to inactive
            loginCamera.enabled = false;
            gameplayCamera.enabled = true;
        }

    }

    //Function to switch between the login screen and the puzzle game
    public void switchCanvas(bool whichScreen)
    {
        //If true 
        if (whichScreen)
        {
            //Show the login screen and hide the puzzle game
            loginCanvas.SetActive(true);
            gameCanvas.SetActive(false);
        }
        //Otherwise
        else
        {
            //Show the puzzle game and hide the login screen
            loginCanvas.SetActive(false);
            gameCanvas.SetActive(true);
        }

    }

    //Function to show or hide the game code textbox on the login screen
    public void switchLoginType(bool whatType)
    {
        //If true then its a participant, show the game code entry box
        if (whatType)
        {
            //Show the input field
            gameCodeInputField.SetActive(true);
            gameCodeText.SetActive(true);
        }
        //Otherwise its a proctor, don't show the entry box
        else
        {
            //hide the input field
            gameCodeInputField.SetActive(false);
            gameCodeText.SetActive(false);
        }

    }

}
