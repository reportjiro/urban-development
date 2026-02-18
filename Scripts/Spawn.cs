using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject house;
    
    // Vector3型の変数を定義
    private Vector3 spawnPosition;
    //public float X;
    //public float Y;
    //public float Z;

    void Start()
    {
        // spawnPositionを初期化
        spawnPosition = new Vector3(0.0f, 0.65f, 0.0f); // (x, y, z)
    }

    // 重要：Instantiate(生成したいObject, 表示される位置, 回転させるか否か？)
    public void SpawnHouse()
    {
        //spawnPosition = this.transform.position;
        // Instantiate(house, Vector3.zero, Quaternion.identity);
        Instantiate(house, spawnPosition, Quaternion.identity);
    }
}

