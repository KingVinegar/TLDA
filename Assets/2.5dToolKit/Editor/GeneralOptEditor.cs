using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

partial class VB25dTKEditor {

    private void GeneralOpt() {
        VB25dTK nav = (VB25dTK)target;
        GUILayout.BeginVertical("box");
        EditorGUILayout.Space();
        if (nav.scene == "") {
            nav.scene = SceneManager.GetActiveScene().name;
            if (nav.scene == "") {
                //lblCol.normal.textColor = Color.red;
                EditorGUILayout.LabelField(" A new scene must be saved with a name", lblCol);
                //lblCol.normal.textColor = Color.black;
                EditorGUILayout.Space();
                nav.isInitMissing = true;
            }
        }
        string sceneName = nav.scene;
        if (sceneName == "") {
            sceneName = "Missing";
        }
        EditorGUILayout.TextField("Scene name", sceneName);
        if (nav.cam == null) {
            strCommon = " *";
        }
        else {
            strCommon = "";
        }
        EditorGUI.BeginChangeCheck();
        Camera Cam = EditorGUILayout.ObjectField("Main Camera" + strCommon, nav.cam, typeof(Camera), true) as Camera;
        if (EditorGUI.EndChangeCheck()) {
            nav.cam = Cam;
        }
        if (nav.character == null) {
            strCommon = " *";
        }
        else {
            strCommon = "";
        }
        EditorGUI.BeginChangeCheck();
        GameObject Character = EditorGUILayout.ObjectField("Character" + strCommon, nav.character, typeof(GameObject), true) as GameObject;
        if (EditorGUI.EndChangeCheck()) {
            nav.character = Character;
        }
        if (nav.floor == null && !nav.use25dtkEnv && nav.floorTilemap == null) {
            strCommon = " **";
        }
        else {
            strCommon = "";
        }
        EditorGUI.BeginChangeCheck();
        GameObject Floor = EditorGUILayout.ObjectField("Floor" + strCommon, nav.floor, typeof(GameObject), true) as GameObject;
        if (EditorGUI.EndChangeCheck()) {
            string planeName = "";
            if (nav.floor != null) {
                planeName = nav.floor.name;
            }
            nav.floor = Floor;
            if (nav.use25dtkEnv || nav.listGroup.Count >= 2 || nav.floorTilemap != null) {
                int option = EditorUtility.DisplayDialogComplex("Caution!", "There seems to be work in progress.\n" +
                                                "It should be cleared and the environment restored to default conditions.\n" +
                                                "\n" +
                                                "Press <No> to keep this layout in new job (not recommended)", "Yes", "No", "Cancel");
                switch (option) {
                    case 0:
                        nav.floorTilemap = null;
                        nav.SendMessage("RestoreGridTilemap");
                        nav.gridTilemap = null;
                        if (nav.cam != null && nav.character != null) {
                            nav.SendMessage("ResetValue");
                            nav.SendMessage("ResetGroup");
                            nav.SendMessage("ResetAreasFiles");
                            nav.isBeginWork = false;
                            nav.use25dtkEnv = false;
                        }
                        if (nav.floor == null) {
                            EditorUtility.DisplayDialog("Info!", "Please remove or hide planes in Hierarchy if you want to use 2.5D Toolkit environment option", "OK");
                        }
                        break;
                    case 1:
                        nav.floorTilemap = null;
                        nav.SendMessage("RestoreGridTilemap");
                        nav.gridTilemap = null;
                        nav.isBeginWork = false;
                        nav.use25dtkEnv = false;
                        if (nav.floor == null) {
                            EditorUtility.DisplayDialog("Info!", "Please remove or hide planes in Hierarchy if you want to use 2.5D Toolkit environment option", "OK");
                        }
                        break;
                    case 2:
                        if (nav.floor == null) {
                            nav.floor = GameObject.Find(planeName);
                        }
                        else {
                            nav.floor = null;
                        }
                        break;
                    default:
                        Debug.LogError("Unrecognized option.");
                        break;
                }
                GUIUtility.ExitGUI();
            }
        }
        if (nav.cam == null || nav.character == null) {
            //lblCol.normal.textColor = Color.red;
            EditorGUILayout.LabelField(" * Required", lblCol);
            //lblCol.normal.textColor = Color.black;
            nav.isInitMissing = true;
        }
        if (!nav.use25dtkEnv) {
            if (nav.floor == null && nav.floorTilemap == null) {
                //lblCol.normal.textColor = Color.red;
                EditorGUILayout.LabelField(" ** Use <2.5D Toolkit environment> or Drag any plane into Floor field / Use Tilemap from Tilemap tab", lblCol);
                //lblCol.normal.textColor = Color.black;
                nav.isInitMissing = true;
            }
            else if (nav.cam != null && nav.character != null) {
                if (nav.isInitMissing) {
                    nav.isInitMissing = false;
                }
            }
        }
        if (nav.scene == "") {
            nav.isInitMissing = true;
        }
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), lblStyle, GUILayout.ExpandWidth(false))) {
            showMsg[1] = !showMsg[1];
        }
        EditorGUILayout.LabelField("General options", lblStyle);
        GUILayout.EndHorizontal();
        if (showMsg[1]) {
            EditorGUILayout.HelpBox(StringHelp(1), MessageType.Info);
        }
        if (Application.isPlaying) {
            GUI.enabled = false;
        }
        GUILayout.BeginHorizontal();
        if (nav.isHideGeneralSetting) {
            if (GUILayout.Button(EditorGUIUtility.IconContent("winbtn_win_max_h"), lblStyle, GUILayout.ExpandWidth(false))) {
                nav.isHideGeneralSetting = nav.treeState[1] = false;
            }
            EditorGUILayout.LabelField("Show General options", lblStyle);
        }
        else {
            if (GUILayout.Button(EditorGUIUtility.IconContent("winbtn_win_min_h"), lblStyle, GUILayout.ExpandWidth(false))) {
                nav.isHideGeneralSetting = nav.treeState[1] = true;
            }
            EditorGUILayout.LabelField("Hide General options", lblStyle);
        }
        GUILayout.EndHorizontal();
        if (!nav.isHideGeneralSetting) {
            EditorGUI.BeginChangeCheck();
            bool hideLabel = EditorGUILayout.Toggle("Show areas labels", nav.hideLabels, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck()) {
                if (nav.listGroup.Count >= 2) {
                    nav.hideLabels = hideLabel;
                    nav.labelStyle = null;
                    SceneView.RepaintAll();
                }
            }
            EditorGUI.BeginChangeCheck();
            bool setCharPos = EditorGUILayout.Toggle("Hide character", !nav.isEnableChar, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck()) {
                if (nav.character != null) {
                    if (setCharPos) {
                        nav.isEnableChar = false;
                        nav.SendMessage("EnableCharacter");
                        nav.character.SetActive(false);
                    }
                    else {
                        nav.isEnableChar = true;
                        nav.character.SetActive(true);
                        nav.SendMessage("EnableCharacter");
                    }
                }
            }
            if (nav.cam != null && nav.cam.orthographic) {
                EditorGUI.BeginChangeCheck();
                bool setUpScaling = EditorGUILayout.Toggle("Use character resize", nav.isSetUpScaling, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    nav.isSetUpScaling = setUpScaling;
                    if (!setUpScaling) {
                        nav.isPreviewResize = false;
                        nav.character.transform.localScale = nav.scaleCharV3 = nav.oldCharScale;
                    }
                    else {
                        nav.oldCharScale = nav.character.transform.localScale;
                    }
                }
                if (nav.isSetUpScaling && nav.nScalePoints < 1) {
                    //lblCol.normal.textColor = Color.red;
                    EditorGUILayout.LabelField("Missing parameters", lblCol);
                    //lblCol.normal.textColor = Color.black;
                    nav.isPreviewResize = false;
                    GUI.enabled = false;
                }
                if (nav.isSetUpScaling) {
                    if (!nav.use25dtkEnv) {
                        //
                    }
                    EditorGUI.BeginChangeCheck();
                    bool previewResize = EditorGUILayout.Toggle("Preview resize", nav.isPreviewResize, EditorStyles.toggle);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.isPreviewResize = previewResize;
                        if (previewResize) {
                            Vector3 reallyPos = nav.character.transform.position;
                            nav.character.transform.localScale = new Vector3(nav.listNearPointScale[0].z, nav.listNearPointScale[0].z, nav.listNearPointScale[0].z);
                            nav.posChar = nav.charPos = nav.character.transform.position = reallyPos;
                            nav.diffZ = Mathf.Abs((nav.listNearPointPos[0].z) - (nav.listNearPointPos[1].z));
                            nav.diffScale = Mathf.Abs((nav.listNearPointScale[0].z) - (nav.listNearPointScale[1].z));
                            float ch = nav.listNearPointPos[0].z - reallyPos.z;
                            float c = ((ch * nav.diffScale) / nav.diffZ);
                            nav.character.transform.localScale += new Vector3(c, c, c);
                            nav.scaleCharV3 = nav.character.transform.localScale;
                        }
                        else {
                            nav.character.transform.localScale = nav.scaleCharV3 = nav.oldCharScale;
                        }
                    }
                    GUI.enabled = true;
                    GUILayout.BeginHorizontal();
                    if (nav.isHideResizeSetting) {
                        if (GUILayout.Button(EditorGUIUtility.IconContent("winbtn_win_max_h"), lblStyle, GUILayout.ExpandWidth(false))) {
                            nav.isHideResizeSetting = nav.treeState[0] = false;
                        }
                        EditorGUILayout.LabelField("Show character resize settings", lblStyle);
                    }
                    else {
                        if (GUILayout.Button(EditorGUIUtility.IconContent("winbtn_win_min_h"), lblStyle, GUILayout.ExpandWidth(false))) {
                            nav.isHideResizeSetting = nav.treeState[0] = true;
                        }
                        EditorGUILayout.LabelField("Hide character resize settings", lblStyle);
                    }
                    GUILayout.EndHorizontal();
                    if (!nav.isHideResizeSetting) {
                        EditorGUILayout.BeginVertical("box");
                        if (nav.isAccuracy) {
                            oldColor = GUI.color;
                            GUI.color = boxColAccuracy;
                            GUILayout.BeginVertical("box");
                        }

                        EditorGUI.BeginChangeCheck();
                        Vector3 CharScaleAuto = EditorGUILayout.Vector3Field("Scale", nav.scaleCharV3);
                        if (EditorGUI.EndChangeCheck()) {
                            nav.lockCharScaleAuto = true;
                            nav.startCharScale = nav.scaleCharV3;
                            nav.scaleCharV3 = CharScaleAuto;
                            nav.SendMessage("ScaleChar");
                        }
                        GUI.enabled = true;
                        if (nav.isAccuracy) {
                            lblCol.normal.textColor = Color.black;
                            GUILayout.EndVertical();
                            GUI.color = oldColor;
                        }
                        //lblCol.normal.textColor = new Color(0.490f, 0.043f, 0.074f);
                        EditorGUILayout.LabelField(" First point", lblCol);
                        //lblCol.normal.textColor = Color.black;
                        GUI.enabled = false;
                        if (nav.nScalePoints < 0) {
                            Vector3 pointFarPos = EditorGUILayout.Vector3Field("Position", nav.character.transform.position);
                            Vector3 pointFarScale = EditorGUILayout.Vector3Field("Scale", nav.character.transform.localScale);
                        }
                        else {
                            Vector3 pointFarPos = EditorGUILayout.Vector3Field("Position", nav.listNearPointPos[0]);
                            Vector3 pointFarScale = EditorGUILayout.Vector3Field("Scale", nav.listNearPointScale[0]);
                        }
                        GUI.enabled = true;
                        EditorGUILayout.BeginHorizontal();
                        if (nav.nScalePoints == -1) {
                            GUI.enabled = true;
                        }
                        else {
                            GUI.enabled = false;
                        }
                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "save.png", typeof(Texture2D));
                        icon_con = new GUIContent(" " + "Save point", icon);
                        if (GUILayout.Button(icon_con)) {
                            GUI.FocusControl(null);
                            nav.listNearPointPos[0] = nav.character.transform.position;
                            nav.listNearPointScale[0] = nav.character.transform.localScale;
                            nav.nScalePoints = 0;
                            nav.listNearPointPos.Add(Vector3.zero);
                            nav.listNearPointScale.Add(Vector3.one);
                        }
                        GUI.enabled = true;
                        if (nav.nScalePoints == -1) {
                            GUI.enabled = false;
                        }
                        else {
                            GUI.enabled = true;
                        }
                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                        icon_con = new GUIContent(" " + "Reset point", icon);
                        if (GUILayout.Button(icon_con)) {
                            GUI.FocusControl(null);
                            nav.nScalePoints = -1;
                            nav.character.transform.localScale = nav.scaleCharV3 = nav.oldCharScale;
                            nav.listNearPointPos.Clear();
                            nav.listNearPointScale.Clear();
                            nav.listNearPointPos.Add(Vector3.zero);
                            nav.listNearPointScale.Add(Vector3.one);
                        }

                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "far.png", typeof(Texture2D));
                        icon_con = new GUIContent(" " + "Go to point", icon);
                        if (GUILayout.Button(icon_con)) {
                            nav.character.transform.position = nav.listNearPointPos[0];
                            nav.character.transform.localScale = nav.scaleCharV3 = nav.listNearPointScale[0];
                            nav.regionScale = 0;
                        }
                        EditorGUILayout.EndHorizontal();
                        if (nav.nScalePoints >= 0) {
                            //lblCol.normal.textColor = new Color(0.490f, 0.043f, 0.074f);
                            EditorGUILayout.LabelField(" Second point", lblCol);
                            //lblCol.normal.textColor = Color.black;
                        }
                        GUI.enabled = false;
                        if (nav.nScalePoints >= 0) {
                            if (nav.nScalePoints < 1) {
                                Vector3 pointFarPos = EditorGUILayout.Vector3Field("Position", nav.character.transform.position);
                                Vector3 pointFarScale = EditorGUILayout.Vector3Field("Scale", nav.character.transform.localScale);
                            }
                            else {
                                Vector3 pointNearPos = EditorGUILayout.Vector3Field("Position", nav.listNearPointPos[1]);
                                Vector3 pointNearScale = EditorGUILayout.Vector3Field("Scale", nav.listNearPointScale[1]);
                            }
                            GUI.enabled = true;
                            EditorGUILayout.BeginHorizontal();
                            if (nav.nScalePoints == 0) {
                                GUI.enabled = true;
                            }
                            else {
                                GUI.enabled = false;
                            }
                            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "save.png", typeof(Texture2D));
                            icon_con = new GUIContent(" " + "Save point", icon);
                            if (GUILayout.Button(icon_con)) {
                                if (nav.character.transform.position.z > nav.listNearPointPos[0].z) {
                                    nav.listNearPointPos[1] = nav.listNearPointPos[0];
                                    nav.listNearPointScale[1] = nav.listNearPointScale[0];
                                    nav.listNearPointPos[0] = (nav.character.transform.position);
                                    nav.listNearPointScale[0] = (nav.character.transform.localScale);
                                    nav.nScalePoints = 1;
                                }
                                else {
                                    nav.listNearPointPos[1] = (nav.character.transform.position);
                                    nav.listNearPointScale[1] = (nav.character.transform.localScale);
                                    nav.nScalePoints = 1;
                                }
                            }
                            GUI.enabled = true;
                            if (nav.nScalePoints == 1) {
                                GUI.enabled = true;
                            }
                            else {
                                GUI.enabled = false;
                            }
                            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                            icon_con = new GUIContent(" " + "Reset point", icon);
                            if (GUILayout.Button(icon_con)) {
                                nav.nScalePoints = 0;
                                nav.character.transform.localScale = nav.scaleCharV3 = nav.listNearPointScale[0];
                            }

                            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "far.png", typeof(Texture2D));
                            icon_con = new GUIContent(" " + "Go to point", icon);
                            if (GUILayout.Button(icon_con)) {
                                nav.character.transform.position = nav.listNearPointPos[1];
                                nav.character.transform.localScale = nav.scaleCharV3 = nav.listNearPointScale[1];
                                nav.regionScale = 1;
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        GUI.enabled = true;
                        EditorGUILayout.EndVertical();
                    }
                }
            }
            else {
                if (nav.isSetUpScaling) {
                    nav.isSetUpScaling = nav.isPreviewResize = false;
                }
            }
            EditorGUI.BeginChangeCheck();
            float sizeSpot;
            if (nav.cam != null) {
                if (nav.cam.orthographic) {
                    sizeSpot = EditorGUILayout.Slider("Spot size", nav.spotSize, 2f, 200f);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.spotSize = sizeSpot;
                        SceneView.RepaintAll();
                    }
                }
                else {
                    sizeSpot = EditorGUILayout.Slider("Spot size", nav.spotSize, 2f, 1000f);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.spotSize = sizeSpot;
                        SceneView.RepaintAll();
                    }
                }
            }
            EditorGUI.BeginChangeCheck();
            bool wireSpot = EditorGUILayout.Toggle("Draw wire spot", nav.isWireSpot, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck()) {
                nav.isWireSpot = wireSpot;
                SceneView.RepaintAll();
            }
            if (nav.isAccuracy) {
                oldColor = GUI.color;
                GUI.color = boxColAccuracy;
                GUILayout.BeginVertical("box");
            }
            if (Application.isPlaying) {
                GUI.enabled = false;
            }
            EditorGUI.BeginChangeCheck();
            bool unityBehavior = EditorGUILayout.Toggle("Enable accuracy", nav.isAccuracy, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck()) {
                nav.isAccuracy = unityBehavior;
            }
            if (nav.isAccuracy) {
                EditorGUI.BeginChangeCheck();
                bool moreAccuracy = EditorGUILayout.Toggle("More accuracy", nav.isMoreAccuracy, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    nav.isMoreAccuracy = moreAccuracy;
                    if (nav.isMoreAccuracy) {
                        nav.isExtremeAccuracy = false;
                    }
                }
                EditorGUI.BeginChangeCheck();
                bool extremeAccuracy = EditorGUILayout.Toggle("Extreme accuracy", nav.isExtremeAccuracy, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    nav.isExtremeAccuracy = extremeAccuracy;
                    if (nav.isExtremeAccuracy) {
                        nav.isMoreAccuracy = false;
                    }
                }
            }
            if (nav.isAccuracy) {
                //lblCol.normal.textColor = Color.red;
                EditorGUILayout.LabelField("You can't directly type values into these fields", lblCol);
                //lblCol.normal.textColor = Color.black;
                GUILayout.EndVertical();
                GUI.color = oldColor;
            }
            if (nav.isBtnRepeat) {
                oldColor = GUI.color;
                GUI.color = boxColHoldDown;
                GUILayout.BeginVertical("box");
            }
            EditorGUI.BeginChangeCheck();
            bool btnRepeat = EditorGUILayout.Toggle("Enable hold down", nav.isBtnRepeat, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck()) {
                nav.isBtnRepeat = btnRepeat;
            }
            if (nav.isBtnRepeat) {
                GUILayout.EndVertical();
                GUI.color = oldColor;
            }
            EditorGUI.BeginChangeCheck();
            bool viewFrustum = EditorGUILayout.Toggle("View camera", nav.isViewFrustum, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck()) {
                nav.isViewFrustum = viewFrustum;
            }
        }
        GUILayout.EndVertical();
    }
}

