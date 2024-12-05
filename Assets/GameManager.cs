using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public int tortugasEliminadas = 0;
    public int limiteEliminaciones = 2;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverScoreText;
    public UnityEngine.UI.Button reiniciarButton;
    private Water waterScript;
    public Animator animator;

    private void Start()
    {
        waterScript = FindFirstObjectByType<Water>();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (reiniciarButton != null)
        {
            reiniciarButton.onClick.AddListener(ReiniciarJuego);
        }
    }


    public void EliminarTortuga()
    {
        tortugasEliminadas++;
        animator.SetInteger("Muerte", tortugasEliminadas);

        if (tortugasEliminadas >= limiteEliminaciones)
        {
            ActivarGameOver();
        }
    }

    private void ActivarGameOver()
    {
        Time.timeScale = 0f; // Pausar el juego
        gameOverPanel.SetActive(true);

        if (waterScript != null && gameOverScoreText != null)
        {

            gameOverScoreText.text = "Puntaje Final: " + waterScript.totalScore;
        }
    }


    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reiniciar la escena actual
    }
}