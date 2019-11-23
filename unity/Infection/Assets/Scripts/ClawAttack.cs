using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClawAttack : Attack
{
    Movement target;
    public Transform clawRoot;
    public Transform claw;
    float clawToTargetDuration;
    float returnDuration;
    Pose localStartPose;
    public float pullSpeed;
    public float pullDuration;
    public float pullEndPointOffset;

    public float range;
    public float grabRange;
    public LayerMask targetLayers;
    public LayerMask groundLayer;
    public Transform holdPoint;

    public override void Start()
    {
        clawToTargetDuration = startUpLag;
        returnDuration = endLag;
        localStartPose = new Pose(claw.localPosition, claw.localRotation);
        base.Start();
    }
    public override void StartAttack()
    {
        if (PlayerInputController.instance.IsHoldingObject)
        {
            Throw(PlayerInputController.instance.HoldingObject);
        }
        else
        {
            target = AutoTarget(grabRange);
            if (target)
            {
                Grab(target);
            }
            else
            {
                target = AutoTarget(range);
                base.StartAttack();
            }
        }
    }
    public override void OnAttackStartUp()
    {
        //base.OnAttackStartUp();
        Vector3 targetPos;
        Vector3 targetRotFwd;
        if (target)
        {
            target.HitCharacter(Vector3.one * 0.001f, clawToTargetDuration, 0, 0);
            targetPos = target.transform.position;
            targetRotFwd = target.transform.position - clawRoot.transform.position;
        }
        else
        {
            Vector3 forward = self.isFacingRight ? Vector3.right : Vector3.left;
            float noTargetRange = range / 3;
            RaycastHit hit;
            if (Physics.Raycast(claw.transform.position, forward, out hit, noTargetRange, groundLayer))
            {
                noTargetRange = hit.distance;
            }
            targetPos = claw.transform.position + forward * noTargetRange;
            targetRotFwd = forward;
        }

        claw.parent = null;
        claw.transform.right = targetRotFwd;
        claw.transform.DOMove(targetPos, clawToTargetDuration);
    }

    public override void OnAttack()
    {
        base.OnAttack();

    }
    public override void OnEndLagUpdate()
    {
        base.OnEndLagUpdate();
        if (CanTarget(target, grabRange) && Input.GetKey(key))
        {
            EndAttack();
            Grab(target);
        }
    }
    public override void OnEndLagStart()
    {
        base.OnEndLagStart();
        claw.transform.DOMove(clawRoot.transform.position, returnDuration * 5);
        if (target)
        {
            Vector3 pullTargetPos = self.bottomOffset.position + ((self.isFacingRight ? Vector3.right : Vector3.left) * pullEndPointOffset);
            Vector3 targetRotFwd = target.transform.position - pullTargetPos;
            targetRotFwd.Normalize();
            targetRotFwd *= -pullSpeed;
            target.HitCharacter(targetRotFwd, pullDuration, stunTimeZero, 0);
        }
    }
    public override void EndAttack()
    {
        base.EndAttack();
        ReturnClaw();
    }
    public void ReturnClaw()
    {
        claw.DOKill();
        claw.parent = clawRoot;
        claw.localPosition = localStartPose.position;
        claw.localRotation = localStartPose.rotation;
    }

    public Movement AutoTarget(float r)
    {
        Collider[] cols = Physics.OverlapSphere(self.transform.position, r, targetLayers);
        float closest = Mathf.Infinity;
        Movement result = null;
        foreach (Collider col in cols)
        {
            Movement m = col.GetComponentInParent<Movement>();
            if (m == null || m == self) continue;
            float dist = Vector3.Distance(m.transform.position, self.transform.position);
            if (dist <= closest)
            {
                if (CanTarget(m, r))
                {
                    result = m;
                    closest = dist;
                }
            }
        }
        return result;
    }
    public bool CanTarget(Movement m, float r)
    {
        if (m == null) return false;
        float dist = Vector3.Distance(m.transform.position, self.transform.position);
        if (dist > r) return false;

        if (self.isFacingRight)
        {
            if (m.transform.position.x <= self.transform.position.x)
                return false;
        }
        else
        {
            if (m.transform.position.x >= self.transform.position.x)
                return false;
        }
        if (Physics.Raycast(claw.transform.position, m.transform.position - claw.transform.position, dist * .9f, groundLayer))
            return false;

        return true;

    }
    public void Grab(Movement m)
    {
        PlayerInputController.instance.HoldingObject = m;
        m.FreezeRBody();
        m.SimulateInput(Vector2.zero);
        m.transform.SetParent(holdPoint);
        m.transform.localPosition = Vector3.zero;
        m.transform.localRotation = Quaternion.identity;
    }

    public void Throw(Movement m)
    {
        PlayerInputController.instance.HoldingObject = null;
        m.UnFreezeRBody();
        Vector3 throwVelocity = new Vector3(9, 9);
        if (!self.isFacingRight)
            throwVelocity.x *= -1;
        m.transform.SetParent(null);
        m.HitCharacter(throwVelocity, .251f, 3f, 0);
    }
    public override bool CanAttack()
    {
        return base.CanAttack();// && !PlayerInputController.instance.IsHoldingObject;
    }
}
