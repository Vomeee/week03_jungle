using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HW_Run : IPlayerState
{

    private HW_PlayerStateController controller;
    private InputSystem_Actions actions;

    public HW_Run(HW_PlayerStateController controller)
    {
        this.controller = controller;
        this.actions = controller.GetInputActions();

    }

    [Header("Run Variables")]
    float maxRunSpeed = 80f;
    float runForce = 1000f;
    float runJumpForce = 11000f;

    public void EnterState()
    {
        Debug.Log("Enter Run");
        actions.Player.Crouch.canceled += ToWalkState;
        actions.Player.Attack.performed += ToDashState;
        actions.Player.Jump.performed += ToAirState;
    }

    private void ToWalkState(InputAction.CallbackContext context)
    {
        HW_PlayerStateController.Instance.ChangeState(new HW_Walk(controller));
    }

    private void ToDashState(InputAction.CallbackContext context)
    {
        HW_PlayerStateController.Instance.ChangeState(new HW_Dash(controller));
    }

    private void ToAirState(InputAction.CallbackContext context)
    {
        PlayerMoveManager.Instance.MoveByImpulse(Vector3.up * runJumpForce);
        HW_PlayerStateController.Instance.ChangeState(new HW_Air(controller));
    }

    
    public void ExitState()
    {
        Debug.Log("Exit run");
        actions.Player.Sprint.canceled -= ToWalkState;
        actions.Player.Attack.performed -= ToDashState;
        actions.Player.Jump.performed -= ToAirState;
    }

    public void UpdateState()
    {
        Vector2 moveVector = actions.Player.Move.ReadValue<Vector2>();
        if (moveVector.magnitude < 0.1f) return; // �Է��� ������ ����

        // ī�޶� ���� ���� ���
        Transform cameraTransform = Camera.main.transform; // PlayerMoveManager���� ī�޶� ������
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0; // ���� �̵���
        cameraRight.y = 0;
        Vector3 moveDirection = (cameraForward * moveVector.y + cameraRight * moveVector.x).normalized;

        // �� ���� (�ӵ� ����)
        PlayerMoveManager.Instance.MoveByForce(moveDirection * runForce);

        // ĳ���� ������ �̵� ���⿡ ���� (ī�޶� ����)
        Rigidbody rb = PlayerMoveManager.Instance.GetComponent<Rigidbody>();
        if (moveVector.magnitude > 0.1f) // �Է��� ���� ���� ȸ��
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 5f));
        }

        // �ӵ� ����
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVelocity.magnitude > maxRunSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxRunSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }
}

