using UnityEngine;
using System.Collections.Generic;

//オブジェクト最適化script
//hideDistanceより遠くのオブジェクトのRenderer(描画)を無効化している
public class OptimizationManager : MonoBehaviour
{
    public Transform vrCameraTransform;
    public float checkInterval = 1.0f;  //待つ時間
    public float hideDistance = 10f;    //カメラからの距離

    private List<Renderer> renderers = new List<Renderer>();

    void Start()
    {
        if (vrCameraTransform == null)
        {
            //VRカメラ(CameraRig)を探す
            var ovrRig = FindObjectOfType<OVRCameraRig>();
            //CameraRigが見つかなかったらMainCameraにする
            if (ovrRig != null)
                vrCameraTransform = ovrRig.centerEyeAnchor;
            else
                vrCameraTransform = Camera.main.transform;
        }

        //対象のタグを持つオブジェクト名をリスト化
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Optimizable");
        foreach (var obj in targets)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
                renderers.Add(renderer);

            InvokeRepeating(nameof(UpdateVisibility), 0, checkInterval);
        }
    }

    void UpdateVisibility()
    {
        foreach (var renderer in renderers)
        {
            if (renderer == null) continue;

            float dist = Vector3.Distance(renderer.transform.position, vrCameraTransform.position);
            renderer.enabled = dist < hideDistance;
        }
    }
}
