using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class State_FindCapturePoints : IState
{
    private List<CapturePoint> _points;
    private Team _team;
    private NPC _owner;

    public State_FindCapturePoints(NPC owner, Team team, List<CapturePoint> points)
    {
        _owner = owner;
        _team = team;
        _points = points;
    }

    public void OnEnter()
    {
        _owner.stateDisplayText.text = "Finding capture point...";
    }

    public void OnExit()
    {
        //
    }

    public void Tick()
    {
        _owner.TargetPoint = FindNewTargetPoint();
    }

    private CapturePoint FindNewTargetPoint()
    {
        return _points
             .OrderBy(t => Vector3.Distance(_owner.transform.position, t.transform.position))
             .Where(t => t.GetOwningTeam != _team)
             .OrderBy(t => Random.Range(0, int.MaxValue))
             .FirstOrDefault();
    }
}
