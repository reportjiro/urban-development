using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingID : MonoBehaviour
{
    [SerializeField] public int uniqueID;

    //:‚ğ‰Ÿ‚µ‚Ä"Rebuild All Building IDs In Scene‚ğ‰Ÿ‚·‚ÆÀs‚³‚ê‚é
#if UNITY_EDITOR
    [ContextMenu("Rebuild All Building IDs In Scene")]
    private void RebuildAllIDs()
    {
        var all = FindObjectsOfType<BuildingID>(true);

        for (int i = 0; i < all.Length; i++)
        {
            all[i].uniqueID = i;
            UnityEditor.EditorUtility.SetDirty(all[i]);
        }

        Debug.Log($"BuildingID Ä¶¬Š®—¹F{all.Length} ŒÂ");
    }
#endif
}
