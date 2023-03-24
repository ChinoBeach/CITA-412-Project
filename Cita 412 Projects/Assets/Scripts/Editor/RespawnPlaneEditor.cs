using UnityEngine;
using UnityEditor;

///<summary>
/// 
///</summary>
[CustomEditor(typeof(RespawnPlane))]
public class RespawnPlaneEditor : Editor
{

    private void OnEnable()
    {
        EditorUtility.SetDirty(target);
    
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RespawnPlane targetScript = (RespawnPlane)target;

        if(GUILayout.Button("Add New Respawn Point"))
        {
            var point = targetScript.AddNewRespawnPoint();
            Selection.activeObject = point;
        }

        if (GUILayout.Button("Delete Newest Respawn Point"))
        {
            targetScript.RemoveNewestRespawnPoint();
        }

        if (GUILayout.Button("Clear Respawn Points"))
        {
            targetScript.ClearAllRespawnPoints();
        }
    }

}