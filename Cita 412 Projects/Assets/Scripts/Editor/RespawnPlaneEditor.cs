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
            targetScript.AddNewRespawnPoint();
        }
    }

}