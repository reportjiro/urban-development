using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollower : MonoBehaviour
{
    [Header("State")]
    public bool followEnabled = false;

    public float CameraDistance = 1.0f;
    public float smoothTime = 0.3f;

    [SerializeField] private Camera Camera2Follow;

    private Vector3 velocity = Vector3.zero;
    private Transform target;

    private void Awake()
    {
        target = Camera2Follow.transform;
    }

    private void Update()
    { 
        if (followEnabled)
        {
            // Define my target position in front of the camera ->
            Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, CameraDistance));

            // Smoothly move my object towards that position ->
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // version 1: my object's rotation is always facing to camera with no dampening  ->
            transform.LookAt(transform.position + Camera2Follow.transform.rotation * Vector3.forward, Camera2Follow.transform.rotation * Vector3.up);

            // version 2 : my object's rotation isn't finished synchronously with the position smooth.damp ->
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, 35 * Time.deltaTime);
        }
        else
        {
            Vector3 targetPosition = transform.position;

            transform.LookAt(transform.position + Camera2Follow.transform.rotation * Vector3.forward, Camera2Follow.transform.rotation * Vector3.up);
        }
    }

    public void ShowUI() 
    {
        followEnabled = true;
    }

    public void HideUI() 
    {
        followEnabled = false;
    }
}
