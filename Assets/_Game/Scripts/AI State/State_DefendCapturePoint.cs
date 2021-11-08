using UnityEngine;

public class State_DefendCapturePoint : IState
{
    private NPC _owner;

    public State_DefendCapturePoint(NPC owner)
    {
        _owner = owner;
    }

    public void OnEnter()
    {
        _owner.stateDisplayText.text = "Defending capture point...";
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}
