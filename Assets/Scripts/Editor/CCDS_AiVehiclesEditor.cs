using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CCDS_AiVehicles))]
public class CCDS_AiVehiclesEditor : Editor
{
	
	CCDS_AiVehicles prop;
	static bool autoUpdate = true;
	GUISkin skin;

	private void OnEnable() {

		skin = Resources.Load<GUISkin>("CCDS_Gui");

	}

	public override void OnInspectorGUI() {

		prop = (CCDS_AiVehicles)target;
		serializedObject.Update();
		GUI.skin = skin;

		autoUpdate = EditorGUILayout.ToggleLeft("Auto Update Prefabs", autoUpdate);
		EditorGUILayout.Separator();

		if (!autoUpdate && GUILayout.Button("Update"))
			UpdateRCCPVehicles();

		EditorGUI.indentLevel++;
		DrawDefaultInspector();
		EditorGUI.indentLevel--;

		//for (int i = 0; i < prop.playerVehicles.Length; i++) {

		//	if (prop.playerVehicles[i] != null && prop.playerVehicles[i].vehicle != null) {

		//		if (!prop.playerVehicles[i].vehicle.gameObject.TryGetComponent(out CCDS_Player playerComponent)) {

		//			prop.playerVehicles[i].vehicle.gameObject.AddComponent<CCDS_Player>();
		//			EditorUtility.SetDirty(prop);

		//		}

		//	}

		//}

		if (GUI.changed) {

			EditorUtility.SetDirty(prop);

			if (autoUpdate)
				UpdateRCCPVehicles();

		}

		serializedObject.ApplyModifiedProperties();

	}

	/// <summary>
	/// Updates all RCCP vehicles with given settings in the CCDS_PlayerVehicles (Tools --> BCG --> CCDS --> Player Vehicles).
	/// </summary>
	private void UpdateRCCPVehicles() {

		for (int i = 0; i < CCDS_AiVehicles.Instance.playerVehicles.Length; i++) {

			CCDS_AiVehicles.PlayerVehicle vehicle = CCDS_AiVehicles.Instance.playerVehicles[i];

			if (vehicle.vehicle != null) {

				RCCP_Engine engine = vehicle.vehicle.GetComponentInChildren<RCCP_Engine>(true);
				RCCP_Stability stability = vehicle.vehicle.GetComponentInChildren<RCCP_Stability>(true);
				RCCP_Differential differential = vehicle.vehicle.GetComponentInChildren<RCCP_Differential>(true);

				if (engine)
					engine.maximumTorqueAsNM = vehicle.engineTorque;

				if (stability) {

					stability.tractionHelperStrength = vehicle.handling * .5f;
					stability.steerHelperStrength = vehicle.handling * .5f;

				}

				if (differential) {

					differential.finalDriveRatio = Mathf.Lerp(5.31f, 2.8f, Mathf.InverseLerp(160f, 380f, vehicle.speed));

				}

				RCCP_VehicleUpgrade_Engine engineUpgrade = vehicle.vehicle.GetComponentInChildren<RCCP_VehicleUpgrade_Engine>(true);
				RCCP_VehicleUpgrade_Handling stabilityUpgrade = vehicle.vehicle.GetComponentInChildren<RCCP_VehicleUpgrade_Handling>(true);
				RCCP_VehicleUpgrade_Speed speedUpgrade = vehicle.vehicle.GetComponentInChildren<RCCP_VehicleUpgrade_Speed>(true);

				if (engineUpgrade)
					engineUpgrade.efficiency = vehicle.upgradedEngineEfficiency;

				if (stabilityUpgrade)
					stabilityUpgrade.efficiency = vehicle.upgradedHandlingEfficiency;

				if (speedUpgrade)
					speedUpgrade.efficiency = vehicle.upgradedSpeedEfficiency;

				EditorUtility.SetDirty(vehicle.vehicle.gameObject);
				PrefabUtility.SavePrefabAsset(vehicle.vehicle.gameObject);

			}

		}

		serializedObject.ApplyModifiedProperties();
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

	}
	
}
