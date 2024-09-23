using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LookMe : MonoBehaviour
{
    [Header("Speed Camera")]
    public Vector3 speedCam;

    [Header("Move Down From Transform")]
    public float posy;

    public static LookMe instance;
    private Vector3 originalDamping;
    private Vector3 originaloffset;
    private Vector3 originalPos;
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

        originalPos = pos.position;  // เก็บตำแหน่งเดิมของ pos
        Vector3 newCamPos = new Vector3(pos.position.x, pos.position.y - posy, pos.position.z); // ตำแหน่งกล้องใหม่

        cam.Target.TrackingTarget = pos;  // ตั้งค่าให้กล้องติดตาม Transform pos
        camFollow = cam.GetComponent<CinemachineFollow>();
        if (camFollow != null)
        {
            originalDamping = camFollow.TrackerSettings.PositionDamping;
            originaloffset = camFollow.FollowOffset;

            camFollow.TrackerSettings.PositionDamping = speedCam;

            // ปรับ FollowOffset โดยให้แกน Y ลดลงตามค่า posy
            camFollow.FollowOffset = new Vector3(originaloffset.x, originaloffset.y - posy, originaloffset.z);
        }
    }

    public void BackToPlayer()
    {
        cam.Target.TrackingTarget = GameObject.FindGameObjectWithTag("Player").transform;
        camFollow.TrackerSettings.PositionDamping = originalDamping;
        camFollow.FollowOffset = originaloffset;  // คืนค่ากลับสู่ตำแหน่ง offset เดิม
    }
}
