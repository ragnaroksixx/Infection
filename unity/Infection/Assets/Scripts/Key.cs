using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour
{
    GrabObject self;

    public KeyType type;

    public void Init()
    {
        self = GetComponentInParent<GrabObject>();
        if (type == KeyType.NONE)
            Debug.LogError("Set key type!!!", this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Door d = other.GetComponentInParent<Door>();
        if (d && d.isLocked && d.keyType == type)
        {
            d.Unlock();
            d.Open();
            self.Die();
        }
    }
}
public enum KeyType
{
    NONE = 0,
    RED,
    BLUE,
    GREEN
}
