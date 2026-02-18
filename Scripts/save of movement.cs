using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Save : MonoBehaviour
{
    private string saveFilePath;

    void Start()
    {
        // 保存ファイルのパスを設定
        saveFilePath = Application.persistentDataPath + "/transform_save.txt";
        Debug.Log("Save file path: " + saveFilePath);
    }

    void Update()
    {
        // キー入力で保存と読み込みをテスト
        if (Input.GetKeyDown(KeyCode.S)) // "S"キーで保存
        {
            SaveTransformData();
        }
        
        if (Input.GetKeyDown(KeyCode.L)) // "L"キーで読み込み
        {
            LoadTransformData();
        }
    }

    // Transformの状態を保存する
    public void SaveTransformData()
    {
        // 保存したいTransformを指定（このスクリプトがアタッチされているオブジェクトのTransform）
        Transform targetTransform = transform;

        // 保存するデータを作成
        string saveData = targetTransform.CreateSaveString(
            isLocal: true,   // ローカル座標系で保存
            isSavePosition: true,
            isSaveRotation: true,
            isSaveScale: true
        );

        // 保存データをファイルに書き込む
        File.WriteAllText(saveFilePath, saveData);
        Debug.Log("Transform data saved.");
    }

    // Transformの状態を読み込む
    public void LoadTransformData()
    {
        if (File.Exists(saveFilePath))
        {
            // 保存されているデータを読み込む
            string saveData = File.ReadAllText(saveFilePath);

            // 読み込んだデータをTransformに適用
            transform.SetupFromSaveString(
                saveData,
                isLocal: true,   // ローカル座標系で読み込む
                isLoadPosition: true,
                isLoadRotation: true,
                isLoadScale: true
            );
            Debug.Log("Transform data loaded.");
        }
        else
        {
            Debug.LogWarning("No saved transform data found.");
        }
    }
}

public static class TransformExtension
{
    // 区切りの文字列
    const string _SAVE_SEPARATOR = "_";

    // Transformのデータを保存する文字列を作成
    public static string CreateSaveString(this Transform transform, bool isLocal, bool isSavePosition, bool isSaveRotation, bool isSaveScale)
    {
        if (isLocal)
        {
            return CreateLocalSaveString(transform, isSavePosition, isSaveRotation, isSaveScale); 
        }

        return CreateWorldSaveString(transform, isSavePosition, isSaveRotation, isSaveScale);
    }

    private static string CreateLocalSaveString(Transform transform, bool isSavePosition, bool isSaveRotation, bool isSaveScale)
    {
        string data = "";

        if (isSavePosition)
        {
            Vector3 pos = transform.localPosition;
            data += pos.x.ToString() + _SAVE_SEPARATOR;
            data += pos.y.ToString() + _SAVE_SEPARATOR;
            data += pos.z.ToString() + _SAVE_SEPARATOR;
        }

        if (isSaveRotation)
        {
            Vector3 rot = transform.localEulerAngles;
            data += rot.x.ToString() + _SAVE_SEPARATOR;
            data += rot.y.ToString() + _SAVE_SEPARATOR;
            data += rot.z.ToString() + _SAVE_SEPARATOR;
        }

        if (isSaveScale)
        {
            Vector3 scale = transform.localScale;
            data += scale.x.ToString() + _SAVE_SEPARATOR;
            data += scale.y.ToString() + _SAVE_SEPARATOR;
            data += scale.z.ToString() + _SAVE_SEPARATOR;
        }

        return data;
    }

    private static string CreateWorldSaveString(Transform transform, bool isSavePosition, bool isSaveRotation, bool isSaveScale)
    {
        string data = "";

        if (isSavePosition)
        {
            Vector3 pos = transform.position;
            data += pos.x.ToString() + _SAVE_SEPARATOR;
            data += pos.y.ToString() + _SAVE_SEPARATOR;
            data += pos.z.ToString() + _SAVE_SEPARATOR;
        }

        if (isSaveRotation)
        {
            Vector3 rot = transform.eulerAngles;
            data += rot.x.ToString() + _SAVE_SEPARATOR;
            data += rot.y.ToString() + _SAVE_SEPARATOR;
            data += rot.z.ToString() + _SAVE_SEPARATOR;
        }

        if (isSaveScale)
        {
            Vector3 scale = transform.localScale;
            data += scale.x.ToString() + _SAVE_SEPARATOR;
            data += scale.y.ToString() + _SAVE_SEPARATOR;
            data += scale.z.ToString() + _SAVE_SEPARATOR;
        }

        return data;
    }

    // 保存データを元にTransformを設定
    public static void SetupFromSaveString(this Transform transform, string data, bool isLocal, 
                                           bool isLoadPosition, bool isLoadRotation, bool isLoadScale)
    {
        if (isLocal)
        {
            SetupLocalFromString(transform, data, isLoadPosition, isLoadRotation, isLoadScale);
            return;
        }

        SetupWorldFromString(transform, data, isLoadPosition, isLoadRotation, isLoadScale);
    }

    private static void SetupLocalFromString(Transform transform, string data, bool isLoadPosition, bool isLoadRotation, bool isLoadScale)
    {
        string[] dataList = data.Split(_SAVE_SEPARATOR[0]);
        int index = 0;

        if (isLoadPosition)
        {
            transform.localPosition = new Vector3(float.Parse(dataList[index++]),
                                                  float.Parse(dataList[index++]),
                                                  float.Parse(dataList[index++])
                                                  );
        }

        if (isLoadRotation)
        {
            transform.localEulerAngles = new Vector3(float.Parse(dataList[index++]),
                                                    float.Parse(dataList[index++]),
                                                    float.Parse(dataList[index++])
                                                    );
        }

        if (isLoadScale)
        {
            transform.localScale = new Vector3(float.Parse(dataList[index++]),
                                               float.Parse(dataList[index++]),
                                               float.Parse(dataList[index++])
                                               );
        }
    }

    private static void SetupWorldFromString(Transform transform, string data, bool isLoadPosition, bool isLoadRotation, bool isLoadScale)
    {
        string[] dataList = data.Split(_SAVE_SEPARATOR[0]);
        int index = 0;

        if (isLoadPosition)
        {
            transform.position = new Vector3(float.Parse(dataList[index++]),
                                             float.Parse(dataList[index++]),
                                             float.Parse(dataList[index++])
                                             );
        }

        if (isLoadRotation)
        {
            transform.eulerAngles = new Vector3(float.Parse(dataList[index++]),
                                                float.Parse(dataList[index++]),
                                                float.Parse(dataList[index++])
                                                );
        }

        if (isLoadScale)
        {
            transform.localScale = new Vector3(float.Parse(dataList[index++]),
                                               float.Parse(dataList[index++]),
                                               float.Parse(dataList[index++])
                                               );
        }
    }

    // Transformのローカル座標をリセット
    public static void ResetLocal(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    // Transformのワールド座標をリセット
    public static void ResetWorld(this Transform transform)
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}

