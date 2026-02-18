using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIを使用

public class CubeSpawner : MonoBehaviour
{
    // 出現させる立方体のプレハブ
    public GameObject cubePrefab; 
    
    // 立方体を出現させる位置
    // 現状は、原点座標表示のまま…
    // public Vector3 spawnPosition = Vector3.zero; 
    // public Vector3 spawnPosition = new Vector3( -1.0f, 0.0f, 0.0f); 
    public Vector3 spawnPosition;

    // ボタンの参照 
    public Button spawnButton; 

    private void Start()
    {
        // ボタンがクリックされた時にSpawnCubeAtPositionを呼び出すリスナーを追加
        spawnButton.onClick.AddListener(SpawnCubeAtPosition);
    }

    // ボタンがクリックされた時に立方体を出現させる関数
    void SpawnCubeAtPosition()
    {
        // cubePrefabをspawnPositionの位置に出現させる
        Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
    }
}
