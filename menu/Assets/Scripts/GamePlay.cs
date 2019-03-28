using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    //Can't make "new" instance of MonoBehaviour class so we attach in the Unity editor
    public DatabaseHandler database;

    //The satisfaction buttons
    public Button goodButton;
    public Button poorButton;

    //Will be used to trigger new game move
    public Button doneButton;

    //The suggested move buttons
    public Button oneButton;
    public Button twoButton;
    public Button threeButton;
    public Button fourButton;
    public Button fiveButton;
    public Button sixButton;
    public Button sevenButton;
    public Button eightButton;
    public Button nineButton;

    //Users email and gamecode
    private string email = "";
    private string gameCode = "";

    //The block that the user moved
    private int blockToMove;

    //If the good button is pressed, ture otherwise false
    private bool moveSatisfaction = false;

    //The current players suggested move for thes next player
    private string suggestedMove;

    //Create a list of the suggested moves buttons so we can index them
    private List<Button> buttonList = new List<Button>();
    //Create a list that has the button names in it so we can match the text to it, one -> nine
    private List<string> list = new List<string>();
    //Create a list for the satisfaction buttons
    private List<Button> satisfactionButtonList = new List<Button>();
    //Create a list for the satisfaction values, true/false
    private List<string> list2 = new List<string>();

    private int count = 0;

 

    //Method to make the move and then send it to the DatabaseHandler class to be sent to the database
    public void makeMove()
    {
        count++;
        Debug.Log("Count for move: " + count);
        //Make a JSON for this game move
        GameMoveJSON moveJson = new GameMoveJSON(email, blockToMove, suggestedMove, moveSatisfaction, count);
        //Send it to the DatabaseHandler class
        database.logMove(moveJson, gameCode, count);
        
    }


    //Method to highlight the button that the other person selected during their move
    public void changeButton(string suggestedMove, string satisfaction)
    {

        createLists();
    
        int indexSuggested = list.IndexOf(suggestedMove);
        int indexSatisfaction = list2.IndexOf(satisfaction);
       

        var colors = buttonList[indexSuggested].colors;
        colors.normalColor = Color.yellow;
        buttonList[indexSuggested].colors = colors;


        var colors2 = satisfactionButtonList[indexSatisfaction].colors;
        colors2.normalColor = Color.yellow;
        satisfactionButtonList[indexSatisfaction].colors = colors2;

    }

    //Method to clear button color from previous suggestion
    public void clearButtonColor()
    {
        foreach (Button button in buttonList)
        {
            var colors = button.colors;
            colors.normalColor = Color.white;
            button.colors = colors;
        }

        foreach (Button button in satisfactionButtonList)
        {
            var colors = button.colors;
            colors.normalColor = Color.white;
            button.colors = colors;
        }
    }

    //Method that sets up the lists we need to iterate over
    private void createLists()
    {
        satisfactionButtonList.Add(goodButton);
        satisfactionButtonList.Add(poorButton);
        list2.Add("True");
        list2.Add("False");
        buttonList.Add(oneButton);
        buttonList.Add(twoButton);
        buttonList.Add(threeButton);
        buttonList.Add(fourButton);
        buttonList.Add(fiveButton);
        buttonList.Add(sixButton);
        buttonList.Add(sevenButton);
        buttonList.Add(eightButton);
        buttonList.Add(nineButton);
        list.Add("one");
        list.Add("two");
        list.Add("three");
        list.Add("four");
        list.Add("five");
        list.Add("six");
        list.Add("seven");
        list.Add("eight");
        list.Add("nine");
    }

    //Getters and setters for satisfaction
    public void setGoodMove()
    {
        moveSatisfaction = true;
    }

    public void setPoorMove()
    {
        moveSatisfaction = false;
    }

    public bool getSatisfaction()
    {
        return moveSatisfaction;
    }

    public void setSuggestedMove(string name)
    {
        suggestedMove = name;
    }

    public void setUsersEmail(string email)
    {
        this.email = email;
    }

    public void setGameCode(string gameCode)
    {
        this.gameCode = gameCode;
    }

    public void setBlockToMove(int blockToMove)
    {
        //Debug.Log("Block Set: " + blockToMove.coord);
        this.blockToMove = blockToMove;
    }

    public void setCount(int count)
    {
        this.count = count;
    }
}
