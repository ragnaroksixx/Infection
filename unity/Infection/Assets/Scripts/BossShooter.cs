using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class BossShooter : MonoBehaviour
{
    public Projectile prefab;
    public Transform shotSource;
    public float spinSpeed, shootSpeed;
    public float timeBtwShoots;
    float timeTrack;
    private void Awake()
    {
        Stop();
    }
    public void StartShooting()
    {
        shotSource.parent.localEulerAngles = Vector3.zero;
        enabled = true;
    }
    public void Stop()
    {
        enabled = false;
    }
    private void Update()
    {
        Vector3 rot = shotSource.parent.localEulerAngles;
        rot.z += spinSpeed * Time.deltaTime;
        if (rot.z > 360)
            rot.z -= 360;
        shotSource.parent.localEulerAngles = rot;
        timeTrack -= Time.deltaTime;
        if (timeTrack <= 0)
        {
            Shoot();
        }
    }
    public void Shoot()
    {
        timeTrack = timeBtwShoots;
        Projectile p = GameObject.Instantiate(prefab, shotSource.position, shotSource.rotation);
        p.Init(5);
        p.RBody.AddForce(shotSource.right * shootSpeed);
        p.RBody.AddRelativeTorque(p.transform.up * spinSpeed);
    }
}

