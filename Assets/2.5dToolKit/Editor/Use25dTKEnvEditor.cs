using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

partial class VB25dTKEditor {

    Vector3 storeCharV3;
    Vector3 storeCubeV3;
    Vector3[] storeWidthPosV3 = new Vector3[2];
    Vector3[] storeDepthPosV3 = new Vector3[2];

    private void Use25dTKEnv() {
        VB25dTK nav = (VB25dTK)target;
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), lblStyle, GUILayout.ExpandWidth(false))) {
            showMsg[5] = !showMsg[5];
        }
        EditorGUILayout.LabelField("Use 2.5D Toolkit environment", lblStyle);
        GUILayout.EndHorizontal();
        if (showMsg[5]) {
            EditorGUILayout.HelpBox(StringHelp(5), MessageType.Info);
        }
        if (nav.use25dtkEnv) {
            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "end.png", typeof(Texture2D));
            icon_con = new GUIContent(" " + "End", icon);
        }
        else {
            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "start.png", typeof(Texture2D));
            icon_con = new GUIContent(" " + "Start", icon);
            if (GameObject.Find("VBCubeUI")) {
                nav.SendMessage("RemoveCube");
            }
        }
        if (GUILayout.Button(icon_con)) {
            if (nav.cam == null || nav.character == null || SceneManager.GetActiveScene().name == "" || nav.isUseImgInScene || !nav.isUseImgBackground) {
                EditorUtility.DisplayDialog("Info!", "End initialization to begin or use Background Camera", "Ok");
            }
            else {
                nav.use25dtkEnv = !nav.use25dtkEnv;
                if (nav.use25dtkEnv) {
                    nav.isInitMissing = false;
                    if (nav.floor != null || nav.listGroup.Count >= 2 || nav.floorTilemap != null) {
                        int option = EditorUtility.DisplayDialogComplex("Caution!", "There seems to be work in progress.\n" +
                                                        "It should be cleared and the environment restored to default conditions.\n" +
                                                        "(please remove or hide planes in Hierarchy or this function may not work properly).\n" +
                                                        "\n" +
                                                        "Press <No> to keep this layout in new job (not recommended)", "Yes", "No", "Cancel");
                        switch (option) {
                            case 0:
                                nav.floorTilemap = null;
                                nav.SendMessage("RestoreGridTilemap");
                                nav.gridTilemap = null;
                                nav.floor = null;
                                if (nav.cam != null && nav.character != null) {
                                    nav.SendMessage("ResetValue");
                                    nav.SendMessage("ResetGroup");
                                    nav.SendMessage("ResetAreasFiles");
                                    nav.isBeginWork = true;
                                    InitializeEnv();
                                    SceneView.RepaintAll();
                                }
                                break;
                            case 1:
                                nav.floorTilemap = null;
                                nav.SendMessage("RestoreGridTilemap");
                                nav.gridTilemap = null;
                                nav.floor = null;
                                nav.isBeginWork = true;
                                InitializeEnv();
                                SceneView.RepaintAll();
                                break;
                            case 2:
                                nav.isBeginWork = false;
                                nav.use25dtkEnv = false;
                                nav.SendMessage("RemoveCube");
                                SceneView.RepaintAll();
                                break;
                            default:
                                Debug.LogError("Unrecognized option.");
                                break;
                        }
                    }
                    else {
                        if (nav.cam != null && nav.character != null) {
                            nav.isBeginWork = true;
                            nav.isShowFloor = true;
                            InitializeEnv();
                            SceneView.RepaintAll();
                        }
                    }
                }
                else {
                    if (nav.listGroup.Count >= 2) {
                        int option = EditorUtility.DisplayDialogComplex("Caution!", "There seems to be work in progress.\n" +
                                                        "It should be cleared and the environment restored to default conditions.\n" +
                                                        "\n" +
                                                        "Press <No> to keep this layout in new job (not recommended)", "Yes", "No", "Cancel");
                        switch (option) {
                            case 0:
                                if (nav.cam != null && nav.character != null) {
                                    nav.SendMessage("ResetValue");
                                    nav.SendMessage("ResetGroup");
                                    nav.SendMessage("ResetAreasFiles");
                                    nav.isBeginWork = false;
                                    nav.isSetCharPos = false;
                                    nav.SendMessage("RemoveCube");
                                    nav.SendMessage("ResetChar");
                                    SceneView.RepaintAll();
                                }
                                break;
                            case 1:
                                nav.isBeginWork = false;
                                nav.isSetCharPos = false;
                                nav.SendMessage("RemoveCube");
                                SceneView.RepaintAll();
                                break;
                            case 2:
                                nav.isBeginWork = true;
                                nav.use25dtkEnv = true;
                                SceneView.RepaintAll();
                                break;
                            default:
                                Debug.LogError("Unrecognized option.");
                                break;
                        }
                    }
                }
            }
            GUIUtility.ExitGUI();
        }
        if (nav.use25dtkEnv) {
            EditorGUILayout.Space();
            if (nav.cam == null) {
                nav.isBeginWork = false;
                nav.use25dtkEnv = false;
            }
            else if (nav.character == null) {
                nav.isBeginWork = false;
                nav.use25dtkEnv = false;
            }
            else {
                Dimensions = GUI.skin.label.CalcSize(new GUIContent("View background"));
                EditorGUIUtility.labelWidth = Dimensions.x + 20;
                lblCol.normal.textColor = Color.black;
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + "Initialize environment", icon);
                if (GUILayout.Button(icon_con)) {
                    if (EditorUtility.DisplayDialog("Caution!", "Previous job will be canceled and the environment restored to default conditions.\n" +
                                                                "Do you want to continue?", "Yes", "Cancel")) {
                        nav.SendMessage("ResetValue");
                        nav.SendMessage("ResetGroup");
                        nav.SendMessage("ResetAreasFiles");
                        nav.SendMessage("RemoveCube");
                        InitializeEnv();
                    }
                    GUIUtility.ExitGUI();
                }
                EditorGUILayout.Space();
                lblCol.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField(" Floor", lblCol);
                lblCol.fontStyle = FontStyle.Normal;
                EditorGUI.BeginChangeCheck();
                float planeStart = EditorGUILayout.FloatField("Position", nav.vert);
                if (EditorGUI.EndChangeCheck()) {
                    StoreObjPos();
                    nav.startFloorPos = nav.vert;
                    nav.vert = planeStart;
                    nav.SendMessage("MoveFloor");
                    nav.SendMessage("CreateFloor");
                    if (!nav.cam.orthographic) {
                        RestoreObjPos();
                    }
                    else {
                        RestoreObjPosOrtho();
                    }
                }
                EditorGUI.BeginChangeCheck();
                bool floorShow = EditorGUILayout.Toggle("Show floor", nav.isShowFloor, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    nav.isShowFloor = floorShow;
                }
                if (floorShow) {
                    EditorGUI.BeginChangeCheck();
                    int squareX = EditorGUILayout.IntSlider("Dimension", nav.nSquare, 2, 1000);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.nSquare = squareX;
                    }
                }
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + "Reset Floor", icon);
                if (GUILayout.Button(icon_con)) {
                    GUI.FocusControl(null);
                    float yChar = nav.vert = -1f;
                    nav.character.transform.position = new Vector3(nav.character.transform.position.x, yChar, nav.character.transform.position.z);

                }
                EditorGUILayout.Space();
                lblCol.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField(" Main camera", lblCol);
                lblCol.fontStyle = FontStyle.Normal;
                EditorGUI.BeginChangeCheck();
                bool camOff = EditorGUILayout.Toggle("Camera off", !nav.cam.enabled, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    nav.cam.enabled = !camOff;
                }
                //lblCol.normal.textColor = new Color(0.490f, 0.043f, 0.074f);
                if (nav.cam.orthographic) {
                    EditorGUILayout.LabelField(" Projection: Orthographic", lblCol);
                    nav.isCamOrtho = true;
                }
                else {
                    EditorGUILayout.LabelField(" Projection: Perspective", lblCol);
                    nav.isCamOrtho = false;
                }
                lblCol.normal.textColor = Color.black;
                Dimensions = GUI.skin.label.CalcSize(new GUIContent("View background"));
                EditorGUIUtility.labelWidth = Dimensions.x + 20;
                EditorGUI.BeginChangeCheck();
                bool projectionMode = EditorGUILayout.Toggle("Orthographic", nav.isCamOrtho, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    string firstLine = "Do you want to automatically set camera size?";
                    string secondLine = "You can also do it later at any time by clicking the <Apply camera orthographic size> button";
                    if (!projectionMode) {
                        firstLine = "Do you want to automatically set camera distance?";
                        secondLine = "You can also do it later at any time by clicking the <Apply camera distance> button";
                    }
                    int option = EditorUtility.DisplayDialogComplex("Caution!",
                                                                "Changing projection all changes will be lost.\n" +
                                                                firstLine + "\n" +
                                                                secondLine,
                                                                "Set it",
                                                                "Don't set",
                                                                "Cancel");
                    switch (option) {
                        case 0:
                            nav.SendMessage("ResetValue");
                            nav.SendMessage("ResetGroup");
                            nav.SendMessage("ResetAreasFiles");
                            nav.SendMessage("RemoveCube");
                            nav.SendMessage("reset25dTKEnv");
                            if (projectionMode) {
                                nav.cam.orthographic = nav.isCamOrtho = true;
                                nav.spotSize = 20f;
                                nav.cam.orthographicSize = nav.backgroundImg.rect.height / (2f * nav.backgroundImg.pixelsPerUnit);
                                nav.sizeCam = nav.cam.orthographicSize;
                            }
                            else {
                                nav.cam.orthographic = nav.isCamOrtho = false;
                                nav.spotSize = 100f;
                                nav.cam.fieldOfView = nav.fovCam = 60;
                                float frustumHeight = nav.backgroundImg.bounds.size.x / nav.cam.aspect;
                                float distance = frustumHeight * 0.5f / Mathf.Tan(nav.cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                                Vector3 newPos = new Vector3(nav.cam.transform.position.x, nav.cam.transform.position.y, -distance);
                                nav.cam.transform.position = nav.posCamV3 = newPos;
                            }
                            break;
                        case 1:
                            nav.SendMessage("ResetValue");
                            nav.SendMessage("ResetGroup");
                            nav.SendMessage("ResetAreasFiles");
                            nav.SendMessage("RemoveCube");
                            nav.SendMessage("reset25dTKEnv");
                            if (projectionMode) {
                                nav.cam.orthographic = nav.isCamOrtho = true;
                                nav.spotSize = 20f;
                            }
                            else {
                                nav.cam.orthographic = nav.isCamOrtho = false;
                                nav.spotSize = 100f;
                                nav.cam.fieldOfView = nav.fovCam = 60;
                            }
                            break;
                        case 2:
                            // 
                            break;
                        default:
                            Debug.LogError("Unrecognized option.");
                            break;
                    }
                    GUIUtility.ExitGUI();
                }
                if (nav.isAccuracy) {
                    oldColor = GUI.color;
                    GUI.color = boxColAccuracy;
                    GUILayout.BeginVertical("box");
                }
                EditorGUI.BeginChangeCheck();
                Vector3 CamPos = EditorGUILayout.Vector3Field("Position", nav.posCamV3);
                if (EditorGUI.EndChangeCheck()) {
                    StoreObjPos();
                    nav.startCamPos = nav.posCamV3;
                    nav.posCamV3 = CamPos;
                    nav.SendMessage("PosCam");
                    nav.SendMessage("CreateFloor");
                    if (!nav.cam.orthographic) {
                        RestoreObjPos();
                    }
                    else {
                        RestoreObjPosOrtho();
                    }
                }
                EditorGUI.BeginChangeCheck();
                Vector3 CamRot = EditorGUILayout.Vector3Field("Rotation", nav.rotCam);
                if (EditorGUI.EndChangeCheck()) {
                    if (CamRot.x < 0f) {
                        return;
                    }
                    StoreObjPos();
                    nav.startCamRot = nav.rotCam;
                    nav.rotCam = CamRot;
                    nav.SendMessage("RotCam");
                    nav.SendMessage("CreateFloor");
                    if (!nav.cam.orthographic) {
                        RestoreObjPos();
                    }
                    else {
                        RestoreObjPosOrtho();
                    }
                    if (nav.setBackgroundImg != null && !nav.isBGManualSetting) {
                        nav.RotBGV3 = nav.rotCam;
                        nav.setBackgroundImg.transform.rotation = Quaternion.Euler(nav.RotBGV3);
                        nav.SendMessage("centerBG");
                    }
                }
                if (nav.isAccuracy) {
                    GUILayout.EndVertical();
                    GUI.color = oldColor;
                }
                if (!nav.isCamOrtho) {
                    EditorGUILayout.Space();
                    //lblCol.normal.textColor = new Color(0.490f, 0.043f, 0.074f);
                    string AutoEnabled = "(Enabled)";
                    if (nav.isAutoDistanceDisabled) {
                        AutoEnabled = "(Disabled)";
                    }
                    EditorGUILayout.LabelField(" Automatic distance on FOV variations " + AutoEnabled, lblCol);
                    lblCol.normal.textColor = Color.black;
                    EditorGUI.BeginChangeCheck();
                    bool autoDistanceDisabled = EditorGUILayout.Toggle("Disable", nav.isAutoDistanceDisabled, EditorStyles.toggle);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.isAutoDistanceDisabled = autoDistanceDisabled;
                    }
                }
                EditorGUI.BeginChangeCheck();
                if (nav.isCamOrtho) {
                    float camSize = EditorGUILayout.FloatField("Size", nav.cam.orthographicSize);
                    if (EditorGUI.EndChangeCheck()) {
                        StoreObjPos();
                        nav.cam.orthographicSize = camSize;
                        nav.SendMessage("CreateFloor");
                        RestoreObjPosOrtho();
                    }
                }
                else {
                    float camFov = EditorGUILayout.Slider("Fov", nav.cam.fieldOfView, 5, 179);
                    if (EditorGUI.EndChangeCheck()) {
                        StoreObjPos();
                        nav.cam.fieldOfView = camFov;
                        nav.SendMessage("CreateFloor");
                        if (!nav.isAutoDistanceDisabled) {
                            float frustumHeight = nav.backgroundImg.bounds.size.x / nav.cam.aspect;
                            float distance = frustumHeight * 0.5f / Mathf.Tan(nav.cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                            Vector3 newPos = new Vector3(nav.cam.transform.position.x, nav.cam.transform.position.y, -distance);
                            nav.cam.transform.position = nav.posCamV3 = newPos;
                        }
                        RestoreObjPos();
                    }
                }
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + "Reset Camera", icon);
                if (GUILayout.Button(icon_con)) {
                    GUI.FocusControl(null);
                    nav.cam.transform.position = nav.posCamV3 = new Vector3(0.0f, 1f, -10f);
                    nav.cam.transform.rotation = Quaternion.Euler(new Vector3(3f, 0f, 0f));
                    nav.rotCam = new Vector3(3f, 0f, 0f);
                    nav.cam.fieldOfView = nav.fovCam = 60;
                    nav.cam.orthographicSize = nav.sizeCam = 5;
                    if (nav.setBackgroundImg != null) {
                        nav.isBackgroundImgSetView = false;
                        nav.setBackgroundImg.transform.position = nav.posBGV3 = new Vector3(0.0f, 1f, 0.0f);
                        nav.setBackgroundImg.transform.rotation = Quaternion.Euler(Vector3.zero);
                    }
                }
                EditorGUILayout.Space();
                lblCol.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField(" Background", lblCol);
                lblCol.fontStyle = FontStyle.Normal;
                string camType = "Apply camera distance";
                if (nav.isCamOrtho) {
                    camType = "Apply camera orthographic size";
                }
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "ok.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + camType, icon);
                if (GUILayout.Button(icon_con)) {
                    if (nav.backgroundImg == null) {
                        EditorUtility.DisplayDialog("Info!", "There is no background camera image", "Ok");
                    }
                    else if (nav.isCamOrtho) {
                        if (nav.backgroundImg != null) {
                            nav.cam.orthographicSize = nav.sizeCam = nav.backgroundImg.rect.height / (2f * nav.backgroundImg.pixelsPerUnit);
                            if (nav.setBackgroundImg != null) {
                                StoreObjPos();
                                nav.SendMessage("SearchBGSettings");
                                RestoreObjPosOrtho();
                            }
                        }
                    }
                    else {
                        if (nav.backgroundImg != null) {
                            StoreObjPos();
                            float frustumHeight = nav.backgroundImg.bounds.size.x / nav.cam.aspect;
                            float distance = frustumHeight * 0.5f / Mathf.Tan(nav.cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                            Vector3 newPos = new Vector3(nav.cam.transform.position.x, nav.cam.transform.position.y, -distance);
                            nav.cam.transform.position = nav.posCamV3 = newPos;
                            RestoreObjPos();
                            if (nav.setBackgroundImg != null) {
                                StoreObjPos();
                                nav.SendMessage("SearchBGSettings");
                                RestoreObjPos();
                            }
                        }
                    }
                    GUIUtility.ExitGUI();
                }
                EditorGUILayout.Space();
                EditorGUI.BeginChangeCheck();
                GameObject backgroundImgSet = EditorGUILayout.ObjectField("Image", nav.setBackgroundImg, typeof(GameObject), true) as GameObject;
                if (EditorGUI.EndChangeCheck()) {
                    nav.setBackgroundImg = backgroundImgSet;
                    if (backgroundImgSet != null) {
                        StoreObjPos();
                        nav.SendMessage("SearchBGSettings");
                        if (!nav.cam.orthographic) {
                            RestoreObjPos();
                        }
                        else {
                            RestoreObjPosOrtho();
                        }
                        nav.setBackgroundImg.SetActive(false);
                        nav.isBackgroundImgSetView = false;
                    }
                }
                if (backgroundImgSet != null) {
                    icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "remove.png", typeof(Texture2D));
                    icon_con = new GUIContent(" " + "Empty object", icon);
                    if (GUILayout.Button(icon_con)) {
                        nav.isBGManualSetting = false;
                        nav.setBackgroundImg.SetActive(false);
                        nav.isBackgroundImgSetView = false;
                        nav.setBackgroundImg = null;
                    }
                    EditorGUILayout.Space();
                    EditorGUI.BeginChangeCheck();
                    bool backgroundImgSetView = EditorGUILayout.Toggle("View background", nav.isBackgroundImgSetView, EditorStyles.toggle);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.isBackgroundImgSetView = backgroundImgSetView;
                        nav.setBackgroundImg.SetActive(backgroundImgSetView);
                    }
                    EditorGUI.BeginChangeCheck();
                    bool bgManualSettting = EditorGUILayout.Toggle("Manual setting", nav.isBGManualSetting, EditorStyles.toggle);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.isBGManualSetting = bgManualSettting;
                        if (!bgManualSettting) {
                            //
                        }
                    }
                    // character and cube do not keep their position
                    if (bgManualSettting) {
                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "centerbg.png", typeof(Texture2D));
                        icon_con = new GUIContent(" " + "Center image", icon);
                        if (GUILayout.Button(icon_con)) {
                            GUI.FocusControl(null);
                            nav.SendMessage("SearchBGSettings");
                        }
                        if (nav.isAccuracy) {
                            oldColor = GUI.color;
                            GUI.color = boxColAccuracy;
                            GUILayout.BeginVertical("box");
                        }
                        if (!nav.isBGManualSetting) {
                            EditorGUILayout.Vector3Field("Position", nav.posBGV3);
                        }
                        else {
                            EditorGUI.BeginChangeCheck();
                            Vector3 backgroundImgSetPos = EditorGUILayout.Vector3Field("Position", nav.posBGV3);
                            if (EditorGUI.EndChangeCheck()) {
                                nav.startBGPos = nav.posBGV3;
                                nav.posBGV3 = backgroundImgSetPos;
                                nav.SendMessage("PosBG");
                            }
                        }
                        if (nav.isAccuracy) {
                            GUILayout.EndVertical();
                            GUI.color = oldColor;
                        }
                        if (nav.isBtnRepeat) {
                            oldColor = GUI.color;
                            GUI.color = boxColHoldDown;
                            GUILayout.BeginVertical("box");
                        }
                        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                        EditorGUILayout.BeginHorizontal();
                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "leftX.png", typeof(Texture2D));
                        icon_con = new GUIContent("" + "", icon);
                        if (!nav.isBtnRepeat) {
                            if (GUILayout.Button(icon_con)) {
                                nav.SendMessage("bgBtnMove", 1);
                            }
                        }
                        else {
                            if (GUILayout.RepeatButton(icon_con)) {
                                nav.SendMessage("bgBtnMove", 1);
                            }
                        }
                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "rightX.png", typeof(Texture2D));
                        icon_con = new GUIContent("" + "", icon);
                        if (!nav.isBtnRepeat) {
                            if (GUILayout.Button(icon_con)) {
                                nav.SendMessage("bgBtnMove", 2);
                            }
                        }
                        else {
                            if (GUILayout.RepeatButton(icon_con)) {
                                nav.SendMessage("bgBtnMove", 2);
                            }
                        }
                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "upY.png", typeof(Texture2D));
                        icon_con = new GUIContent("" + "", icon);
                        if (!nav.isBtnRepeat) {
                            if (GUILayout.Button(icon_con)) {
                                nav.SendMessage("bgBtnMove", 3);
                            }
                        }
                        else {
                            if (GUILayout.RepeatButton(icon_con)) {
                                nav.SendMessage("bgBtnMove", 3);
                            }
                        }
                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "downY.png", typeof(Texture2D));
                        icon_con = new GUIContent("" + "", icon);
                        if (!nav.isBtnRepeat) {
                            if (GUILayout.Button(icon_con)) {
                                nav.SendMessage("bgBtnMove", 4);
                            }
                        }
                        else {
                            if (GUILayout.RepeatButton(icon_con)) {
                                nav.SendMessage("bgBtnMove", 4);
                            }
                        }
                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "forwardZ.png", typeof(Texture2D));
                        icon_con = new GUIContent("" + "", icon);
                        if (!nav.isBtnRepeat) {
                            if (GUILayout.Button(icon_con)) {
                                nav.SendMessage("bgBtnMove", 5);
                            }
                        }
                        else {
                            if (GUILayout.RepeatButton(icon_con)) {
                                nav.SendMessage("bgBtnMove", 5);
                            }
                        }
                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "backZ.png", typeof(Texture2D));
                        icon_con = new GUIContent("" + "", icon);
                        if (!nav.isBtnRepeat) {
                            if (GUILayout.Button(icon_con)) {
                                nav.SendMessage("bgBtnMove", 6);
                            }
                        }
                        else {
                            if (GUILayout.RepeatButton(icon_con)) {
                                nav.SendMessage("bgBtnMove", 6);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                        if (nav.isBtnRepeat) {
                            GUILayout.EndVertical();
                            GUI.color = oldColor;
                        }
                        if (!nav.isBGManualSetting) {
                            EditorGUILayout.Vector3Field("Rotatione", nav.RotBGV3);
                        }
                        else {
                            EditorGUI.BeginChangeCheck();
                            Vector3 backgroundImgSetRot = EditorGUILayout.Vector3Field("Rotation", nav.RotBGV3);
                            if (EditorGUI.EndChangeCheck()) {
                                nav.RotBGV3 = backgroundImgSetRot;
                                nav.setBackgroundImg.transform.rotation = Quaternion.Euler(nav.RotBGV3);
                            }
                        }
                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                        icon_con = new GUIContent(" " + "Reset background image", icon);
                        if (GUILayout.Button(icon_con)) {
                            GUI.FocusControl(null);
                            nav.isBackgroundImgSetView = false;
                            nav.setBackgroundImg.transform.position = nav.posBGV3 = new Vector3(0.0f, 1f, 0.0f);
                            nav.setBackgroundImg.transform.rotation = Quaternion.Euler(Vector3.zero);
                        }
                    }
                }
                EditorGUILayout.Space();
                lblCol.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField(" Character", lblCol);
                lblCol.fontStyle = FontStyle.Normal;
                if (!nav.isEnableChar) {
                    GUI.enabled = false;
                }
                EditorGUI.BeginChangeCheck();
                Vector3 CharPos = EditorGUILayout.Vector3Field("Position", nav.posChar);
                if (EditorGUI.EndChangeCheck()) {
                    nav.posChar = CharPos;
                    nav.character.transform.position = CharPos;
                }
                EditorGUI.BeginChangeCheck();
                Vector3 CharRot = EditorGUILayout.Vector3Field("Rotation", nav.rotChar);
                if (EditorGUI.EndChangeCheck()) {
                    nav.rotChar = CharRot;
                    nav.character.transform.rotation = Quaternion.Euler(nav.rotChar);
                }
                EditorGUI.BeginChangeCheck();
                bool lockCharScale = EditorGUILayout.Toggle("Lock scale", nav.isLockCharScale, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    nav.isLockCharScale = lockCharScale;
                }
                if (nav.isAccuracy) {
                    oldColor = GUI.color;
                    GUI.color = boxColAccuracy;
                    GUILayout.BeginVertical("box");
                }
                EditorGUI.BeginChangeCheck();
                Vector3 CharScale = EditorGUILayout.Vector3Field("Scale", nav.scaleCharV3);
                if (EditorGUI.EndChangeCheck()) {
                    nav.startCharScale = nav.scaleCharV3;
                    nav.scaleCharV3 = CharScale;
                    nav.SendMessage("ScaleChar");
                }
                if (nav.isAccuracy) {
                    GUILayout.EndVertical();
                    GUI.color = oldColor;
                }
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + "Reset Character", icon);
                if (GUILayout.Button(icon_con)) {
                    nav.character.transform.position = nav.posChar = Vector3.zero;
                    nav.character.transform.rotation = Quaternion.Euler(Vector3.zero);
                    nav.rotChar = Vector3.zero;
                    nav.character.transform.localScale = Vector3.one;
                    nav.scaleCharV3 = Vector3.one;
                }
                GUI.enabled = true;
                EditorGUILayout.Space();
                lblCol.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField(" Options", lblCol);
                lblCol.fontStyle = FontStyle.Normal;
                if (nav.isUseCube) {
                    GUILayout.BeginVertical("box");
                }
                EditorGUI.BeginChangeCheck();
                bool useCube = EditorGUILayout.Toggle("Use cube", nav.isUseCube, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    nav.isUseCube = useCube;
                    if (useCube) {
                        nav.CubeUI = Instantiate(Resources.Load("CubeUI")) as GameObject;
                        nav.CubeUI.name = "VBCubeUI";
                        InitializaCube();
                        nav.isEnableMeter = false;
                        nav.isPlaceCube = true;
                    }
                    else {
                        nav.SendMessage("RemoveCube");
                    }
                }
                if (nav.isUseCube) {
                    EditorGUI.BeginChangeCheck();
                    bool placeCube = EditorGUILayout.Toggle("Place cube", nav.isPlaceCube, EditorStyles.toggle);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.isPlaceCube = placeCube;
                        if (nav.isEnableMeter) {
                            if (nav.isCamOrtho) {
                                nav.isSetProportions = false;
                            }
                            nav.isEnableMeter = false;
                        }
                    }
                    EditorGUI.BeginChangeCheck();
                    bool transpCube = EditorGUILayout.Toggle("Transparent cube", nav.isTranspCube, EditorStyles.toggle);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.isTranspCube = transpCube;
                        if (nav.isTranspCube) {
                            nav.SendMessage("MakeTranspCube");
                        }
                        else {
                            nav.SendMessage("RevertTranspCube");
                        }
                    }
                    if (!nav.cam.orthographic) {
                        EditorGUI.BeginChangeCheck();
                        bool noKeepCubeProp = EditorGUILayout.Toggle("Don't keep proportions", nav.isNoKeepCubeProp, EditorStyles.toggle);
                        if (EditorGUI.EndChangeCheck()) {
                            nav.isNoKeepCubeProp = noKeepCubeProp;
                            if (nav.isNoKeepCubeProp) {
                                nav.CubeUI.transform.localScale = nav.scaleCubeV3;
                            }
                            else {
                                Vector3 a = nav.scaleCubeV3;
                                nav.CubeUI.transform.localScale = a * (Mathf.Abs((new Plane(nav.cam.transform.forward,
                                                                                            nav.cam.transform.position).GetDistanceToPoint(
                                                                                            nav.CubeUI.transform.position)) / nav.initialCubeDist));
                            }
                        }
                    }
                    EditorGUI.BeginChangeCheck();
                    Vector3 CubePos = EditorGUILayout.Vector3Field("Position", nav.posCube);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.posCube = CubePos;
                        nav.CubeUI.transform.position = CubePos;
                    }
                    EditorGUI.BeginChangeCheck();
                    Vector3 cubeRot = EditorGUILayout.Vector3Field("Rotation", nav.rotCube);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.rotCube = cubeRot;
                        nav.CubeUI.transform.rotation = Quaternion.Euler(nav.rotCube);
                    }
                    EditorGUI.BeginChangeCheck();
                    bool lockCubeScale = EditorGUILayout.Toggle("Lock scale", nav.isLockCubeScale, EditorStyles.toggle);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.isLockCubeScale = lockCubeScale;
                    }
                    if (nav.isAccuracy) {
                        oldColor = GUI.color;
                        GUI.color = boxColAccuracy;
                        GUILayout.BeginVertical("box");
                    }
                    EditorGUI.BeginChangeCheck();
                    Vector3 cubeScale = EditorGUILayout.Vector3Field("Scale", nav.scaleCubeV3);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.startCubeScale = nav.scaleCubeV3;
                        nav.scaleCubeV3 = cubeScale;
                        nav.SendMessage("ScaleCube");
                    }
                    if (nav.isAccuracy) {
                        GUILayout.EndVertical();
                        GUI.color = oldColor;
                    }
                    GUI.enabled = false;
                    EditorGUILayout.Vector3Field("True obj scale", nav.CubeUI.transform.localScale);
                    GUI.enabled = true;
                    icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                    icon_con = new GUIContent(" " + "Reset Cube", icon);
                    if (GUILayout.Button(icon_con)) {
                        nav.CubeUI.transform.position = nav.posCube = Vector3.zero;
                        nav.CubeUI.transform.rotation = Quaternion.Euler(Vector3.zero);
                        nav.rotCube = Vector3.zero;
                        nav.CubeUI.transform.localScale = Vector3.one;
                        nav.scaleCubeV3 = Vector3.one;
                    }
                }
                if (nav.isUseCube) {
                    GUILayout.EndVertical();
                }
                if (nav.isUseMeter) {
                    GUILayout.BeginVertical("box");
                }
                EditorGUI.BeginChangeCheck();
                bool useMeter = EditorGUILayout.Toggle("Use meter", nav.isUseMeter, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    nav.isUseMeter = useMeter;
                    if (nav.isUseMeter) {
                        nav.isPlaceCube = false;
                        nav.isEnableMeter = true;
                    }
                }
                if (nav.isUseMeter) {
                    EditorGUI.BeginChangeCheck();
                    bool enableMeter = EditorGUILayout.Toggle("Enable meter", nav.isEnableMeter, EditorStyles.toggle);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.isEnableMeter = enableMeter;
                        if (!nav.isEnableMeter) {
                            if (nav.isCamOrtho) {
                                nav.isSetProportions = false;
                            }
                        }
                        if (nav.isPlaceCube) {
                            nav.isPlaceCube = false;
                        }
                    }
                    if (nav.isCamOrtho) {
                        if (nav.isUseProportions) {
                            GUILayout.BeginVertical("box");
                        }
                        EditorGUI.BeginChangeCheck();
                        bool useProportions = EditorGUILayout.Toggle("Use proportions", nav.isUseProportions, EditorStyles.toggle);
                        if (EditorGUI.EndChangeCheck()) {
                            nav.isUseProportions = useProportions;
                        }
                        if (nav.isUseProportions) {
                            EditorGUILayout.Space();
                            //lblCol.normal.textColor = new Color(0.490f, 0.043f, 0.074f);
                            EditorGUILayout.LabelField(" Proportions", lblCol);
                            //lblCol.normal.textColor = Color.black;
                            if (!nav.isEnableMeter) {
                                GUI.enabled = false;
                            }
                            EditorGUI.BeginChangeCheck();
                            bool setProportions = EditorGUILayout.Toggle("Set proportion", nav.isSetProportions, EditorStyles.toggle);
                            if (EditorGUI.EndChangeCheck()) {
                                nav.isSetProportions = setProportions;
                            }
                            GUI.enabled = false;
                            if (nav.isSetProportions) {
                                nav.isStoreWidth = false;
                                nav.isStoreDepth = false;
                                GUI.enabled = true;
                            }
                            EditorGUI.BeginChangeCheck();
                            float valueToConvert = EditorGUILayout.FloatField("Meter value", nav.saveForMeter);
                            if (EditorGUI.EndChangeCheck()) {
                                nav.saveForMeter = valueToConvert;
                            }

                            EditorGUI.BeginChangeCheck();
                            float meterLink = EditorGUILayout.FloatField("Link to", nav.linkToMeter);
                            if (EditorGUI.EndChangeCheck()) {
                                if (meterLink <= 0f) {
                                    meterLink = nav.linkToMeter = 1f;
                                }
                                else {
                                    nav.linkToMeter = meterLink;
                                }
                            }
                            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "ok.png", typeof(Texture2D));
                            icon_con = new GUIContent(" " + "Save proportion", icon);
                            if (GUILayout.Button(icon_con)) {
                                GUI.FocusControl(null);
                                float height = 2f * nav.cam.orthographicSize;
                                float width = height * nav.cam.aspect;
                                float x = (nav.linkToMeter * width) / nav.saveForMeter;
                                nav.linkedMeter = (nav.measurementMeter * x) / width;
                                nav.isSetProportions = false;
                            }
                            GUI.enabled = false;
                            EditorGUILayout.Space();
                            if (nav.isEnableMeter) {
                                GUI.enabled = true;
                            }
                            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                            icon_con = new GUIContent(" " + "Reset Meter", icon);
                            if (GUILayout.Button(icon_con)) {
                                nav.isSetProportions = false;
                                //nav.isUseProportions = false;
                                nav.measurementMeter = 1f;
                                nav.saveForMeter = 1f;
                                nav.linkToMeter = 1f;
                                nav.linkedMeter = 1f;
                            }
                        }
                        if (nav.isUseProportions) {
                            GUILayout.EndVertical();
                        }
                    }
                    if (nav.isAutoMeter) {
                        GUILayout.BeginVertical("box");
                    }
                    EditorGUI.BeginChangeCheck();
                    bool autoMeter = EditorGUILayout.Toggle("Automatic meter", nav.isAutoMeter, EditorStyles.toggle);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.isAutoMeter = autoMeter;
                        if (!nav.isAutoMeter) {
                            nav.isLabelAutoMeter = false;
                            nav.isStoreWidth = false;
                            nav.isStoreDepth = false;
                            ResetAutoMeter();
                        }
                    }
                    if (nav.isAutoMeter) {
                        EditorGUI.BeginChangeCheck();
                        bool storeWidth = EditorGUILayout.Toggle("Save width", nav.isStoreWidth, EditorStyles.toggle);
                        if (EditorGUI.EndChangeCheck()) {
                            nav.isStoreWidth = storeWidth;
                            nav.isStoreDepth = false;
                            nav.isSetProportions = false;
                        }
                        EditorGUILayout.FloatField("Width", nav.nStoreWidth);
                        if (nav.nStoreWidth == 0) {
                            //lblCol.normal.textColor = Color.red;
                            EditorGUILayout.LabelField(" Missing", lblCol);
                            //lblCol.normal.textColor = Color.black;
                        }
                        EditorGUI.BeginChangeCheck();
                        bool storeDepth = EditorGUILayout.Toggle("Save depth", nav.isStoreDepth, EditorStyles.toggle);
                        if (EditorGUI.EndChangeCheck()) {
                            nav.isStoreDepth = storeDepth;
                            nav.isStoreWidth = false;
                            nav.isSetProportions = false;
                        }
                        EditorGUILayout.FloatField("Depth", nav.nStoreDepth);
                        if (nav.nStoreDepth == 0) {
                            //lblCol.normal.textColor = Color.red;
                            EditorGUILayout.LabelField(" Missing", lblCol);
                            //lblCol.normal.textColor = Color.black;
                        }
                        EditorGUI.BeginChangeCheck();
                        bool hideLineAutoMeter = EditorGUILayout.Toggle("Hide lines", nav.isHideLineAutoMeter, EditorStyles.toggle);
                        if (EditorGUI.EndChangeCheck()) {
                            nav.isHideLineAutoMeter = hideLineAutoMeter;
                        }
                        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                        icon_con = new GUIContent(" " + "Reset values", icon);
                        if (GUILayout.Button(icon_con)) {
                            nav.isStoreDepth = false;
                            nav.isStoreWidth = false;
                            ResetAutoMeter();
                        }
                        EditorGUI.BeginChangeCheck();
                        bool LabelAutoMeter = EditorGUILayout.Toggle("Show label", nav.isLabelAutoMeter, EditorStyles.toggle);
                        if (EditorGUI.EndChangeCheck()) {
                            nav.isLabelAutoMeter = LabelAutoMeter;
                        }
                        if (nav.isLabelAutoMeter) {
                            EditorGUI.BeginChangeCheck();
                            int size = EditorGUILayout.IntSlider("Label font size", nav.fontSizeDegrees, 6, 30);
                            if (EditorGUI.EndChangeCheck()) {
                                nav.fontSizeDegrees = size;
                                nav.labelDegrees = null;
                                SceneView.RepaintAll();
                            }
                        }
                    }
                    if (nav.isAutoMeter) {
                        GUILayout.EndVertical();
                    }
                }
                else {
                    if (nav.isCamOrtho) {
                        nav.isSetProportions = false;
                        nav.isUseProportions = false;
                        nav.isEnableMeter = false;
                    }
                    nav.isLabelAutoMeter = false;
                    nav.isStoreWidth = false;
                    nav.isStoreDepth = false;
                    nav.isAutoMeter = false;
                    ResetAutoMeter();
                }
                if (nav.isUseMeter) {
                    GUILayout.EndVertical();
                }
            }
        }
        GUI.enabled = true;
        if (nav.isUseImgInScene) {
            //lblCol.normal.textColor = Color.red;
            EditorGUILayout.LabelField(" This feature requires Background Camera", lblCol);
            //lblCol.normal.textColor = Color.black;
        }
    }

    private void StoreObjPos() {
        VB25dTK nav = (VB25dTK)target;
        storeCharV3 = nav.cam.WorldToScreenPoint(nav.character.transform.position);
        if (nav.isUseCube) {
            storeCubeV3 = nav.cam.WorldToScreenPoint(nav.CubeUI.transform.position);
        }
        if (nav.isAutoMeter) {
            if (nav.storeWidthPos[0].x != Vector3.negativeInfinity.x) {
                storeWidthPosV3[0] = nav.cam.WorldToScreenPoint(nav.storeWidthPos[0]);
                storeWidthPosV3[1] = nav.cam.WorldToScreenPoint(nav.storeWidthPos[1]);
            }
            if (nav.storeDepthPos[0].x != Vector3.negativeInfinity.x) {
                storeDepthPosV3[0] = nav.cam.WorldToScreenPoint(nav.storeDepthPos[0]);
                storeDepthPosV3[1] = nav.cam.WorldToScreenPoint(nav.storeDepthPos[1]);
            }
        }
    }

    private void RestoreObjPos() {
        VB25dTK nav = (VB25dTK)target;
        float enter = 0.0f;
        Ray ray = nav.cam.ScreenPointToRay(storeCharV3);
        if (nav.horPlane.Raycast(ray, out enter)) {
            Vector3 objPos = ray.GetPoint(enter);
            nav.character.transform.position = nav.posChar = objPos;
        }
        if (nav.isUseCube) {
            enter = 0.0f;
            ray = nav.cam.ScreenPointToRay(storeCubeV3);
            if (nav.horPlane.Raycast(ray, out enter)) {
                Vector3 objPos = ray.GetPoint(enter);
                nav.CubeUI.transform.position = objPos;
                if (!nav.isNoKeepCubeProp) {
                    Vector3 a = nav.scaleCubeV3;
                    nav.CubeUI.transform.localScale = a * (Mathf.Abs((new Plane(nav.cam.transform.forward,
                                                                                nav.cam.transform.position).GetDistanceToPoint(
                                                                                nav.CubeUI.transform.position)) / nav.initialCubeDist));
                }
            }
        }
        if (nav.isAutoMeter) {
            if (nav.storeWidthPos[0].x != Vector3.negativeInfinity.x) {
                enter = 0.0f;
                ray = nav.cam.ScreenPointToRay(storeWidthPosV3[0]);
                if (nav.horPlane.Raycast(ray, out enter)) {
                    Vector3 objPos = ray.GetPoint(enter);
                    nav.storeWidthPos[0] = objPos;
                }
                enter = 0.0f;
                ray = nav.cam.ScreenPointToRay(storeWidthPosV3[1]);
                if (nav.horPlane.Raycast(ray, out enter)) {
                    Vector3 objPos = ray.GetPoint(enter);
                    nav.storeWidthPos[1] = objPos;
                }
                nav.nStoreWidth = Mathf.Round(Vector3.Distance(nav.storeWidthPos[0], nav.storeWidthPos[1]) * 1000) / 1000;
            }
            if (nav.storeDepthPos[0].x != Vector3.negativeInfinity.x) {
                enter = 0.0f;
                ray = nav.cam.ScreenPointToRay(storeDepthPosV3[0]);
                if (nav.horPlane.Raycast(ray, out enter)) {
                    Vector3 objPos = ray.GetPoint(enter);
                    nav.storeDepthPos[0] = objPos;
                }
                enter = 0.0f;
                ray = nav.cam.ScreenPointToRay(storeDepthPosV3[1]);
                if (nav.horPlane.Raycast(ray, out enter)) {
                    Vector3 objPos = ray.GetPoint(enter);
                    nav.storeDepthPos[1] = objPos;
                }
                nav.nStoreDepth = Mathf.Round(Vector3.Distance(nav.storeDepthPos[0], nav.storeDepthPos[1]) * 1000) / 1000;
            }
        }
    }

    private void RestoreObjPosOrtho() {
        VB25dTK nav = (VB25dTK)target;
        float enter = 0.0f;
        Ray ray = nav.cam.ScreenPointToRay(storeCharV3);
        if (nav.horPlane.Raycast(ray, out enter)) {
            Vector3 objPos = ray.GetPoint(enter);
            nav.character.transform.position = nav.posChar = objPos;
        }
        if (nav.isUseCube) {
            enter = 0.0f;
            ray = nav.cam.ScreenPointToRay(storeCubeV3);
            if (nav.horPlane.Raycast(ray, out enter)) {
                Vector3 objPos = ray.GetPoint(enter);
                nav.CubeUI.transform.position = objPos;
            }
        }
        if (nav.isAutoMeter) {
            if (nav.storeWidthPos[0].x != Vector3.negativeInfinity.x) {
                enter = 0.0f;
                ray = nav.cam.ScreenPointToRay(storeWidthPosV3[0]);
                if (nav.horPlane.Raycast(ray, out enter)) {
                    Vector3 objPos = ray.GetPoint(enter);
                    nav.storeWidthPos[0] = objPos;
                }
                enter = 0.0f;
                ray = nav.cam.ScreenPointToRay(storeWidthPosV3[1]);
                if (nav.horPlane.Raycast(ray, out enter)) {
                    Vector3 objPos = ray.GetPoint(enter);
                    nav.storeWidthPos[1] = objPos;
                }
                nav.nStoreWidth = Mathf.Round(Vector3.Distance(nav.storeWidthPos[0], nav.storeWidthPos[1]) * 1000) / 1000;
            }
            if (nav.storeDepthPos[0].x != Vector3.negativeInfinity.x) {
                enter = 0.0f;
                ray = nav.cam.ScreenPointToRay(storeDepthPosV3[0]);
                if (nav.horPlane.Raycast(ray, out enter)) {
                    Vector3 objPos = ray.GetPoint(enter);
                    nav.storeDepthPos[0] = objPos;
                }
                enter = 0.0f;
                ray = nav.cam.ScreenPointToRay(storeDepthPosV3[1]);
                if (nav.horPlane.Raycast(ray, out enter)) {
                    Vector3 objPos = ray.GetPoint(enter);
                    nav.storeDepthPos[1] = objPos;
                }
                nav.nStoreDepth = Mathf.Round(Vector3.Distance(nav.storeDepthPos[0], nav.storeDepthPos[1]) * 1000) / 1000;
            }
        }
    }

    private void ResetAutoMeter() {
        VB25dTK nav = (VB25dTK)target;
        nav.nStoreWidth = 0f;
        nav.nStoreDepth = 0f;
        nav.storeWidthPos[0] = Vector3.negativeInfinity;
        nav.storeWidthPos[1] = Vector3.negativeInfinity;
        nav.storeDepthPos[0] = Vector3.negativeInfinity;
        nav.storeDepthPos[1] = Vector3.negativeInfinity;
    }

    private void InitializaCube() {
        VB25dTK nav = (VB25dTK)target;
        nav.isLockCubeScale = false;
        nav.isNoKeepCubeProp = false;
        nav.posCube = nav.CubeUI.transform.position;
        nav.rotCube = nav.CubeUI.transform.rotation.eulerAngles;
        nav.scaleCubeV3 = nav.CubeUI.transform.localScale;
        nav.initialCubeDist = Mathf.Abs(nav.cam.transform.position.z);
        nav.startCubeScale = nav.scaleCubeV3;
        nav.isTranspCube = false;
        nav.showCubeEdges = false;
    }

    private void InitializeEnv() {
        VB25dTK nav = (VB25dTK)target;
        nav.SendMessage("reset25dTKEnv");
        if (nav.cam.orthographic) {
            nav.cam.orthographic = nav.isCamOrtho = true;
            nav.spotSize = 20f;
            nav.cam.orthographicSize = nav.backgroundImg.rect.height / (2f * nav.backgroundImg.pixelsPerUnit);
            nav.sizeCam = nav.cam.orthographicSize;
        }
        else {
            nav.cam.orthographic = nav.isCamOrtho = false;
            nav.spotSize = 100f;
            nav.cam.fieldOfView = nav.fovCam = 60;
            float frustumHeight = nav.backgroundImg.bounds.size.x / nav.cam.aspect;
            float distance = frustumHeight * 0.5f / Mathf.Tan(nav.cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            Vector3 newPos = new Vector3(nav.cam.transform.position.x, nav.cam.transform.position.y, -distance);
            nav.cam.transform.position = nav.posCamV3 = newPos;
        }
        nav.character.transform.position = nav.posChar = Vector3.zero;
        nav.character.transform.rotation = Quaternion.Euler(Vector3.zero);
        nav.rotChar = Vector3.zero;
        nav.character.transform.localScale = Vector3.one;
        nav.scaleCharV3 = Vector3.one;
        nav.nSquare = 1000;
        nav.isShowFloor = true;
        nav.isPlaceCube = false;
        nav.isUseCube = false;
        nav.isSetProportions = false;
        nav.isUseProportions = false;
        nav.measurementMeter = 0f;
        nav.saveForMeter = 0f;
        nav.linkToMeter = 1f;
        nav.linkedMeter = 0f;
    }
}

