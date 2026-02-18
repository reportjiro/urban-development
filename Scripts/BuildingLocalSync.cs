using System.Collections;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

//各オブジェクトにアタッチする同期用スクリプト
[RequireComponent(typeof(HandGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BuildingID))]
public class BuildingLocalSync : MonoBehaviour
{
    private HandGrabInteractable _handGrab;
    private Rigidbody _rb;
    private BuildingSyncManager _syncManager;
    private BuildingID _buildingID;

    private void Awake()
    {
        _handGrab = GetComponent<HandGrabInteractable>();
        _rb = GetComponent<Rigidbody>();
        _buildingID = GetComponent<BuildingID>();
        _syncManager = FindObjectOfType<BuildingSyncManager>();
    }

    private void OnEnable()
    {
        _handGrab.WhenStateChanged += OnHandGrabStateChanged;
    }

    private void OnDisable()
    {
        _handGrab.WhenStateChanged -= OnHandGrabStateChanged;
    }

    private void OnHandGrabStateChanged(InteractableStateChangeArgs args)
    {
        if (args.NewState == InteractableState.Select)
        {
            _rb.isKinematic = true;  // 掴んだら物理停止
        }
        else if (args.PreviousState == InteractableState.Select)
        {
            _rb.isKinematic = false; // 離したら物理復活
            _syncManager?.SendTransform(this); // 同期要求
        }
    }

    public int GetID()
    {
        return _buildingID.uniqueID;
    }
}
