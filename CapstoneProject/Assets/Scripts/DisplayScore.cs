using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{

    public Text endingScoreText;
    public Text endingScoreTextOne;
    public Text endingScoreTextTwo;

    // Start is called before the first frame update
    void Start()
    {
        endingScoreText.text = Score.score.ToString();

        //won't display below
        endingScoreTextOne.text = Score.playerOneScore.ToString();
        endingScoreTextTwo.text = Score.playerTwoScore.ToString();
    }

}
