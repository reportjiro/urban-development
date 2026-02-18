using Fusion;
using UnityEngine;
using System.Collections.Generic;

public struct BuildingState : INetworkStruct
{
    public int buildingId;
    public Vector3 pos;
    public Quaternion rot;
}

public class BuildingSyncManager : NetworkBehaviour
{
    private Dictionary<int, BuildingLocalSync> buildingDict = new Dictionary<int, BuildingLocalSync>();

    //NetworkObjectが生成された後に実行
    public override void Spawned()
    {
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        buildingDict.Clear();

        var allBuildings = FindObjectsOfType<BuildingLocalSync>();

        foreach (var building in allBuildings)
        {
            int id = building.GetID();

            if (!buildingDict.ContainsKey(id))
            {
                buildingDict.Add(id, building);
            }
            else
            {
                Debug.LogWarning($"Duplicate Building ID: {id}");
            }
        }

        Debug.Log($"Building dictionary built. Count={buildingDict.Count}");
    }

    //Cliant→HostへRPCを送信
    public void SendTransform(BuildingLocalSync building)
    {
        //Networkが初期化されていないときに呼ばないようにする
        if (Object == null) return;
        if (Object.Runner == null) return;

        var state = new BuildingState
        {
            buildingId = building.GetID(),
            pos = building.transform.position,
            rot = building.transform.rotation
        };

        //RpcSources.Allで、クライアントからでも送れるようにする。
        //最終的な決定はStateAuthority（ホスト）で行われる。
        RPC_RequestBuilding(state);
    }

    //Cliant→Hostに送信
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestBuilding(BuildingState state, RpcInfo info = default)
    {
        Debug.Log($"RPC received. DictCount={buildingDict.Count}, id={state.buildingId}");

        //ホストかどうか確認
        if (!Object.HasStateAuthority) return;
        //建物IDを入力している(見つからない場合はreturn)
        int id = state.buildingId;

        if (!buildingDict.TryGetValue(state.buildingId, out var building))
        {
            Debug.LogError($"Building ID not found: {state.buildingId}");
            return;
        }

        //建物の移動
        building.transform.SetPositionAndRotation(state.pos, state.rot);
        Debug.Log("Move:OK");

        // ホストから全員へ確定を配布
        RPC_BroadcastBuilding(state);
    }

    //ホストが全員へ送信(実行はCliant)
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_BroadcastBuilding(BuildingState state, RpcInfo info = default)
    {
        //ホストが動かしたものをクライアントに反映する
        if (!buildingDict.TryGetValue(state.buildingId, out var building)) return;
        building.transform.SetPositionAndRotation(state.pos, state.rot);
    }
}
