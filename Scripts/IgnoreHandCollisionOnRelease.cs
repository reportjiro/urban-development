using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

//一時的に手の衝突を無視するスクリプト
public class IgnoreHandCollisionOnRelease : MonoBehaviour
{
    private Grabbable grabbable;
    private Collider[] handColliders;
    private float ignoreDuration = 0.3f; // 離してから衝突を無効にする時間
    private bool isIgnoring = false;

    private void Awake()
    {
        grabbable = GetComponent<Grabbable>();
    }

    private void Start()
    {
        // 手のColliderを自動検出（左右どちらも）
        var hands = FindObjectsOfType<HandGrabInteractor>();
        var colliders = new List<Collider>();
        foreach (var hand in hands)
        {
            foreach (var c in hand.GetComponentsInChildren<Collider>())
                colliders.Add(c);

            // 掴み状態が変化した時のイベントを登録
            hand.WhenStateChanged += args =>
            {
                // Select → Normal になった（＝掴み終わり）
                if (args.NewState == InteractorState.Normal && args.PreviousState == InteractorState.Select)
                {
                    if (!isIgnoring)
                        StartCoroutine(IgnoreCollisionTemporarily());
                }
            };
        }
        handColliders = colliders.ToArray();
    }

    private void OnReleased()
    {
        // 掴んでいたものを離した瞬間に呼ばれる
        if (grabbable != null && !isIgnoring)
            StartCoroutine(IgnoreCollisionTemporarily());
    }

    private IEnumerator IgnoreCollisionTemporarily()
    {
        isIgnoring = true;
        foreach (var c in handColliders)
        {
            foreach (var myCol in GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(myCol, c, true);
        }

        yield return new WaitForSeconds(ignoreDuration);

        foreach (var c in handColliders)
        {
            foreach (var myCol in GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(myCol, c, false);
        }
        isIgnoring = false;
    }
}
