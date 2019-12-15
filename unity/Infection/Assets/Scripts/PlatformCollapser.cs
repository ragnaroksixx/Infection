using UnityEngine;
using System.Collections;

public class PlatformCollapser : MonoBehaviour
{
    public Transform[] platforms;


    public void Drop()
    {
        foreach (Transform item in platforms)
        {
            Rigidbody rb = item.gameObject.AddComponent<Rigidbody>();
            rb.AddForce(Vector3.up * 5);
            rb.AddTorque(Random.onUnitSphere * 3);
        }
        platforms = new Transform[0];
    }
    private void OnTriggerEnter(Collider other)
    {
        Drop();
    }
}
