using UnityEngine;
using System.Collections;

public static class InputHandler
{

    static InputHandler()
    {
    }

    public static bool IsMovingLeft()
    {
        return Input.GetAxisRaw("Move") < 0;
    }
    public static bool IsMovingRight()
    {
        return Input.GetAxisRaw("Move") > 0;
    }
    public static bool IsJumpKey()
    {
        return Input.GetButton("Jump");
    }
    public static bool IsJumpKeyDown()
    {
        return Input.GetButtonDown("Jump");
    }
    public static bool IsFastFall()
    {
        return Input.GetButton("FastFall") || Input.GetAxisRaw("FastFall") > 0;
    }
}
