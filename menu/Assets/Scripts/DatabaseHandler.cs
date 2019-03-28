using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DatabaseHandler : MonoBehaviour
{
    //Keep count of each game move
    //private int count = 0;

    public GamePlay gamePlay;
    public string gameCode;
    public Puzzle puzzle;
    public int number;

    //Text views
    public Text playerNumberText;
    public Text whosTurnText;
    public Text suggestedMoveText;
    public Text ratingText;


    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ittseniordesign.firebaseio.com/");
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

   
    //Creates a user in the game corresponding to the game code
    public void writeNewUser(string email, string type, string gameCode, string playerNumber)
    {
        this.gameCode = gameCode;

        if (playerNumber != "full")
        {
            //Get the root reference location of the database
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            //Go to Games section and update the player email
            reference.Child("Games").Child(gameCode).Child("player" + playerNumber + "Email").SetValueAsync(email);
            //Go ti the Games section and update the player type
            reference.Child("Games").Child(gameCode).Child("player" + playerNumber + "Type").SetValueAsync(type);
            //Start listening for changes to the database       
        }

        if (playerNumber == "One")
        {
            playerNumberText.text = "You are player one";
            whosTurnText.text = "Your turn";
        }
        else
        if (playerNumber == "Two")
        {
            playerNumberText.text = "Your are player two";
            whosTurnText.text = "Player one's turn";
        }

        StartListener(gameCode);
    }

    //Sends the move to the database
    public void logMove(GameMoveJSON gameMove, string gameCode, int count)
    {
        //Get the root reference location of the database
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        //Turn the incoming GameMoveJSON into a JSON file
        string json = JsonUtility.ToJson(gameMove);
        //Add the move to the proper move location in database
        reference.Child("Games").Child(gameCode).Child("Moves").Child("move" + count.ToString()).SetRawJsonValueAsync(json);
        //Increment the count
        //count++;
        //Log the count
        //reference.Child("Games").Child(gameCode).Child("Moves").Child("count").SetValueAsync(count);
    }

    //Listens to data that has changed in the Games -> Moves section of the database
    public void StartListener(string gameCode)
    {
        //Get the location of Games -> Moves (because this is where we want to see changes)
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Games").Child(gameCode).Child("Moves"); 
        //If something changes here then run the HandleChildChanged method
        reference.ChildAdded += HandleChildChanged;
    }


    //Handles the result if something changed in the Games -> GameCode -> Moves section of the database
    void HandleChildChanged(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        Debug.Log("A value has changed");
        Debug.Log("Player number: " + number);

        //Data comes in as a key, value pair <string, object>
        Dictionary<string, object> update = (Dictionary<string, object>)args.Snapshot.Value;
        //Get the value of the object with the key of "satisfaction"
        object satisfaction = (object) update["satisfaction"];
        //Get the value of the object with the key of "suggestedMove"
        object suggestedMove = (object)update["suggestedMove"];
        //Update the proper count
        object counts = (object) update["count"];
        //Set the new count
        gamePlay.setCount(int.Parse(counts.ToString()));
        //Get the block to move
        object block = (object)update["blockToMove"];



        updateText(int.Parse(counts.ToString()));

        if (number == 1 && int.Parse(counts.ToString()) % 2 != 0)
        {
            //TODO go to next turn
        }
        else
      
        if (number == 1 && int.Parse(counts.ToString()) % 2 == 0)
        {

            //Set the block to move
            gamePlay.setBlockToMove(int.Parse(block.ToString()));
            //Move the block
            puzzle.makeOtherPlayersMove(int.Parse(block.ToString()));
            //Change the colors of the buttons
            gamePlay.changeButton(suggestedMove.ToString(), satisfaction.ToString());
        }
        
        if (number == 2 && int.Parse(counts.ToString()) % 2 == 0)
        {
            //TODO go to next turn
        }
        else
        
        if (number == 2 && int.Parse(counts.ToString()) % 2 != 0)
        {
            //Set the block to move
            gamePlay.setBlockToMove(int.Parse(block.ToString()));
            //Move the block
            puzzle.makeOtherPlayersMove(int.Parse(block.ToString()));
            //Change the colors of the buttons
            gamePlay.changeButton(suggestedMove.ToString(), satisfaction.ToString());
        }

    }

    //Checks to see if the game is open
    public void getPlayerNumber(string email, string type, string gameCode)
    {

        FirebaseDatabase.DefaultInstance
        .GetReference("Games").Child(gameCode)
        .GetValueAsync().ContinueWith(task => {
          if (task.IsFaulted)
          {
              // Handle the error...
          }
          else if (task.IsCompleted)
          {
             
                DataSnapshot snapshot = task.Result;
                Dictionary<string, object> update = (Dictionary<string, object>)snapshot.Value;
                object emailOne = (object)update["playerOneEmail"];
                object emailTwo = (object)update["playerTwoEmail"];
               
                Debug.Log("Email One: " + emailOne.ToString() + " Email Two: " + emailTwo.ToString());

                if (emailOne.ToString() == "email")
                {
                    Debug.Log("Email one empty");
                    writeNewUser(email, type, gameCode, "One");
                    number = 1;
                   
                }
                else

                if (emailTwo.ToString() == "email")
                {
                    Debug.Log("Email two empty");
                    writeNewUser(email, type, gameCode, "Two");
                    number = 2;
                    
                }
                else
                {
                    writeNewUser(email, type, gameCode, "full");
                }
          }
          
        });

    }

    public void updateText(int counts)
    {
        //other player
        if (number == 1 && counts % 2 != 0)
        {
            whosTurnText.text = "Player two's turn";           
        }
        else

        if (number == 1 && counts % 2 == 0)
        {
            whosTurnText.text = "Your turn";
            suggestedMoveText.text = "Player two's suggested move:";
            ratingText.text = "Player two's rating:";
        }

        //other player
        if (number == 2 && counts % 2 == 0)
        {
            whosTurnText.text = "Player one's turn";
        }
        else

        if (number == 2 && counts % 2 != 0)
        {
            whosTurnText.text = "Your turn";
            suggestedMoveText.text = "Player ones's suggested move:";
            ratingText.text = "Player ones's rating:";
        }

    }

    // Exit command
    void Update()
        {       
        //If the escape key is pressed the application will terminate
        if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
}
