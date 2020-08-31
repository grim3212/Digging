using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public int lives;
    public int score;
    public int totalTime;
    public int currentTime;

    public GameManager() {
        this.lives = 3;
        this.score = 0;
        this.totalTime = 300;
        this.currentTime = 0;
    }

    public string LivesForDisplay () {
        return "Lives: " + this.lives;
    }

    public string ScoreForDisplay () {
        return "Score: " + score.ToString().PadLeft(5, '0');
    }

    public string TimeForDisplay() {
        return "Time: " + (this.totalTime - this.currentTime).ToString();
    }
}
