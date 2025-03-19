using System;
using UnityEngine;

public class HW_AirDash : IPlayerState
{
    private HW_PlayerStateController controller;
    private InputSystem_Actions actions;
    private PlayerMoveManager playerMoveManager;

    public HW_AirDash(HW_PlayerStateController controller)
    {
        this.controller = controller;
        this.actions = controller.GetInputActions();
        playerMoveManager = PlayerMoveManager.Instance;
    }

    float airDashElapsedTime = 0f;
    float airDashTime = 0.3f;
    float airDashVelocity = 50f; // 각도로 받는 점프력.
    float airDashAngleY = 3f; // 힘을 받는 각도.
    float controlEnableTime = 1f;
    float elapsedControlEnableTime = 0;
    bool airDashEnd = false;
    Vector3 finalAirDashDirection;

    public void EnterState()
    {
        playerMoveManager.ManageJumpBool(true);

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
        float yComponent = Mathf.Sin(airDashAngleY * Mathf.Deg2Rad); // 약 0.2588
        Vector3 dashDirection = horizontalDirection; // 수평 방향 복사
        dashDirection.y = yComponent; // Y 성분을 양수로 고정
        finalAirDashDirection = dashDirection.normalized;

    }

    public void ExitState()
    {

    }

    public void UpdateState()
    {
        elapsedControlEnableTime += Time.deltaTime;

        if (!airDashEnd && finalAirDashDirection != null)
        {
            airDashElapsedTime += Time.deltaTime;

            playerMoveManager.MoveByVelocity(finalAirDashDirection * airDashVelocity);

            if (airDashElapsedTime > airDashTime)
            {
                airDashEnd = true;
            }
        }

        if(elapsedControlEnableTime > controlEnableTime)
        {
            HW_PlayerStateController.Instance.ChangeState(new HW_AirRun(controller));
        }


    }
}
