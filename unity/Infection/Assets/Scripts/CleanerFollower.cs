using UnityEngine;
using System.Collections;

public class CleanerFollower : MonoBehaviour
{
    Cleaner c;
    int delay;
    Rigidbody rBody;
    // Update is called once per frame
    private void Start()
    {
        c = GetComponentInParent<Cleaner>();
        rBody = GetComponent<Rigidbody>();
        delay = transform.GetSiblingIndex() * 20;
    }
    void Update()
    {

        Vector3 nextHeadPos = c.posBuffer[delay].position;
        nextHeadPos.z = 0;
        transform.right = -transform.position + nextHeadPos;
        rBody.MovePosition(nextHeadPos);
    }

}
