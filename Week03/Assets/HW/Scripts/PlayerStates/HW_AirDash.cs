using UnityEngine;

public class HW_AirDash : IPlayerState
{
    private HW_PlayerStateController controller;
    private InputSystem_Actions actions;

    public HW_AirDash(HW_PlayerStateController controller)
    {
        this.controller = controller;
        this.actions = controller.GetInputActions();
    }

    public void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
