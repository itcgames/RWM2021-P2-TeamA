using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public Text scoreText;
    public int score;
    private GameObject[] enemies;

    private void Start()
    {
        scoreText.text = "" + score;
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "" + score;
    }
}
