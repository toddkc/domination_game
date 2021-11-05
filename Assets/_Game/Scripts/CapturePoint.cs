using System;
using UnityEngine;
using ScriptableObjectArchitecture;

public class CapturePoint : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private char captureName;
    [SerializeField] private FloatReference captureTime;
    [SerializeField] private BoolReference logTriggerEvents;

    [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material redTeamMaterial;
    [SerializeField] private Material blueTeamMaterial;
    [SerializeField] private Renderer teamRenderer;

    private float redTime = 0;
    private int redPlayers = 0;
    private float blueTime = 0;
    private int bluePlayers = 0;

    private Team capturedTeam = Team.None;

    public Action<Team> OnPointCaptured;

    public char GetName { get { return captureName; } }

    /// <summary>
    /// Returns the team that currently has this point captured.
    /// </summary>
    public Team GetOwningTeam { get { return capturedTeam; } }

    /// <summary>
    /// Adds the player that entered to the players counted for capturing.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<GamePlayer>();
        if (player == null) return;

        var team = player.GetPlayerTeam;

        if (logTriggerEvents.Value)
        {
            string notif = team + " player entered capture point " + captureName;
            NotificationManager.Instance.AddNotification(notif);
        }

        if (team == Team.Blue)
        {
            bluePlayers++;
        }
        else if (team == Team.Red)
        {
            redPlayers++;
        }

        Debug.Log("Blue Players: " + bluePlayers);
        Debug.Log("Red Players: " + redPlayers);
    }

    /// <summary>
    /// Removes the player from the players counted for capturing.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<GamePlayer>();
        if (player == null) return;

        var team = player.GetPlayerTeam;

        if (logTriggerEvents.Value)
        {
            string notif = team + " player exited capture point " + captureName;
            NotificationManager.Instance.AddNotification(notif);
        }

        if (team == Team.Blue)
        {
            bluePlayers--;
        }
        else if (team == Team.Red)
        {
            redPlayers--;
        }

        if (bluePlayers < 0 || redPlayers < 0)
        {
            Debug.LogError("Capture point thinks it has less than 0 of a team.", this);
        }

        Debug.Log("Blue Players: " + bluePlayers);
        Debug.Log("Red Players: " + redPlayers);
    }

    // TODO: this can probably be improved (capture point update method)
    private void Update()
    {
        if (bluePlayers == redPlayers)
        {
            return;
        }

        if (bluePlayers > redPlayers)
        {
            if (blueTime < captureTime.Value)
            {
                blueTime += Time.deltaTime;
                redTime -= Time.deltaTime;
                redTime = redTime < 0 ? 0 : redTime;
            }
            else if (blueTime >= captureTime.Value && capturedTeam == Team.Red)
            {
                PointLost();
            }
            else if (blueTime >= captureTime.Value && capturedTeam == Team.None)
            {
                PointCaptured(Team.Blue);
            }
        }
        else if (redPlayers > bluePlayers)
        {
            if (redTime < captureTime.Value)
            {
                redTime += Time.deltaTime;
                blueTime -= Time.deltaTime;
                blueTime = blueTime < 0 ? 0 : blueTime;
            }
            else if (redTime >= captureTime.Value && capturedTeam == Team.Blue)
            {
                PointLost();
            }
            else if (redTime >= captureTime.Value && capturedTeam == Team.None)
            {
                PointCaptured(Team.Red);
            }
        }
    }

    /// <summary>
    /// Called when a team has been in the point long enough to capture it.
    /// </summary>
    /// <param name="newOwningTeam"></param>
    private void PointCaptured(Team newOwningTeam)
    {
        string notif = "Point " + captureName + " captured by " + newOwningTeam;
        NotificationManager.Instance.AddNotification(notif);

        capturedTeam = newOwningTeam;

        OnPointCaptured?.Invoke(capturedTeam);

        if (newOwningTeam == Team.Blue)
        {
            teamRenderer.material = blueTeamMaterial;
            redTime = 0;
        }
        else if (newOwningTeam == Team.Red)
        {
            teamRenderer.material = redTeamMaterial;
            blueTime = 0;
        }
    }

    /// <summary>
    /// Called when a team has been in the point long enough that the opposing team loses it.
    /// </summary>
    private void PointLost()
    {
        string notif = capturedTeam + " lost Point " + captureName;
        NotificationManager.Instance.AddNotification(notif);

        capturedTeam = Team.None;

        OnPointCaptured?.Invoke(capturedTeam);

        teamRenderer.material = defaultMaterial;
        redTime = 0;
        blueTime = 0;
    }

    /// <summary>
    /// This should return the percentage of capture so a player can show the progress bar.
    /// </summary>
    public float GetCaptureRatio(Team team)
    {
        // TODO: calculate capture percentage
        return 0;
    }
}
