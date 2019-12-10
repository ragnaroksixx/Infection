using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    Movement instance;
    public Transform spawnPoint;
    public bool killOnPlayerStart;

    public KeyType keyType;
    public bool reSpawnOnDie;

    public void Kill()
    {
        if (instance)
            instance.Die();
    }

    public void Spawn(Room r)
    {
        instance = GameObject.Instantiate(prefab, spawnPoint.position, Quaternion.identity).GetComponent<Movement>();
        EnemyMovement em = instance.GetComponent<EnemyMovement>();
        if (em)
            em.room = r;

        Key key = instance.GetComponentInChildren<Key>();
        if (key)
        {
            key.type = keyType;
            key.Init();
        }

    }
}
