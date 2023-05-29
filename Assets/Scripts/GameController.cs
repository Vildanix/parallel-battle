using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField] bool isGameOver = false;
    [SerializeField] int score = 0;

    public UnityEvent onNewGame;
    public UnityEvent<string> onGameOver;

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        isGameOver = false;
        score = 0;
        onNewGame.Invoke();
    }

    public void EndGame()
    {
        isGameOver = true;
        onGameOver.Invoke(score.ToString());
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public int GetScore()
    {
        return score;
    }
}
