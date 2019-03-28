using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;

public class CreateGame : MonoBehaviour
{

    public Toggle p1AiToggle;
    public Toggle p1HumanToggle;
    public Toggle p2AiToggle;
    public Toggle p2HumanToggle;
    public Toggle puzzleToggle;
    public Toggle leverToggle;
    public Text proctorGameCodeText;

    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ittseniordesign.firebaseio.com/");
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    //Create the game in the database
    public void setupGame()
    {
        //Create a JSON holding all game data 
        GameData gameData = new GameData(p1AiToggle.isOn ? "AI" : "Human",
            p2AiToggle.isOn ? "AI" : "Human",
            "email", "email",
            puzzleToggle.isOn ? "Puzzle" : "Lever");

        string gameCodeText = makeGameCode().ToString();

        //Get the root reference location of the database
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        //Turn the GameDataJSON class variables into a json file
        string json = JsonUtility.ToJson(gameData);
        //Create a new game under Games -> gameCode -> JSON that was just made
        reference.Child("Games").Child(gameCodeText).SetRawJsonValueAsync(json);

        proctorGameCodeText.text = "New game code: " + gameCodeText;
    }

    //Creates the random game code to be used
    public int makeGameCode()
    {
        //Make new random
        System.Random randomNumber = new System.Random();
        //Make a random game code with number from 1 -> 5000
        int newGameCode = randomNumber.Next(1, 5000);
        //TODO Check in database to make sure code isnt used yet
        return newGameCode;
    }

}
