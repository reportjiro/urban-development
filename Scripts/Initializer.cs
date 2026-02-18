using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LmaLab.UrbanPlanner
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private UniversalAdditionalCameraData cameraData;

        private void Awake()
        {
            OVRManager.instance.isInsightPassthroughEnabled = false;
            RenderSettings.fog = true;
            if (Camera.main != null) Camera.main.clearFlags = CameraClearFlags.Skybox;
            cameraData.renderPostProcessing = true;
        }
    }
}
