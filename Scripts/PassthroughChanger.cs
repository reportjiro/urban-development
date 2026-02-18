using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LmaLab.UrbanPlanner
{
    public class PassthroughChanger : MonoBehaviour
    {
        private Camera _camera;

        [SerializeField] private Material sky;
        [SerializeField] private UniversalAdditionalCameraData cameraData;

        private void Start()
        {
            _camera = Camera.main;
        }

        /// <summary>
        /// パススルー機能(ARとVRの切り替えを行う)
        /// </summary>
        public void ChangePassthroughMode()
        {
            if (OVRManager.instance.isInsightPassthroughEnabled)
            {
                OVRManager.instance.isInsightPassthroughEnabled = false;
                RenderSettings.fog = true;
                _camera.clearFlags = CameraClearFlags.Skybox;
                cameraData.renderPostProcessing = true;
            }
            else
            {
                OVRManager.instance.isInsightPassthroughEnabled = true;
                RenderSettings.fog = false;
                _camera.clearFlags = CameraClearFlags.SolidColor;
                _camera.backgroundColor = Color.clear;
                cameraData.renderPostProcessing = false;
            }
        }
    }
}
