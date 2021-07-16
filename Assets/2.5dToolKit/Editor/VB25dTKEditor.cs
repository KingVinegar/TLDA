using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(VB25dTK))]
partial class VB25dTKEditor : Editor {

    // add a menu named "Tools/VB Game Studio" to Unity main menu
    [MenuItem("Tools/VB Game Studio/2.5D Toolkit")]
    private static void NewMenuOption() {
        GameObject VB25dTK = new GameObject("VB25dTK");
        VB25dTK.AddComponent<VB25dTK>();
        Selection.activeGameObject = VB25dTK;
    }

    // menu is enabled if "VB25dTK" object does not exist
    [MenuItem("Tools/VB Game Studio/2.5D Toolkit", true)]
    private static bool NewMenuOptionValidation() {
        return GameObject.Find("VB25dTK") == false;
    }
    private string[] placeLabelDegrees = new string[] { "Near mouse", "Fixed on top" };
    private string[] isWalkable = new string[] { "Walkable", "NotWalkable" };
    private string iconPath = "Assets/2.5dToolKit/Graphics/PrefabIcons/";
    private GUIStyle lblStyle = new GUIStyle();
    private GUIStyle lblCol = new GUIStyle();
    private GameObject keepSelection;
    private GameObject VBBGCamera;
    private int emptySlot = 100;
    private string stringMsg;
    private string strCommon;
    private Color colMouseOverSpot;
    private Color colInactiveLine;
    private Color boxColAccuracy;
    private Color boxColHoldDown;
    private Color colNotWalk;
    private Color oldColor;
    private Color colFloor;
    private Color colLine;
    private Color colText;
    private Color colWalk;
    private Color colMesh;
    private Color colBox;
    private bool[] showMsg = new bool[16];
    private bool hideObj;
    GUIContent icon_con;
    Vector2 Dimensions;
    Texture2D bgnormal;
    Texture2D bgactive;
    Texture2D bgbox;
    Texture2D icon;

    void OnEditorUpdate() {
        if (!NewMenuOptionValidation()) {
            VB25dTK nav = (VB25dTK)target;
            EditorUtility.SetDirty(nav);
        }
    }

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        VB25dTK nav = (VB25dTK)target;
        //GUI.skin.button.normal.background = bgnormal;
        //GUI.skin.button.active.background = bgactive;
        //GUI.skin.box.normal.background = bgbox;
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;

        if (nav.currentState == "Editing.") {
            if (nav.selectedTab != 2) {
                if (nav.isSelectedSpot) {
                    nav.SendMessage("ConfirmSpotPos");
                }
                nav.nGroup = 0;
                nav.SendMessage("OnChangeGroup");
            }
        }
        else if (nav.SpriteImg != null || (nav.isHideAreas)) {
            if (nav.selectedTab != 3) {
                nav.SendMessage("EmptyObject");
            }
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), lblStyle, GUILayout.ExpandWidth(false))) {
            showMsg[0] = !showMsg[0];
        }
        EditorGUILayout.LabelField("2.5D Toolkit", lblStyle);
        GUILayout.EndHorizontal();

        if (showMsg[0]) {
            EditorGUILayout.HelpBox(StringHelp(0), MessageType.Info);
        }
        GeneralOpt();
        GUI.enabled = true;

        // create new tabs
        GUILayout.BeginVertical("box");

        EditorGUI.BeginChangeCheck();
        nav.selectedTabTop = GUILayout.Toolbar(nav.selectedTabTop, new string[] { "Settings", "2.5DTK Env.", "Navigation" });
        switch (nav.selectedTabTop) {
            case 0:
                nav.selectedTabExtensions = 3;
                nav.selectedTabBottom = 3;
                nav.selectedTab = 0;
                break;
            case 1:
                nav.selectedTabExtensions = 3;
                nav.selectedTabBottom = 3;
                nav.selectedTab = 1;
                break;
            case 2:
                nav.selectedTabExtensions = 3;
                nav.selectedTabBottom = 3;
                nav.selectedTab = 2;
                break;
        }
        nav.selectedTabBottom = GUILayout.Toolbar(nav.selectedTabBottom, new string[] { "Objects", "Mesh", "Tilemap" });
        switch (nav.selectedTabBottom) {
            case 0:
                nav.selectedTabExtensions = 3;
                nav.selectedTabTop = 3;
                nav.selectedTab = 3;
                break;
            case 1:
                nav.selectedTabExtensions = 3;
                nav.selectedTabTop = 3;
                nav.selectedTab = 4;
                break;
            case 2:
                nav.selectedTabExtensions = 3;
                nav.selectedTabTop = 3;
                nav.selectedTab = 5;
                break;
        }
        nav.selectedTabExtensions = GUILayout.Toolbar(nav.selectedTabExtensions, new string[] { "Advanced", "", "" });
        switch (nav.selectedTabExtensions) {
            case 0:
                nav.selectedTabBottom = 3;
                nav.selectedTabTop = 3;
                nav.selectedTab = 6;
                break;
            case 1:
                nav.selectedTabExtensions = 3;
                nav.selectedTabBottom = 3;
                nav.selectedTabTop = 3;
                break;
            case 2:
                nav.selectedTabExtensions = 3;
                nav.selectedTabBottom = 3;
                nav.selectedTabTop = 3;
                break;
        }
        if (EditorGUI.EndChangeCheck()) {
            GUI.FocusControl(null);
        }
        if (Application.isPlaying) {
            nav.selectedTabBottom = 3;
            nav.selectedTab = nav.selectedTabTop = 2;
        }
        switch (nav.selectedTab) {
            case 0:
                Settings();
                break;
            case 1:
                Use25dTKEnv();
                break;
            case 2:
                Navigation();
                break;
            case 3:
                Objects();
                break;
            case 4:
                Mesh();
                break;
            case 5:
                Tilemap();
                break;
            case 6:
                Advanced();
                break;
        }
        EditorGUILayout.Space();
        GUILayout.EndVertical();
    }

    // draw in Scene View
    void OnSceneGUI() {
        VB25dTK nav = (VB25dTK)target;
        if (nav.isDrawInSceneView) {
            DrawSceneView();
        }
    }

    // info message text
    private string StringHelp(int index) {
        StrHelp(index);
        return stringMsg;
    }

    private void CreateLayer(string name) {
        CreateSlotLayer(name);
    }

    void OnEnable() {
        VB25dTK nav = (VB25dTK)target;
        nav.InitializeEditor();
        nav.InitializeVB25dTK();
        EditorGUIUtility.wideMode = true;
        bgnormal = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "button.png", typeof(Texture2D));
        bgactive = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "button-active.png", typeof(Texture2D));
        bgbox = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "box.png", typeof(Texture2D));
        lblCol.wordWrap = true;
        //lblStyle.normal.textColor = Color.blue;
        lblStyle.fontStyle = FontStyle.BoldAndItalic;
        boxColAccuracy = new Color(0.90f, 0.98f, 1f);
        boxColHoldDown = new Color(0.9981f, 1f, 0.9019f);
        keepSelection = GameObject.Find("VB25dTK");
        EditorApplication.update += OnEditorUpdate;
        Tools.hidden = true;
    }

    void OnDisable() {
        EditorApplication.update -= OnEditorUpdate;
        Tools.hidden = false;
    }
}