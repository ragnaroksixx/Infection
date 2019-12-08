using UnityEngine;
using System.Collections;

public class CameraTarget : MonoBehaviour
{
    public static CameraTarget Instance;
    Transform defaultTarget;
    private void Awake()
    {
        Instance = this;
        defaultTarget = transform.parent;
    }
    public static void SetCameraTarget(Transform t = null)
    {
        if (t == null)
            t = Instance.defaultTarget;
        Instance.transform.SetParent(t);
        Instance.transform.localPosition = Vector3.zero;
    }
}
