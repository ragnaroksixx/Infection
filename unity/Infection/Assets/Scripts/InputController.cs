using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputController : MonoBehaviour
{
    public virtual void Start()
    {

    }
    public virtual void Update()
    {

    }

    public virtual void SetInput(Movement m)
    {

    }

    public virtual void SetAttacks(Movement m)
    {

    }

    public virtual bool IsHoldingJump(Movement m)
    {
        return false;
    }

    public virtual bool IsFastFall(Movement m)
    {
        return false;
    }

    public virtual bool Jump(Movement m)
    {
        return false;
    }
}

