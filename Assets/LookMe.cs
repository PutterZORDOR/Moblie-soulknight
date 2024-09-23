using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class LookMe : MonoBehaviour
{
    [Header("Speed Camera")]
    public Vector3 speedCam;
    private void Start()
    {
        CinemachineCamera cam = GameObject.FindAnyObjectByType<CinemachineCamera>();
        cam.Target.TrackingTarget = transform;
        CinemachineFollow camFollow = cam.GetComponent<CinemachineFollow>();
        if (camFollow != null)
        {
            camFollow.TrackerSettings.PositionDamping = speedCam;

            camFollow.FollowOffset = new Vector3(0, 5, -10);
        }
    }
}
