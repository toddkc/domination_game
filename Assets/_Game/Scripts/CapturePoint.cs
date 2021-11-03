using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    [SerializeField] private char captureName;
    [SerializeField] private float captureTime = 5;
    [SerializeField] private Material redTeamMaterial;
    [SerializeField] private Material blueTeamMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Renderer teamRenderer;

    private float redTime = 0;
    private int redPlayers = 0;

    private float blueTime = 0;
    private int bluePlayers = 0;

    private Team capturedTeam = Team.None;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<GamePlayer>();
        if (player == null) return;

        var team = player.GetPlayerTeam;
        Debug.Log(team + " player entered capture point " + captureName);

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

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<GamePlayer>();
        if (player == null) return;

        var team = player.GetPlayerTeam;
        Debug.Log(team + " player exited capture point " + captureName);

        if (team == Team.Blue)
        {
            bluePlayers--;
        }
        else if (team == Team.Red)
        {
            redPlayers--;
        }

        if(bluePlayers < 0 || redPlayers < 0)
        {
            Debug.LogError("Capture point thinks it has less than 0 of a team.", this);
        }

        Debug.Log("Blue Players: " + bluePlayers);
        Debug.Log("Red Players: " + redPlayers);
    }

    private void Update()
    {
        // figure out who is in

        // update captured state if necessary

        // send GM score update if necessary
    }
}
