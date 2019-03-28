//* Class that will check all toggles *//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleChecker : MonoBehaviour
{
    //Toggles for the Login screen
    public Toggle participantToggle;
    public Toggle proctorToggle;

    //Toggles for the proctor screen
    public Toggle playerOneAiToggle;
    public Toggle playerOneHumanToggle;
    public Toggle playerTwoAiToggle;
    public Toggle playerTwoHumanToggle;
    public Toggle puzzleToggle;
    public Toggle leverToggle;

    //Function to check whos playing. Returns 0 for participant, 1 for proctor.
    public bool checkWhosPlaying()
    {
        bool whosPlaying = false;

        //If they are a participant then show the game code section
        if (participantToggle.isOn)
        {
            whosPlaying = true;
        }
        //If they are a proctor then dont show the game code, because they will be creating it
        if (proctorToggle.isOn)
        {
            whosPlaying = false;
        }
 
        return whosPlaying;
    }

    //Function to check which toggles are enabled on the proctor screen
    public List<string> checkSetupToggles()
    {
        //If the playerOneHumanToggle is on then they are human
        string playerOneType = playerOneHumanToggle.isOn ? "Human": "AI";
        //If the playerTwoHumanToggle is on then they are human
        string playerTwoType = playerTwoHumanToggle.isOn ? "Human" : "AI";
        //If the puzzleToggle is on then it should be a puzzle game
        string gameType = puzzleToggle.isOn ? "Puzzle" : "Lever";

        //return each type in a list
        return new List<string> {playerOneType, playerTwoType, gameType};
    }

}
