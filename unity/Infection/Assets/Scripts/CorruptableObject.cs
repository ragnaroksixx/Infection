
public class CorruptableObject : Movement, ICorruptable
{
    public override void Die()
    {
        if (PlayerInputController.instance.CorruptingEnemy==this)
        {
            PlayerInputController.instance.ReleasCorruption();
        }
        base.Die();

    }
}

