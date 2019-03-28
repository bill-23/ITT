using System;
using UnityEngine;

public class GameMoveJSON
{
    public string email;
    public string suggestedMove;
    public bool satisfaction;
    public int blockToMove;
    public int count;

    public GameMoveJSON() { }

    public GameMoveJSON(string email, int blockToMove, string suggestedMove, bool satisfaction, int count) {
        this.email = email;
        this.blockToMove = blockToMove;
        this.suggestedMove = suggestedMove;
        this.satisfaction = satisfaction;
        this.count = count;
    }

}

