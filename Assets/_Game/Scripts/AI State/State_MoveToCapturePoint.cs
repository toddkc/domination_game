using UnityEngine;
using UnityEngine.AI;

public class State_MoveToCapturePoint : IState
{
    private NPC _owner;
    private NavMeshAgent _agent;
    private Vector3 _lastPosition = Vector3.zero;

    public float TimeStuck { get; private set; }

    public State_MoveToCapturePoint(NPC owner, NavMeshAgent agent)
    {
        _owner = owner;
        _agent = agent;
    }

    public void OnEnter()
    {
        _owner.stateDisplayText.text = "Moving to capture point...";
        TimeStuck = 0f;
        _agent.enabled = true;
        _agent.SetDestination(GetPointInsideArea());
    }

    public void OnExit()
    {
        _agent.enabled = false;
    }

    public void Tick()
    {
        if (Vector3.Distance(_owner.transform.position, _lastPosition) <= 0f)
        {
            TimeStuck += Time.deltaTime;
        }

        _lastPosition = _owner.transform.position;
    }

    private Vector3 GetPointInsideArea()
    {
        var _point = (Random.insideUnitSphere * 5) + _owner.TargetPoint.transform.position;
        _point.y = 0;
        return _point;
    }
}
