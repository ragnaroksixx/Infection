using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public static class InputHandler
{
    static InfectionControls controls = new InfectionControls();

    static InputHandler()
    {
        controls.Player.Enable();
    }

    public static bool IsMovingLeft()
    {
        return controls.Player.Move.ReadValue<Vector2>().x < 0;
    }
    public static bool IsMovingRight()
    {
        return controls.Player.Move.ReadValue<Vector2>().x > 0;
    }
    public static bool IsJumpKey()
    {
        return controls.Player.SlowFall.phase == InputActionPhase.Started;
    }
    public static bool IsJumpKeyDown()
    {
        return controls.Player.Jump.triggered;
    }
    public static bool IsFastFall()
    {
        return controls.Player.FastFall.phase == InputActionPhase.Started;
    }

    public static bool IsAttackDown()
    {
        return false;
    }
}
