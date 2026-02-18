using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//オブジェクトの非表示マネージャー
public class ObjectVisibilityManager : MonoBehaviour
{
    public float viewDistance = 10f;         //カメラとの距離
    public float viewAngle = 90f;            //カメラの角度
    public string targetTag = "Optimizable"; // 適用対象のタグ
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in objects)
        {
            Vector3 directionToObject = obj.transform.position - mainCamera.transform.position;
            float angle = Vector3.Angle(mainCamera.transform.forward, directionToObject);
            float distance = directionToObject.magnitude;

            //visibleが指定のカメラアングルや距離外であればfalseにする
            bool visible = angle < viewAngle && distance < viewDistance;

            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null) rend.enabled = visible;
        }
    }
}

