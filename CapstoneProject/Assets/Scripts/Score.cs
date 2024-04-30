using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int score = 0;
    public static int playerOneScore = 0;
    public static int playerTwoScore = 0;

    public static bool OneInt = true;
    public static bool TwoInt = true;


    public Text scoreText;
    public Text playerOneScoreText;
    public Text playerTwoScoreText;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        playerOneScore = 0;
        playerTwoScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        playerOneScoreText.text = playerOneScore.ToString();
        playerTwoScoreText.text = playerTwoScore.ToString();
    }
}
