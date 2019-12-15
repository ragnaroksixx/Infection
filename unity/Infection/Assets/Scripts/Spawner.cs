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
    public virtual void Kill()
    {
        if (co != null)
            StopCoroutine(co);
        if (instance)
            instance.Die(true);
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
    public virtual GameObject Spawn(Room r)
    {
        if (instance)
            instance.Die(true);
        cacheRoom = r;
        GameObject obj;
        obj = GameObject.Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        instance = obj.GetComponent<Movement>();
        if (instance)
            instance.Spawn = this;
        EnemyMovement em = obj.GetComponent<EnemyMovement>();
        if (em)
            em.room = r;

        Key key = obj.GetComponentInChildren<Key>();
        if (key)
        {
            key.type = keyType;
            key.Init();
        }
        return obj;

    }
}
