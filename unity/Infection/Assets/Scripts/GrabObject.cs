using UnityEngine;
using System.Collections;

public class GrabObject : Movement, IGrabable
{
    public Collider[] cols;
    public void OnGrab()
    {
        foreach (Collider col in cols)
            col.enabled = false;
    }

    public void OnThrow()
    {
        foreach (Collider col in cols)
            col.enabled = true;
    }

    public override void Die(bool ignoreSpawn)
    {
        if (PlayerInputController.instance.HoldingObject == this)
            PlayerMovement.instance.clawAttack.Drop();
        base.Die(ignoreSpawn);
    }
    public bool CanGrab()
    {
        return true;
    }
}
