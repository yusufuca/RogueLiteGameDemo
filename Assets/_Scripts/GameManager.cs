using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public event Action<int> OnScoreChanged;


    [SerializeField] private TextMeshProUGUI scoreText;
   
    public Transform patrolArea;
    public float patrolAreaLength = 10f;
    public float patrolAreaDepth = 10f;

    public GameObject player;
    public GameObject goblin;
    public bool isDebugTextOpen = true;

    private int currentScore;
    private void Awake()
    {
        gm = this;
        currentScore = 0;
        scoreText.text = currentScore.ToString();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            Instantiate(goblin);
        }
    }
    public void ChangeScore(int score)
    {
        currentScore += score;
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}
