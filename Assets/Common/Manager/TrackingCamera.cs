using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TrackingCamera : Singleton<TrackingCamera>
{
    private CinemachineVirtualCamera tracking_cam;
    //追従対象
    public GameObject Target;

    void Awake()
    {
        tracking_cam = this.gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        //追従更新
        if (Target == null) return;
        tracking_cam.Follow = Target.transform;
    }
}
