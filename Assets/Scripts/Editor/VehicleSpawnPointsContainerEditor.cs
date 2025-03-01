using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(VehicleSpawnPointsContainer))]
public class VehicleSpawnPointsContainerEditor : Editor
{
	VehicleSpawnPointsContainer spawnPointScript;
	
	public override void OnInspectorGUI()
	{

		spawnPointScript = (VehicleSpawnPointsContainer)target;
		serializedObject.Update();

		EditorGUILayout.HelpBox("Create SpawnPoints By Shift + Left Mouse Button On Your Road", MessageType.Info);

		EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnPoints"), new GUIContent("SpawnPoints", "SpawnPoints"), true);


		if (GUILayout.Button("Delete SpawnPoints")) 
		{

			foreach (var t in spawnPointScript.spawnPoints)
			{
				DestroyImmediate(t.gameObject);
			}

			spawnPointScript.spawnPoints.Clear();
			EditorUtility.SetDirty(spawnPointScript);
		}

		serializedObject.ApplyModifiedProperties();

		if (GUI.changed)
			EditorUtility.SetDirty(spawnPointScript);
	}

	private void OnSceneGUI() 
	{
		Event e = Event.current;
		spawnPointScript = (VehicleSpawnPointsContainer)target;

		if (e != null) 
		{
			if (e.isMouse && e.shift && e.type == EventType.MouseDown) 
			{
				Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				RaycastHit hit = new RaycastHit();

				int controlId = GUIUtility.GetControlID(FocusType.Passive);

				// Tell the UI your event is the main one to use, it override the selection in  the scene view
				GUIUtility.hotControl = controlId;
				// Don't forget to use the event
				Event.current.Use();

				if (Physics.Raycast(ray, out hit, 5000.0f))
				{
					Vector3 newTilePosition = hit.point;

					GameObject spawnPoint = new GameObject("SpawnPoint " + spawnPointScript.spawnPoints.Count.ToString());
					spawnPoint.AddComponent<CCDS_SpawnPoint>();
					spawnPoint.transform.position = newTilePosition;
					spawnPoint.transform.SetParent(spawnPointScript.transform);

					spawnPointScript.GetAllSpawnPoints();

					EditorUtility.SetDirty(spawnPointScript);
				}

			}

		}

		spawnPointScript.GetAllSpawnPoints();

	}

}
