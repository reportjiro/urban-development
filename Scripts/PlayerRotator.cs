using Oculus.Interaction.Input;
using UnityEngine;

namespace LmaLab.UrbanPlanner
{
    /// <summary>
    /// プレイヤーを回転させるクラス
    /// </summary>
    public class PlayerRotator : MonoBehaviour
    {
        private const float RotateAngle = 1f;           // 1フレームのあたりの回転量
        private const float ThumbAngleOffset = 90f;     // 上方向のオフセット角度
        private const float LeftThreshold = 20f;        // 左側へ傾けた角度の閾値
        private const float RightThreshold = -20f;      // 右側へ傾けた角度の閾値
        
        private bool _rotateMode;
        private IHand _hand;
        private Transform _hmdCamera;
        
        [SerializeField] private OVRCameraRig rig;
        
        private void Start()
        {
            _hand = GetComponentInParent<HandRef>().Hand;
            if (Camera.main != null) _hmdCamera = Camera.main.transform;
        }
        
        private void Update()
        {
            // 回転モードがオフの場合，何もしない．
            if (!_rotateMode) return;
            
            // 親指の先端と付け根それぞれの Pose を取得する．それぞれ取得できない場合，何もしない．
            if (!_hand.GetJointPose(HandJointId.HandThumbTip, out Pose thumbTip) ||
                !_hand.GetJointPose(HandJointId.HandThumb2, out Pose thumbRoot)) return;

            // 指の付け根から指先への方向ベクトルを求める
            Vector3 direction = thumbTip.position - thumbRoot.position;

            // HMDのヨー回転（左右の回転）を取得
            Quaternion hmdYawRotation = Quaternion.Euler(0, _hmdCamera.eulerAngles.y, 0);
            
            // direction をHMDのヨー回転のみ考慮したローカル座標系に変換
            Vector3 localDirection = Quaternion.Inverse(hmdYawRotation) * direction;

            // localDirection のz軸の回転を求める
            float thumbAngleZ = Mathf.Atan2(localDirection.y, localDirection.x) * Mathf.Rad2Deg;
            
            // 上方向が0度となるように調整
            thumbAngleZ -= ThumbAngleOffset;
            
            switch (thumbAngleZ)
            {
                case > LeftThreshold: // 左に傾けた角度が閾値(LeftThreshold)以上の場合
                    //左に回転させる
                    rig.transform.RotateAround(_hmdCamera.position, Vector3.up, -RotateAngle);
                    break;
                case < RightThreshold: // 右に傾けた角度が閾値(RightThreshold)以上の場合
                    //右に回転させる
                    rig.transform.RotateAround(_hmdCamera.position, Vector3.up, RotateAngle);
                    break;
            }
        }

        /// <summary>
        /// 回転モード(_rotateMode)を切り替える．
        /// </summary>
        public void RotatePlayer()
        {
            _rotateMode = !_rotateMode;
        }
    }
}
