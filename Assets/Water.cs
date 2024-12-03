using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Water : MonoBehaviour
{
    public int totalScore = 0;
    public TextMeshProUGUI scoreText;
    private void Start()
    {
        UpdateScoreUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        TortugaOF turtle = collision.GetComponent<TortugaOF>();
        if (turtle != null)
        {
            totalScore += turtle.points;
            UpdateScoreUI();

        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Puntaje: " + totalScore;
    }
}

