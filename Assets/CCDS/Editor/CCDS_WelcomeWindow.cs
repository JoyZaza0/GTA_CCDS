//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class CCDS_WelcomeWindow : EditorWindow {

    public class ToolBar {

        public string title;
        public UnityEngine.Events.UnityAction Draw;

        /// <summary>
        /// Create New Toolbar
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="onDraw">Method to draw when toolbar is selected</param>
        public ToolBar(string title, UnityEngine.Events.UnityAction onDraw) {

            this.title = title;
            this.Draw = onDraw;

        }

        public static implicit operator string(ToolBar tool) {
            return tool.title;
        }

    }

    /// <summary>
    /// Index of selected toolbar.
    /// </summary>
    public int toolBarIndex = 0;

    /// <summary>
    /// List of Toolbars
    /// </summary>
    public ToolBar[] toolBars = new ToolBar[]{

        new ToolBar("Welcome", WelcomePageContent),
        new ToolBar("Demos", Scenes),
        new ToolBar("Updates", UpdatePageContent),
        new ToolBar("DOCS", Documentations)

    };

    public static Texture2D bannerTexture = null;

    private GUISkin skin;

    private const int windowWidth = 600;
    private const int windowHeight = 520;

    public static void OpenWindow() {

        GetWindow<CCDS_WelcomeWindow>(true);

    }

    private void OnEnable() {

        titleContent = new GUIContent("City Car Driving Simulator");
        minSize = new Vector2(windowWidth, windowHeight);
        maxSize = new Vector2(windowWidth * 1.5f, windowHeight * 1.5f);

        InitStyle();

    }

    private void InitStyle() {

        if (!skin)
            skin = Resources.Load("CCDS_Gui") as GUISkin;

        bannerTexture = (Texture2D)Resources.Load("Editor Icons/CCDS_Banner", typeof(Texture2D));

    }

    private void OnGUI() {

        GUI.skin = skin;

        DrawHeader();
        DrawMenuButtons();
        DrawToolBar();
        DrawFooter();

        if (!EditorApplication.isPlaying)
            Repaint();

    }

    private void DrawHeader() {

        GUILayout.Label(bannerTexture, GUILayout.Height(120));

    }

    private void DrawMenuButtons() {

        GUILayout.Space(-10);
        toolBarIndex = GUILayout.Toolbar(toolBarIndex, ToolbarNames());

    }

    #region ToolBars

    public static void WelcomePageContent() {

        GUILayout.Label("<size=18>Welcome!</size>");
        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Label("Thank you for purchasing and using City Car Driving Simulator. Please read the documentations before use. Also check out the online documentations for updated info. Have fun :)");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Label("CCDS uses demo assets of the RCCP for tutorial purposes and they should be removed from the project if you're not going to use them in the project. Otherwise build size will be increased dramatically.");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Label("Please read the documentation before use. Also watching the YouTube tutorial videos might be helpful. Don't waste your time driving around, let's get back to work! :)");
        EditorGUILayout.EndHorizontal();

        GUI.color = Color.red;

        if (GUILayout.Button("Delete all demo contents from the project")) {

            if (EditorUtility.DisplayDialog("Warning", "You are about to delete all demo contents such as vehicle models, vehicle prefabs, vehicle textures, all scenes, scene models, scene prefabs, scene textures!", "Delete", "Cancel"))
                DeleteDemoContent();

        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.Separator();

        GUI.color = Color.white;

    }

    public static void UpdatePageContent() {

        GUILayout.Label("<size=18>Updates</size>");

        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Label("<b>Installed Version: </b>" + CCDS_Version.version.ToString());
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);

        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Label("<b>1</b>- Always backup your project before updating CCDS or any asset in your project!");
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);

        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Label("<b>2</b>- If you have own assets such as prefabs, audioclips, models, scripts in CCDS folder, keep your own asset outside from CCDS folder.");
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);

        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Label("<b>3</b>- Delete CCDS folder, and import latest version to your project.");
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);

        if (GUILayout.Button("Check Updates"))
            Application.OpenURL("https://u3d.as/3eqY");

        GUILayout.Space(6);

        GUILayout.FlexibleSpace();

    }

    public static void Scenes() {

        GUILayout.Label("<size=18>Demo Scenes</size>");

        EditorGUILayout.Separator();
        EditorGUILayout.HelpBox("All scenes must be in your Build Settings to run the demo.", MessageType.Warning, true);
        EditorGUILayout.Separator();

        EditorGUILayout.BeginVertical("box");

        if (GUILayout.Button("CCDS_MainMenu_City_1")) {

            EditorSceneManager.OpenScene("Assets/CCDS/Scenes/CCDS_MainMenu_City.unity", OpenSceneMode.Single);

        }

        if (GUILayout.Button("CCDS_Gameplay_City_1")) {

            EditorSceneManager.OpenScene("Assets/CCDS/Scenes/CCDS_Gameplay_City_1.unity", OpenSceneMode.Single);

        }

        if (GUILayout.Button("CCDS_Gameplay_City_2")) {

            EditorSceneManager.OpenScene("Assets/CCDS/Scenes/CCDS_Gameplay_City_2.unity", OpenSceneMode.Single);

        }

        if (GUILayout.Button("CCDS_Vehicles_Prototype")) {

            EditorSceneManager.OpenScene("Assets/CCDS/Scenes/CCDS_Vehicles_Prototype.unity", OpenSceneMode.Single);

        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        GUILayout.FlexibleSpace();

    }

    public static void Documentations() {

        GUILayout.Label("<size=18>Dcoumentation</size>");

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.HelpBox("Offline documentations can be found in the documentations folder.", MessageType.Info);

        if (GUILayout.Button("Online Documentations"))
            Application.OpenURL("https://www.bonecrackergames.com/city-car-driving-simulator/");

        if (GUILayout.Button("Youtube Tutorial Videos"))
            Application.OpenURL("https://www.bonecrackergames.com/city-car-driving-simulator/");

        if (GUILayout.Button("Other Assets"))
            Application.OpenURL("https://www.bonecrackergames.com/city-car-driving-simulator/");

        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();

    }

    #endregion

    private string[] ToolbarNames() {

        string[] names = new string[toolBars.Length];

        for (int i = 0; i < toolBars.Length; i++)
            names[i] = toolBars[i];

        return names;

    }

    private void DrawToolBar() {

        GUILayout.BeginArea(new Rect(4, 150, position.width - 8, position.height - 190));

        toolBars[toolBarIndex].Draw();

        GUILayout.EndArea();

        GUILayout.FlexibleSpace();

    }

    private void DrawFooter() {

        EditorGUILayout.BeginHorizontal("box");

        EditorGUILayout.LabelField("BoneCracker Games", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.LabelField("City Car Driving Simulator " + CCDS_Version.version, EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.LabelField("Ekrem Bugra Ozdoganlar", EditorStyles.centeredGreyMiniLabel);

        EditorGUILayout.EndHorizontal();

    }

    private static void DeleteDemoContent() {

        Debug.LogWarning("Deleting demo contents...");

        foreach (var item in RCCP_DemoContent.Instance.contents) {

            if (item != null)
                FileUtil.DeleteFileOrDirectory(RCCP_GetAssetPath.GetAssetPath(item));

        }

        RCCP_DemoVehicles.Instance.vehicles = new RCCP_CarController[1];
        RCCP_DemoVehicles.Instance.vehicles[0] = RCCP_PrototypeContent.Instance.vehicles[0];
        RCCP_DemoScenes.Instance.Clean();

        List<CCDS_PlayerVehicles.PlayerVehicle> allPlayerVehiclesList = new List<CCDS_PlayerVehicles.PlayerVehicle>();

        for (int i = 0; i < CCDS_PlayerVehicles.Instance.playerVehicles.Length; i++) {

            if (CCDS_PlayerVehicles.Instance.playerVehicles[i].vehicle != null)
                allPlayerVehiclesList.Add(CCDS_PlayerVehicles.Instance.playerVehicles[i]);

        }

        CCDS_PlayerVehicles.Instance.playerVehicles = allPlayerVehiclesList.ToArray();

        EditorUtility.SetDirty(RCCP_DemoVehicles.Instance);
        EditorUtility.SetDirty(RCCP_DemoScenes.Instance);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        RCCP_SetScriptingSymbol.SetEnabled("RCCP_DEMO", false);

        Debug.LogWarning("Deleted demo contents!");
        EditorUtility.DisplayDialog("Deleted Demo Contents", "All demo contents have been deleted!", "Ok");

    }

}
