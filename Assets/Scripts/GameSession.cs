using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI numScore;
    [SerializeField] TextMeshProUGUI numLives;

    private void Start() {
        numLives.text = playerLives.ToString();
        numScore.text = score.ToString();
    }
    
    private void Awake() {

        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            playerLives -=1;
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
            numLives.text = playerLives.ToString();
        }
        else
        {
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        numScore.text = score.ToString();
    }

}
