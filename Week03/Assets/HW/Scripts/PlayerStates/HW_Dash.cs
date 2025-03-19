using System;
using UnityEngine;

public class HW_Dash : IPlayerState
{
    private PlayerMoveManager playerMoveManager;
    private HW_PlayerStateController controller;
    private InputSystem_Actions actions;
    private Rigidbody rigidBody;

    public HW_Dash(HW_PlayerStateController controller)
    {
        this.controller = controller;
        this.actions = controller.GetInputActions();
        rigidBody = PlayerMoveManager.Instance.GetComponent<Rigidbody>();
        playerMoveManager = PlayerMoveManager.Instance;
        playerMoveManager.onGroundedAction += ToRunState;
    }

    float elapsedTimeFromDash = 0f;
    float dashForce = 6000f; // 각도로 받는 점프력.
    float dashAngleY = 15f; // 힘을 받는 각도.

    public void EnterState()
    {
        Vector2 moveVector = actions.Player.Move.ReadValue<Vector2>();
        if (moveVector.magnitude < 0.1f)
        {
            moveVector = new Vector2(0, 1); // 기본 forward 방향
        }

        // 카메라 기준 방향 계산 (수평)
        Transform cameraTransform = Camera.main.transform;
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        Vector3 horizontalDirection = (cameraForward * moveVector.y + cameraRight * moveVector.x).normalized;

        // Y축 상향 각도 적용
        float yComponent = Mathf.Sin(dashAngleY * Mathf.Deg2Rad); // 약 0.2588
        Vector3 dashDirection = horizontalDirection; // 수평 방향 복사
        dashDirection.y = yComponent; // Y 성분을 양수로 고정
        dashDirection = dashDirection.normalized;

        // 디버깅
        Debug.Log("Horizontal Direction: " + horizontalDirection);
        Debug.Log("Dash Direction: " + dashDirection + ", Y Component: " + dashDirection.y);

        // 대쉬 힘 적용
        PlayerMoveManager.Instance.MoveByImpulse(dashDirection * dashForce);

        elapsedTimeFromDash = 0f;

    }

    private void ToRunState()
    {
        HW_PlayerStateController.Instance.ChangeState(new HW_Walk(controller));
    }

    public void ExitState()
    {
        //throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        //throw new System.NotImplementedException();
    }
}
