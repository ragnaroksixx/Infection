using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputController : MonoBehaviour
{
    public Movement movement;
    protected Attack[] attacks;

    public virtual void Awake()
    {
        attacks = new Attack[0];

    }
    private void Start()
    {
        SetMovement(movement);
    }
    public void SetMovement(Movement m)
    {
        movement = m;
        if (movement)
            movement.SetController(this);
    }
    public virtual void SetInput()
    {

    }

    public virtual void SetAttacks()
    {

    }
    public void AddAttacks(params Attack[] atks)
    {
        attacks = atks;
    }

    public virtual bool IsHoldingJump()
    {
        return false;
    }

    public virtual bool IsFastFall()
    {
        return false;
    }

    public virtual bool Jump()
    {
        return false;
    }
}

