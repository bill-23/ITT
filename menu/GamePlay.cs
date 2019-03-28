using System;

public class GamePlay
{
    Puzzle puzzle = new Puzzle();
    DatabaseHandler database = new DatabaseHandler();


    public Button goodButton;
    public Button poorButton;
    public Button doneButton;

    //If the good button is pressed, ture otherwise false
    private bool moveSatisfaction = false;
    

	public GamePlay() { }

    public void setGoodMove() {
        moveSatisfaction = true;
    }

    public void setPoorMove() {
        moveSatisfaction = false;
    }

    public bool getSatisfaction() {
        return moveSatisfaction;
    }

    public void makeMove() {
        GameMoveJSON moveJson = new GameMoveJSON("", puzzle.blockToMove, "", moveSatisfaction);
        database.logMove(moveJson, database.GameCode);

    }
}
