using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(GamePlayer))]
public class NPC : MonoBehaviour
{
    private StateMachine _stateMachine;
    public CapturePoint TargetPoint { get; set; }
    public new Transform transform { get; private set; }

    [SerializeField] private Text stateDisplay;
    [SerializeField] private float destinationOffset = 1;
    public Text stateDisplayText { get { return stateDisplay; } }

    private GamePlayer gamePlayer;

    private void Awake()
    {
        _stateMachine = new StateMachine();

        transform = GetComponent<Transform>();
        gamePlayer = GetComponent<GamePlayer>();
        var navMeshAgent = GetComponent<NavMeshAgent>();

        var fcp = new State_FindCapturePoints(this, gamePlayer.GetPlayerTeam, FindObjectsOfType<CapturePoint>().ToList());
        var mcp = new State_MoveToCapturePoint(this, navMeshAgent);
        var dcp = new State_DefendCapturePoint(this);

        _stateMachine.AddTransition(fcp, mcp, () => TargetPoint != null);
        _stateMachine.AddTransition(mcp, dcp, HasReachedDestination());

        _stateMachine.AddAnyTransition(fcp, HasDefendedPoint());

        _stateMachine.SetState(fcp);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    Func<bool> HasReachedDestination() => () => TargetPoint != null &&
        (Vector3.Distance(transform.position, TargetPoint.transform.position) < destinationOffset);

    Func<bool> HasDefendedPoint() => () => TargetPoint != null && TargetPoint.GetOwningTeam == gamePlayer.GetPlayerTeam;
}
