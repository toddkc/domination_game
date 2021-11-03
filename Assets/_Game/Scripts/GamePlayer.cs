using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    [SerializeField] private Team playerTeam;

    public Team GetPlayerTeam
    {
        get { return playerTeam; }
    }
}

