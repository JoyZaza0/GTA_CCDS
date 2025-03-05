using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Clear_CCDS_Save))]
public class Clear_CCDS_SaveEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		Clear_CCDS_Save clearSave = (Clear_CCDS_Save)target;
		
		if(GUILayout.Button("Delete"))
		{
			clearSave.Clear();
		}
	}
}
