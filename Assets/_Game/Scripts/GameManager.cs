using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int startingScore = 100;
    [SerializeField] private float scoreCheckDelay = 3;
    [SerializeField] private int maxPointsPerCheck = 5;

    [Header("For debug only...")]
    [SerializeField] private int redScore;
    [SerializeField] private int blueScore;

    private float scoreCheckTimer = 0;
    private List<CapturePoint> capturePoints = new List<CapturePoint>();
    private bool isGameRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        redScore = blueScore = startingScore;

        Debug.Log("Red Team Score: " + redScore);
        Debug.Log("Blue Team Score: " + blueScore);

        isGameRunning = true;
    }

    /// <summary>
    /// Check points to update score here
    /// Doing this in update so eventually if pause is implemented as a timescale adjustment score won't keep changing while paused
    /// </summary>
    private void Update()
    {
        if (!isGameRunning) return;

        scoreCheckTimer += Time.deltaTime;
        if (scoreCheckTimer >= scoreCheckDelay)
        {
            CheckPoints();
            scoreCheckTimer = 0;
        }
    }

    /// <summary>
    /// Adds a capture point to the game manager for tracking.
    /// </summary>
    /// <param name="point">The point.</param>
    public void RegisterCapturePoint(CapturePoint point)
    {
        if (capturePoints.Contains(point)) return;
        capturePoints.Add(point);
    }

    /// <summary>
    /// Check which team owns which points to update scores.
    /// </summary>
    private void CheckPoints()
    {
        int _blue = 0;
        int _red = 0;
        foreach (var point in capturePoints)
        {
            if (point.GetOwningTeam == Team.Blue) _blue++;
            else if (point.GetOwningTeam == Team.Red) _red++;
        }

        if (_blue == _red) return;

        float highTeam = _blue > _red ? _blue : _red;
        float percentage = highTeam / capturePoints.Count;
        int points = (int)(percentage * maxPointsPerCheck);

        if (_blue > _red) redScore -= points;
        else if (_blue < _red) blueScore -= points;

        if (redScore <= 0 || blueScore <= 0)
        {
            CheckScoreGameOver();
        }
    }

    /// <summary>
    /// Determines the winning team.
    /// </summary>
    private void CheckScoreGameOver()
    {
        if (redScore > 0 && blueScore > 0)
        {
            Debug.LogError("GameManager thinks game is over, but it is not?", this);
            return;
        }

        string notif = string.Empty;

        if (redScore <= 0 && blueScore <= 0)
        {
            notif = "Game Over - No Winner";
        }

        if (redScore <= 0)
        {
            notif = "Game Over - Blue Team Wins";
        }
        else if (blueScore <= 0)
        {
            notif = "Game Over - Red Team Wins";
        }

        NotificationManager.Instance.AddNotification(notif);

        isGameRunning = false;
    }
}

/// <summary>
/// Red vs Blue
/// </summary>
public enum Team
{
    None = 0,
    Red = 1,
    Blue = 2
}