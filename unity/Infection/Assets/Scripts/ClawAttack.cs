using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClawAttack : Attack
{
    Movement target;
    float clawToTargetDuration;
    float returnDuration;
    Pose localStartPose;
    Pose throwRootlocalStartPose;
    public float pullSpeed;
    public float pullDuration;
    public float pullEndPointOffset;

    public float range;
    public float grabRange;
    public LayerMask targetLayers;
    public LayerMask groundLayer;
    public Transform holdPoint;

    public Animator holdAnim, grabAnim;

    public Transform throwRoot;
    Transform throwRootParentCache;

    public Transform grappleArmObject;
    Transform grappleArmParentCache;

    public Transform throwSourcePoint;
    private void Awake()
    {
        SaveLoad.UpdateCollectibles();
    }
    public override void Start()
    {
        clawToTargetDuration = startUpLag;
        returnDuration = endLag;
        localStartPose = new Pose(grappleArmObject.localPosition, grappleArmObject.localRotation);
        throwRootlocalStartPose = new Pose(throwRoot.localPosition, throwRoot.localRotation);
        throwRootParentCache = throwRoot.parent;
        grappleArmParentCache = grappleArmObject.parent;
        base.Start();
    }
    public override void StartAttack()
    {
        isPullingObject = false;
        isPullingSelf = false;
        if (PlayerInputController.instance.IsHoldingObject)
        {
            Throw(PlayerInputController.instance.HoldingObject);
        }
        else
        {
            target = AutoTarget(grabRange);
            if (target && CanGrabObject(target))
            {
                Grab(target);
            }
            else
            {
                target = AutoTarget(range);
                AudioManager.Play(sfx, self.transform.position);
                base.StartAttack();
            }

        }
    }
    public override void OnAttackStartUp()
    {
        //base.OnAttackStartUp();
        Vector3 targetPos;
        Vector3 targetRotFwd;
        PlayerInputController.instance.originalPlayer.UseSeperateArm(false);
        if (target)
        {
            target.HitCharacter(Vector3.one * 0.001f, clawToTargetDuration, 0, 0);
            targetPos = target.transform.position;
            targetRotFwd = target.transform.position - throwSourcePoint.position;
        }
        else
        {
            Vector3 forward = self.isFacingRight ? Vector3.right : Vector3.left;
            float noTargetRange = range / 3;
            RaycastHit hit;
            if (Physics.Raycast(throwSourcePoint.position, forward, out hit, noTargetRange, groundLayer))
            {
                noTargetRange = hit.distance;
            }
            targetPos = throwSourcePoint.position + forward * noTargetRange;
            targetRotFwd = forward;
        }

        throwRoot.parent = null;

        grappleArmObject.parent = throwRoot;
        throwRoot.transform.forward = targetRotFwd;
        //grappleArmObject.localPosition = Vector3.zero;
        //grappleArmObject.localRotation = Quaternion.identity;

        throwRoot.transform.DOMove(targetPos, clawToTargetDuration);
        grabAnim.SetTrigger("grab");
    }

    public override void OnAttack()
    {
        base.OnAttack();

    }

    bool isPullingObject;
    bool isPullingSelf;
    Vector3 pullDir;

    public override void OnEndLagStart()
    {
        base.OnEndLagStart();
        throwRoot.transform.DOMove(throwSourcePoint.position, returnDuration * 5);
        if (target)
        {
            Vector3 pullTargetPos = self.bottomOffset.position + ((self.isFacingRight ? Vector3.right : Vector3.left) * pullEndPointOffset);
            Vector3 targetRotFwd = target.transform.position - pullTargetPos;
            targetRotFwd.Normalize();
            targetRotFwd *= -pullSpeed;
            grabAnim.SetTrigger("stopGrab");
            if (CanPullObject(target))
            {
                isPullingObject = true;
                target.HitCharacter(targetRotFwd, pullDuration, stunTimeZero, 0);
            }
            else if (SaveLoad.hasClaw)
            {
                isPullingSelf = true;
                PullSelf(target);
            }
        }
    }
    public override void OnEndLagUpdate()
    {
        base.OnEndLagUpdate();
        if (isPullingObject)
        {
            if (CanTarget(target, grabRange) && KeyDown() && CanGrabObject(target))
            {
                EndAttack();
                Grab(target);
            }
        }
        if (isPullingSelf)
        {
            // self.SimulateInput(pullDir);
        }
    }
    float pullStrength = 20;
    float minPull = 10, maxPull = 15;
    public void PullSelf(Movement target)
    {
        pullDir = -self.transform.position + target.transform.position;
        //pullDir.y = 0;
        pullDir.z = 0;
        pullDir.Normalize();
        pullDir *= pullStrength;
        //pullDir.x = Mathf.Sign(pullDir.x) * Mathf.Clamp(Mathf.Abs(pullDir.x), minPull, maxPull);
        //pullDir.y = Mathf.Sign(pullDir.y) * Mathf.Clamp(Mathf.Abs(pullDir.y), minPull, maxPull);
        PullOverride po = target.GetComponent<PullOverride>();
        if (po)
        {
            pullDir.x *= po.dampener.x;
            pullDir.y *= po.dampener.y;
            pullDir.z *= po.dampener.z;
        }
        self.HitCharacter(pullDir, pullDuration, 0, 0);
    }
    public override void EndAttack()
    {
        if (isPullingSelf)
            self.StopSimulateInput();
        base.EndAttack();
        ReturnClaw();
    }
    public void ReturnClaw()
    {
        throwRoot.DOKill();

        grappleArmObject.parent = grappleArmParentCache;
        grappleArmObject.localPosition = localStartPose.position;
        grappleArmObject.localRotation = localStartPose.rotation;

        throwRoot.parent = throwRootParentCache;
        throwRoot.localPosition = Vector3.zero;
        throwRoot.localRotation = Quaternion.identity;

        if (!PlayerInputController.instance.IsHoldingObject)
            PlayerInputController.instance.originalPlayer.UseAttachedArm();
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
            if (dist <= closest || (result && m.targetPriority > result.targetPriority))
            {
                if (m.targetPriority < 0) continue;
                if (!CanPullObject(m) && !SaveLoad.hasClaw) continue;
                if (result && m.targetPriority < result.targetPriority) continue;
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
        float dist = Vector3.Distance(m.transform.position, throwSourcePoint.position);
        if (dist > r) return false;

        float angle = Vector3.SignedAngle(-self.transform.position + m.transform.position, self.transform.up, Vector3.up);
        if (self.isFacingRight)
        {
            if (m.transform.position.x <= self.transform.position.x)
            {
                if (angle > 30)
                    return false;
            }

        }
        else
        {
            if (m.transform.position.x >= self.transform.position.x)
            {
                if (angle > 30)
                    return false;
            }
        }
        RaycastHit hit;
        if (Physics.Raycast(throwSourcePoint.position, m.transform.position - throwSourcePoint.position, out hit, dist * .9f, groundLayer))
        {
            Debug.DrawRay(throwSourcePoint.position, m.transform.position - throwSourcePoint.position, Color.red, 5);
            Transform t = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            t.position = hit.point;
            t.localScale = Vector3.one * 0.1f;

            return false;
        }


        return true;

    }
    public void Grab(Movement m)
    {
        PlayerInputController.instance.HoldingObject = m;
        holdAnim.SetBool("isHolding", true);
        PlayerInputController.instance.originalPlayer.UseSeperateArm(true);
        IGrabable grabable = m as IGrabable;
        grabable.OnGrab();
        m.FreezeRBody();
        m.SimulateInput(Vector2.zero);
        m.transform.SetParent(holdPoint);
        m.transform.localPosition = Vector3.zero;
        m.transform.localRotation = Quaternion.identity;
    }

    public void Throw(Movement m)
    {
        Drop();
        m.UnFreezeRBody();
        Vector3 throwVelocity = new Vector3(14, 6);
        if (!self.isFacingRight)
            throwVelocity.x *= -1;
        m.transform.SetParent(null);
        IGrabable grabable = m as IGrabable;
        grabable.OnThrow();
        m.HitCharacter(throwVelocity, .251f, 3f, 0);
    }
    public void Drop()
    {
        PlayerInputController.instance.HoldingObject = null;
        holdAnim.SetBool("isHolding", false);
        PlayerInputController.instance.originalPlayer.UseAttachedArm();
    }
    public override bool CanAttack()
    {
        return base.CanAttack() && SaveLoad.hasGrab;
    }
    public bool CanPullObject(Movement m)
    {
        return m.RBody.mass < self.RBody.mass;
    }

    public bool CanGrabObject(Movement m)
    {
        return m is IGrabable && (m as IGrabable).CanGrab();
    }
}
