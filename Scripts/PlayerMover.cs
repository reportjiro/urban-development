using Oculus.Interaction.Input;
using UnityEngine;

namespace LmaLab.UrbanPlanner
{
    /// <summary>
    /// プレイヤーを移動させるクラス
    /// </summary>
    public class PlayerMover : MonoBehaviour
    {
        private bool _isMove;
        private IHand _hand;

        [SerializeField] private OVRCameraRig rig;

        private void Start()
        {
            _hand = GetComponentInParent<HandRef>().Hand;
        }

        private void Update()
        {
            // 移動モードがオフの場合，何もしない．
            if (!_isMove) return;
            
            // 人差し指の先端と付け根それぞれの Pose を取得する．それぞれ取得できない場合，何もしない．
            if (!_hand.GetJointPose(HandJointId.HandIndexTip, out Pose indexTip) ||
                !_hand.GetJointPose(HandJointId.HandIndex1, out Pose indexRoot)) return;

            // 指の付け根から指先への方向ベクトルを,y 成分を無視して正規化したベクトルを求める
            Vector3 direction = new Vector3(indexTip.position.x - indexRoot.position.x, 0,
                indexTip.position.z - indexRoot.position.z).normalized;

            // 求めたベクトル方向へ移動させる
            rig.transform.position += direction * Time.deltaTime;
        }

        public void MovePlayer()
        {
            _isMove = !_isMove;
        }
    }
}