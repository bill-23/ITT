using System;

public class GameData
{
    
    public string playerOneType;
    public string playerTwoType;

    public string playerOneEmail;
    public string playerTwoEmail;

    public string gameType;

    public GameData() { }

    public GameData(string playerOneType, string playerTwoType, string playerOneEmail, 
        string playerTwoEmail, string gameType) {
       
        this.playerOneType = playerOneType;
        this.playerTwoType = playerTwoType;
        this.playerOneEmail = playerOneEmail;
        this.playerTwoEmail = playerTwoEmail;
        
        this.gameType = gameType;
    
    }
}
