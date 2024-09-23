using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class LookMe : MonoBehaviour
{
    [Header("Speed Camera")]
    public Vector3 speedCam;

    [Header("Move Down From Transform")]
    public float posy;

    public static LookMe instance;
    private Vector3 originalDamping;
    private Vector3 originaloffset;
    public Transform playerTransform;
    CinemachineFollow camFollow;
    CinemachineCamera cam;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void MoveTo(Transform pos) 
    { 
        cam = GameObject.FindAnyObjectByType<CinemachineCamera>();
        Vector3 VecPos = new Vector3 (pos.position.x, pos.position.y - posy, pos.position.z);
        pos.position = VecPos;
        cam.Target.TrackingTarget = pos;
        camFollow = cam.GetComponent<CinemachineFollow>();
        if (camFollow != null)
        {
            originalDamping = camFollow.TrackerSettings.PositionDamping;
            originaloffset = camFollow.FollowOffset;
            camFollow.TrackerSettings.PositionDamping = speedCam;
            camFollow.FollowOffset = new Vector3(0, 5, -10);
        }
    }
    public void BackToPlayer()
    {
        cam.Target.TrackingTarget = GameObject.FindGameObjectWithTag("Player").transform;
        camFollow.TrackerSettings.PositionDamping = originalDamping;
        camFollow.FollowOffset = originaloffset;
    }
}
