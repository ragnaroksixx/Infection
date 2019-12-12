using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool<T> where T : Behaviour
{
    protected List<T> inactiveItems, activeItems;
    GameObject blueprint;
    Transform poolLocation;
    int size;
    public Pool(GameObject b, int initialSize)
    {
        inactiveItems = new List<T>();
        activeItems = new List<T>();
        blueprint = b;
        GameObject g = GameObject.Find("Pool");
        if (g == null)
        {
            g = new GameObject("Pool");
        }
        poolLocation = g.transform.Find(b.name + " Pool");
        if (poolLocation == null)
        {
            GameObject parent = new GameObject(b.name + " Pool");
            parent.transform.SetParent(g.transform);
            poolLocation = parent.transform;
        }
        size = initialSize;
        Populate(size);
    }
    private void Populate(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject g = GameObject.Instantiate(blueprint, poolLocation);
            Init(g.GetComponent<T>());
            Push(g.GetComponent<T>());
        }
    }
    public T Pop()
    {
        if (inactiveItems.Count == 0)
            Populate(size);
        T obj = inactiveItems[0];
        inactiveItems.RemoveAt(0);
        activeItems.Add(obj);
        OnPop(obj);
        return obj;
    }

    public void Push(T obj)
    {
        inactiveItems.Add(obj);
        activeItems.Remove(obj);
        OnReturn(obj);
    }

    protected virtual void OnPop(T obj)
    {
        obj.gameObject.SetActive(true);
    }

    protected virtual void OnReturn(T obj)
    {
        obj.gameObject.SetActive(false);
    }
    protected virtual void Init(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.localEulerAngles = Vector3.zero;
        obj.transform.localPosition = Vector3.zero;
    }
}
