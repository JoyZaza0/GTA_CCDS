using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RemoveParent))]
public class RemoveParentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RemoveParent removeParent = (RemoveParent)target;

        if (GUILayout.Button("Remove"))
        {
            removeParent.Remove();
        }
    }
}
