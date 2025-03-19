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
    float dashForce = 6000f; // ������ �޴� ������.
    float dashAngleY = 15f; // ���� �޴� ����.

    public void EnterState()
    {
        Vector2 moveVector = actions.Player.Move.ReadValue<Vector2>();
        if (moveVector.magnitude < 0.1f)
        {
            moveVector = new Vector2(0, 1); // �⺻ forward ����
        }

        // ī�޶� ���� ���� ��� (����)
        Transform cameraTransform = Camera.main.transform;
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        Vector3 horizontalDirection = (cameraForward * moveVector.y + cameraRight * moveVector.x).normalized;

        // Y�� ���� ���� ����
        float yComponent = Mathf.Sin(dashAngleY * Mathf.Deg2Rad); // �� 0.2588
        Vector3 dashDirection = horizontalDirection; // ���� ���� ����
        dashDirection.y = yComponent; // Y ������ ����� ����
        dashDirection = dashDirection.normalized;

        // �����
        Debug.Log("Horizontal Direction: " + horizontalDirection);
        Debug.Log("Dash Direction: " + dashDirection + ", Y Component: " + dashDirection.y);

        // �뽬 �� ����
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
