using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClawAttack : Attack
{
    public Movement target;
    public Transform clawRoot;
    public Transform claw;
    float clawToTargetDuration;
    float returnDuration;
    Pose localStartPose;
    public float pullSpeed;
    public float pullDuration;
    public float pullEndPointOffset;
    public override void Start()
    {
        clawToTargetDuration = startUpLag;
        returnDuration = endLag;
        localStartPose = new Pose(claw.localPosition, claw.localRotation);
        base.Start();
    }

    public override void OnAttackStartUp()
    {
        //base.OnAttackStartUp();
        target.HitCharacter(Vector3.one * 0.001f, clawToTargetDuration);

        Vector3 targetPos = target.transform.position;
        Vector3 targetRotFwd = target.transform.position - clawRoot.transform.position;

        claw.parent = null;
        claw.transform.right = targetRotFwd;
        claw.transform.DOMove(targetPos, clawToTargetDuration);
    }

    public override void OnAttack()
    {
        base.OnAttack();

    }
    public override void OnEndLagStart()
    {
        base.OnEndLagStart();
        claw.transform.DOMove(clawRoot.transform.position, returnDuration * 5);
        Vector3 pullTargetPos = self.bottomOffset.position + ((self.isFacingRight ? Vector3.right : Vector3.left) * pullEndPointOffset);
        Vector3 targetRotFwd = target.transform.position - pullTargetPos;
        targetRotFwd.Normalize();
        targetRotFwd *= -pullSpeed;
        target.HitCharacter(targetRotFwd, pullDuration);
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
}
