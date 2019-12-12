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
    Coroutine co;
    public void Kill()
    {
        if (co != null)
            StopCoroutine(co);
        if (instance)
            instance.Die();
    }
    public void OnDieCallback()
    {
        instance = null;
        if (reSpawnOnDie)
            co = StartCoroutine(spawnInX());
    }
    IEnumerator spawnInX()
    {
        yield return new WaitForSeconds(2);
        Spawn(cacheRoom);
    }
    Room cacheRoom;
    public void Spawn(Room r)
    {
        cacheRoom = r;
        instance = GameObject.Instantiate(prefab, spawnPoint.position, Quaternion.identity).GetComponent<Movement>();
        instance.Spawn = this;
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
