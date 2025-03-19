using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HW_Idle : IPlayerState
{
    private HW_PlayerStateController controller;
    private InputSystem_Actions actions;

    public HW_Idle(HW_PlayerStateController controller)
    {
        this.controller = controller;
        this.actions = controller.GetInputActions();
    }

    float idleJumpForce = 15000f;

    public void EnterState()
    {
        actions.Player.Move.performed += ToWalkState; //움직임 -> Walk
        actions.Player.Jump.performed += ToAirState; //점프 -> Jump
    }

    private void ToAirState(InputAction.CallbackContext context)
    {
        PlayerMoveManager.Instance.MoveByImpulse(Vector3.up * idleJumpForce);
        HW_PlayerStateController.Instance.ChangeState(new HW_Air(controller));
    }

    private void ToWalkState(InputAction.CallbackContext context)
    {
        HW_PlayerStateController.Instance.ChangeState(new HW_Walk(controller));
    }

    public void ExitState()
    {
        Debug.Log("Exit Idle");

        //액션 제거.
        actions.Player.Move.performed -= ToWalkState; 
        actions.Player.Jump.performed -= ToAirState;       
    }

    public void UpdateState()
    {
        
    }


}
