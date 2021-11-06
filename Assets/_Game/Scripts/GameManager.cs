using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScriptableObjectArchitecture;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private IntReference startingScore;
    [SerializeField] private FloatReference scoreCheckDelay;
    [SerializeField] private IntReference maxPointsPerCheck;
    [SerializeField] private FloatReference pauseTimescale;

    [SerializeField] private IntReference redScoreRef;
    [SerializeField] private IntReference blueScoreRef;
    
    [SerializeField] private GameEvent togglePauseEvent;

    // TODO: remove these
    [Header("For debug only...")]
    [SerializeField] private int redScore;
    [SerializeField] private int blueScore;

    private float scoreCheckTimer = 0;
    private List<CapturePoint> capturePoints;
    private bool isGameRunning = false;
    private bool canPlayerToggleGameRunning = true;

    public GameState GetGameState { get { return currentGameState; } }
    private GameState currentGameState;

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
        capturePoints = FindObjectsOfType<CapturePoint>().ToList();
        redScore = blueScore = startingScore.Value;
        redScoreRef.Value = blueScoreRef.Value = startingScore.Value;
        isGameRunning = true;
        currentGameState = GameState.Running;
    }

    /// <summary>
    /// Check points to update score here
    /// Doing this in update so eventually if pause is implemented as a timescale adjustment score won't keep changing while paused
    /// </summary>
    private void Update()
    {
        if (!isGameRunning) return;

        scoreCheckTimer += Time.deltaTime;
        if (scoreCheckTimer >= scoreCheckDelay.Value)
        {
            CheckPoints();
            scoreCheckTimer = 0;
        }
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
        int points = (int)(percentage * maxPointsPerCheck.Value);

        if (_blue > _red)
        {
            redScore -= points;
            redScoreRef.Value -= points;
        }
        else if (_blue < _red)
        {
            blueScore -= points;
            blueScoreRef.Value -= points;
        }

        if (redScoreRef.Value <= 0 || blueScoreRef.Value <= 0)
        {
            CheckScoreGameOver();
        }
    }

    /// <summary>
    /// Determines the winning team.
    /// </summary>
    private void CheckScoreGameOver()
    {
        if (redScoreRef.Value > 0 && blueScoreRef.Value > 0)
        {
            Debug.LogError("GameManager thinks game is over, but it is not?", this);
            return;
        }

        string notif = string.Empty;

        if (redScoreRef.Value <= 0 && blueScoreRef.Value <= 0)
        {
            redScoreRef.Value = blueScoreRef.Value = 0;
            notif = "Game Over - No Winner";
        }

        if (redScoreRef.Value <= 0)
        {
            redScoreRef.Value = 0;
            notif = "Game Over - Blue Team Wins";
        }
        else if (blueScoreRef.Value <= 0)
        {
            blueScoreRef.Value = 0;
            notif = "Game Over - Red Team Wins";
        }

        NotificationManager.Instance.AddNotification(notif);

        PauseGame(false);
    }

    /// <summary>
    /// Pause the game time.
    /// </summary>
    public void PauseGame(bool canPlayerUnpause = true)
    {
        Time.timeScale = pauseTimescale.Value;
        isGameRunning = false;
        canPlayerToggleGameRunning = canPlayerUnpause;

        currentGameState = canPlayerToggleGameRunning ?
            GameState.Paused :
            GameState.End;

        togglePauseEvent.Raise();
    }

    /// <summary>
    /// Called when the player requests the game to be paused/resumed.
    /// </summary>
    public void RequestTogglePause()
    {
        if (!canPlayerToggleGameRunning) return;
        if (isGameRunning)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    /// <summary>
    /// Resumes the game time if player is allowed to unpause.
    /// </summary>
    public bool ResumeGame()
    {
        if (canPlayerToggleGameRunning)
        {
            Time.timeScale = 1;
            isGameRunning = true;
            currentGameState = GameState.Running;
            togglePauseEvent.Raise();
            return true;
        }

        return false;
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

/// <summary>
/// The state of the game.
/// </summary>
public enum GameState
{
    Running = 0,
    Paused = 1,
    End = 2
}