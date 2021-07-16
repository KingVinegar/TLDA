using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

partial class VB25dTKEditor {

    private void Settings() {
        VB25dTK nav = (VB25dTK)target;
        EditorGUILayout.Space();
        Dimensions = GUI.skin.label.CalcSize(new GUIContent("Label background"));
        EditorGUIUtility.labelWidth = Dimensions.x + 20;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), lblStyle, GUILayout.ExpandWidth(false))) {
            showMsg[2] = !showMsg[2];
        }
        EditorGUILayout.LabelField("General settings", lblStyle);
        GUILayout.EndHorizontal();
        if (showMsg[2]) {
            EditorGUILayout.HelpBox(StringHelp(2), MessageType.Info);
        }
        GUILayout.BeginHorizontal();
        if (nav.isHideGizmosSetting) {
            if (GUILayout.Button(EditorGUIUtility.IconContent("winbtn_win_max_h"), lblStyle, GUILayout.ExpandWidth(false))) {
                nav.isHideGizmosSetting = nav.treeState[2] = false;
            }
            EditorGUILayout.LabelField("Show General settings", lblStyle);
        }
        else {
            if (GUILayout.Button(EditorGUIUtility.IconContent("winbtn_win_min_h"), lblStyle, GUILayout.ExpandWidth(false))) {
                nav.isHideGizmosSetting = nav.treeState[2] = true;
            }
            EditorGUILayout.LabelField("Hide General settings", lblStyle);
        }
        GUILayout.EndHorizontal();
        if (!nav.isHideGizmosSetting) {
            EditorGUI.BeginChangeCheck();
            Texture2D cursMMouse = EditorGUILayout.ObjectField("Cursor", nav.MouseCurs, typeof(Object), false) as Texture2D;
            if (EditorGUI.EndChangeCheck()) {
                nav.MouseCurs = cursMMouse;
                nav.SendMessage("ChangeCursor");
            }
            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "remove.png", typeof(Texture2D));
            icon_con = new GUIContent(" " + "Empty cursor", icon);
            if (GUILayout.Button(icon_con)) {
                nav.MouseCurs = null;
                nav.SendMessage("ChangeCursor");
            }
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            colLine = EditorGUILayout.ColorField("Active area", nav.lineColor);
            if (EditorGUI.EndChangeCheck()) {
                nav.lineColor = colLine;
                nav.labelStyle = null;
                SceneView.RepaintAll();
            }
            EditorGUI.BeginChangeCheck();
            colInactiveLine = EditorGUILayout.ColorField("Inactive area", nav.inactiveLineColor);
            if (EditorGUI.EndChangeCheck()) {
                nav.inactiveLineColor = colInactiveLine;
                nav.labelStyle = null;
                SceneView.RepaintAll();
            }
            EditorGUI.BeginChangeCheck();
            colMouseOverSpot = EditorGUILayout.ColorField("Mouse over spot", nav.mouseOverSpotCol);
            if (EditorGUI.EndChangeCheck()) {
                nav.mouseOverSpotCol = colMouseOverSpot;
                nav.labelStyle = null;
                SceneView.RepaintAll();
            }
            EditorGUI.BeginChangeCheck();
            colFloor = EditorGUILayout.ColorField("Floor", nav.floorColor);
            if (EditorGUI.EndChangeCheck()) {
                nav.floorColor = colFloor;
            }
            EditorGUI.BeginChangeCheck();
            colBox = EditorGUILayout.ColorField("Label background", nav.boxColor);
            if (EditorGUI.EndChangeCheck()) {
                nav.boxColor = colBox;
                nav.labelStyle = null;
                SceneView.RepaintAll();
            }
            EditorGUI.BeginChangeCheck();
            colText = EditorGUILayout.ColorField("Label text", nav.textColor);
            if (EditorGUI.EndChangeCheck()) {
                nav.textColor = colText;
                nav.labelStyle = null;
                SceneView.RepaintAll();
            }
            EditorGUI.BeginChangeCheck();
            int size = EditorGUILayout.IntSlider("Label font size", nav.fontSize, 6, 30);
            if (EditorGUI.EndChangeCheck()) {
                nav.fontSize = size;
                nav.labelStyle = null;
                SceneView.RepaintAll();
            }
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), lblStyle, GUILayout.ExpandWidth(false))) {
                showMsg[3] = !showMsg[3];
            }
            EditorGUILayout.LabelField("Areas color", lblStyle);
            GUILayout.EndHorizontal();
            if (showMsg[3]) {
                EditorGUILayout.HelpBox(StringHelp(3), MessageType.Info);
            }
            EditorGUI.BeginChangeCheck();
            colWalk = EditorGUILayout.ColorField("Walkable", nav.walkCol);
            if (EditorGUI.EndChangeCheck()) {
                nav.walkCol = colWalk;
                nav.SendMessage("ChangeWalkCol");
            }
            EditorGUI.BeginChangeCheck();
            colNotWalk = EditorGUILayout.ColorField("Not walkable", nav.notWalkCol);
            if (EditorGUI.EndChangeCheck()) {
                nav.notWalkCol = colNotWalk;
                nav.SendMessage("ChangeNotWalkCol");
            }
        }
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), lblStyle, GUILayout.ExpandWidth(false))) {
            showMsg[4] = !showMsg[4];
        }
        EditorGUILayout.LabelField("Background image", lblStyle);
        GUILayout.EndHorizontal();
        if (showMsg[4]) {
            EditorGUILayout.HelpBox(StringHelp(4), MessageType.Info);
        }
        if (nav.cam != null) {
            if ((GameObject.Find("VBBGCamera")) && (nav.cam != null)) {
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "destroy.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + "Delete background camera", icon);
                if (GUILayout.Button(icon_con)) {
                    if (EditorUtility.DisplayDialog("Caution!", "Previous job will be canceled and the environment restored to default conditions.\n" +
                                                                "Do you want to continue?", "Yes", "Cancel")) {
                        ResetValues();
                        nav.backgroundImg = null;
                        nav.isUseImgBackground = false;
                        GameObject VBBGCamera = GameObject.Find("VBBGCamera");
                        DestroyImmediate(VBBGCamera);
                        if (nav.cam.orthographic) {
                            ResetOrthoCam();
                            if (nav.floor != null) {
                                ResetOrthoFloor();
                            }
                        }
                        else {
                            ResetPerspCam();
                            if (nav.floor != null) {
                                ResetPerspFloor();
                            }
                        }
                        if (nav.character != null) {
                            ResetChar();
                        }
                        if (GameObject.Find("VBBackgroundImgUI")) {
                            GameObject VBBackgroundImgUI = GameObject.Find("VBBackgroundImgUI");
                            DestroyImmediate(VBBackgroundImgUI);
                            nav.cam.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
                            nav.cam.GetComponent<Camera>().cullingMask = -1;
                        }
                    }
                    GUIUtility.ExitGUI();
                }
            }
            else {
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "backgroundcam.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + "Create background camera", icon);
                if (GUILayout.Button(icon_con)) {
                    bool createBGCam = true;
                    if (EditorUtility.DisplayDialog("Caution!", "Previous job will be canceled and the environment restored to default conditions.\n" +
                                                    "If a background image exists it will be hidden.\n" +
                                                    "Do you want to continue?", "Yes", "Cancel")) {
                        ResetValues();
                        if (nav.cam.orthographic) {
                            ResetOrthoCam();
                            if (nav.floor != null) {
                                ResetOrthoFloor();
                            }
                        }
                        else {
                            ResetPerspCam();
                            if (nav.floor != null) {
                                ResetPerspFloor();
                            }
                        }
                        if (nav.character != null) {
                            ResetChar();
                        }
                    }
                    else {
                        createBGCam = false;
                    }
                    if (createBGCam) {
                        if (nav.setBackgroundImg != null) {
                            nav.setBackgroundImg.SetActive(false);
                        }
                        nav.backgroundImg = null;
                        nav.setBackgroundImg = null;
                        nav.isUseImgInScene = false;
                        nav.cam.transform.position = new Vector3(0, 0, -10);
                        nav.cam.transform.rotation = Quaternion.Euler(Vector3.zero);
                        CreateLayer("VBBackground");
                        VBBGCamera = new GameObject("VBBGCamera", typeof(Camera));
                        VBBGCamera.GetComponent<Camera>().nearClipPlane = 0.01f;
                        VBBGCamera.GetComponent<Camera>().farClipPlane = 0.02f;
                        VBBGCamera.GetComponent<Camera>().depth = -10;
                        GameObject BackgroundImgUI = Instantiate(Resources.Load("BackgroundImgUI")) as GameObject;
                        BackgroundImgUI.name = "VBBackgroundImgUI";
                        Canvas canvas = BackgroundImgUI.GetComponent<Canvas>();
                        canvas.worldCamera = VBBGCamera.GetComponent<Camera>();
                        if (emptySlot != 100) {
                            GameObject VBBackgroundImgUI = GameObject.Find("VBBackgroundImgUI");
                            VBBackgroundImgUI.layer = emptySlot;
                            VBBGCamera.GetComponent<Camera>().cullingMask = (1 << emptySlot);
                            nav.cam.GetComponent<Camera>().cullingMask = ~(1 << emptySlot);
                        }
                        else {
                            Debug.Log("Create a new layer called VBBackground.");
                            Debug.Log("Change Culling Mask of VBBG Camera to VBBackground only.");
                            Debug.Log("Change Main Camera Culling Mask by unchecking only VBBackground.");
                            Debug.Log("Change VBBackgroundImgUI layer to VBBackground.");
                            Debug.Log("Set the Aspect Ratio of the background in Game View.");
                            EditorUtility.DisplayDialog("Caution!", "It was not possible to create / set a new layer for VBBGCamera.\n" +
                                                        "Complete the setting by following the instructions that will be printed in the Console.", "OK");
                        }
                    }
                    GUIUtility.ExitGUI();
                }
            }
            GUI.enabled = true;
            if (GameObject.Find("VBBGCamera") != null) {
                if (nav.backgroundImg == null) {
                    nav.cam.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
                    //lblCol.normal.textColor = Color.red;
                    EditorGUILayout.LabelField("Drag a image into the field", lblCol);
                    //lblCol.normal.textColor = Color.black;
                }
                VBBGCamera = GameObject.Find("VBBGCamera");
                EditorGUI.BeginChangeCheck();
                Sprite imgBackground = EditorGUILayout.ObjectField("Camera background ", nav.backgroundImg, typeof(Sprite), false) as Sprite; // Texture
                if (EditorGUI.EndChangeCheck()) {
                    VBBGCamera.GetComponent<Camera>().enabled = false;
                    nav.backgroundImg = imgBackground;
                    if (nav.backgroundImg != null) {
                        if (GameObject.Find("VBBackgroundImgUI") != null) {
                            if (GameObject.Find("VBRawImage") != null) {
                                GameObject VBRawImage = GameObject.Find("VBRawImage");
                                VBRawImage.GetComponent<RawImage>().texture = nav.backgroundImg.texture;
                                if (nav.backgroundImg != null) {
                                    nav.isUseImgBackground = true;
                                    lblCol.normal.textColor = Color.black;
                                    VBBGCamera.GetComponent<Camera>().enabled = true;
                                    if (nav.cam.GetComponent<Camera>().clearFlags != CameraClearFlags.Depth) {
                                        nav.cam.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
                                    }
                                    string firstLine = "Do you want to automatically set camera size?";
                                    string secondLine = "You can also do it later at any time by clicking the <Apply camera orthographic size> button";
                                    if (!nav.cam.orthographic) {
                                        firstLine = "Do you want to automatically set camera distance?";
                                        secondLine = "You can also do it later at any time by clicking the <Apply camera distance> button";
                                    }
                                    if (EditorUtility.DisplayDialog("Info!",
                                                                            "Set the Aspect Ratio of the background in Game View.\n" +
                                                                            firstLine + "\n" +
                                                                            secondLine,
                                                                            "Set it",
                                                                            "Don't set")) {
                                        ResetValues();
                                        nav.isUseImgInScene = false;
                                        nav.isUseImgBackground = true;
                                        if (nav.cam.orthographic) {
                                            ResetOrthoCam();
                                            nav.cam.orthographicSize = nav.backgroundImg.rect.height / (2f * nav.backgroundImg.pixelsPerUnit);

                                            if (nav.floor != null) {
                                                ResetOrthoFloor();
                                            }
                                        }
                                        else {
                                            ResetPerspCam();
                                            float frustumHeight = nav.backgroundImg.bounds.size.x / nav.cam.aspect;
                                            float distance = frustumHeight * 0.5f / Mathf.Tan(nav.cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                                            Vector3 newPos = new Vector3(nav.cam.transform.position.x, nav.cam.transform.position.y, -distance);
                                            nav.cam.transform.position = newPos;
                                            if (nav.floor != null) {
                                                ResetPerspFloor();
                                            }
                                        }
                                        if (nav.character != null) {
                                            ResetChar();
                                        }
                                    }
                                    else {
                                        ResetValues();
                                        nav.isUseImgInScene = false;
                                        nav.isUseImgBackground = true;
                                    }
                                    GUIUtility.ExitGUI();
                                }
                            }
                        }
                    }
                    else {
                        nav.isUseImgBackground = false;
                        nav.isBeginWork = false;
                        nav.use25dtkEnv = false;
                    }
                }
            }
            else {
                EditorGUI.BeginChangeCheck();
                Sprite imgBackgroundInScene = EditorGUILayout.ObjectField("Use image in scene ", nav.backgroundImg, typeof(Sprite), true) as Sprite;
                if (EditorGUI.EndChangeCheck()) {
                    nav.backgroundImg = imgBackgroundInScene;
                    if (imgBackgroundInScene != null) {
                        GameObject checkBG = GameObject.Find(imgBackgroundInScene.name);
                        if (checkBG == null) {
                            EditorUtility.DisplayDialog("Info!", "Background image must be in the scene.", "OK");
                            nav.backgroundImg = null;
                        }
                        else {
                            string firstLine = "Do you want to automatically set camera size?";
                            string secondLine = "You can also do it later at any time by clicking the <Apply camera orthographic size> button";
                            if (!nav.cam.orthographic) {
                                firstLine = "Do you want to automatically set camera distance?";
                                secondLine = "You can also do it later at any time by clicking the <Apply camera distance> button";
                            }
                            if (EditorUtility.DisplayDialog("Info!", "Set the Aspect Ratio of the background in Game View.\n" +
                                                                     firstLine + "\n" +
                                                                     secondLine,
                                                                     "Set it",
                                                                     "Don't set")) {
                                ResetValues();
                                nav.isUseImgBackground = false;
                                nav.isUseImgInScene = true;
                                nav.setBackgroundImg = GameObject.Find(nav.backgroundImg.name);
                                if (nav.cam.orthographic) {
                                    ResetOrthoCam();
                                    if (nav.backgroundImg != null) {
                                        nav.SendMessage("SimpleSearchBGSettingsSize");
                                    }
                                    if (nav.floor != null) {
                                        ResetOrthoFloor();
                                    }
                                }
                                else {
                                    ResetPerspCam();
                                    nav.SendMessage("SimpleSearchBGSettingsFOV");
                                    if (nav.floor != null) {
                                        ResetPerspFloor();
                                    }
                                }
                                if (nav.character != null) {
                                    ResetChar();
                                }
                            }
                            else {
                                ResetValues();
                                nav.isUseImgBackground = false;
                                nav.isUseImgInScene = true;
                                nav.setBackgroundImg = GameObject.Find(nav.backgroundImg.name);
                            }
                            GUIUtility.ExitGUI();
                        }
                    }
                    else {
                        nav.isUseImgInScene = false;
                        nav.setBackgroundImg = null;
                    }
                }
            }
            if (nav.isUseImgInScene || nav.isUseImgBackground) {
                lblCol.fontStyle = FontStyle.Normal;
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + "Reset to initial settings ", icon);
                if (GUILayout.Button(icon_con)) {
                    nav.SendMessage("ResetValue");
                    nav.SendMessage("ResetGroup");
                    if (nav.cam.orthographic) {
                        ResetOrthoCam();
                        if (nav.floor != null) {
                            ResetOrthoFloor();
                        }
                    }
                    else {
                        ResetPerspCam();
                        if (nav.floor != null) {
                            ResetPerspFloor();
                        }
                    }
                    if (nav.character != null) {
                        ResetChar();
                    }
                }
            }
            if (nav.isUseImgInScene || nav.isUseImgBackground) {
                string camType = "Apply camera distance";
                if (nav.cam.orthographic) {
                    camType = "Apply camera orthographic size";
                }
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "ok.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + camType, icon);
                if (GUILayout.Button(icon_con)) {
                    GUI.FocusControl(null);
                    if (nav.cam.orthographic) {
                        if (nav.backgroundImg != null) {
                            if (nav.isUseImgInScene) {
                                nav.SendMessage("SimpleSearchBGSettingsSize");
                            }
                            else {
                                nav.cam.orthographicSize = nav.backgroundImg.rect.height / (2f * nav.backgroundImg.pixelsPerUnit);
                            }
                        }
                    }
                    else {
                        if (nav.isUseImgInScene) {
                            nav.SendMessage("SimpleSearchBGSettingsFOV");
                        }
                        else {
                            float frustumHeight = nav.backgroundImg.bounds.size.x / nav.cam.aspect;
                            float distance = frustumHeight * 0.5f / Mathf.Tan(nav.cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                            Vector3 newPos = new Vector3(nav.cam.transform.position.x, nav.cam.transform.position.y, -distance);
                            nav.cam.transform.position = newPos;
                        }
                    }
                }
            }
        }
        else {
            //lblCol.normal.textColor = Color.red;
            EditorGUILayout.LabelField("Main Camera required", lblCol);
            //lblCol.normal.textColor = Color.black;
        }
        GUI.enabled = true;
    }

    private void ResetOrthoCam() {
        VB25dTK nav = (VB25dTK)target;
        nav.cam.transform.position = new Vector3(0f, 0f, -10f);
        nav.cam.transform.rotation = Quaternion.Euler(new Vector3(5f, 0, 0));
        nav.cam.orthographicSize = 5f;
    }

    private void ResetPerspCam() {
        VB25dTK nav = (VB25dTK)target;
        nav.cam.transform.position = new Vector3(0f, 0f, -10f);
        nav.cam.transform.rotation = Quaternion.Euler(Vector3.right);
        nav.cam.fieldOfView = 60f;
    }

    private void ResetOrthoFloor() {
        VB25dTK nav = (VB25dTK)target;
        nav.floor.transform.position = new Vector3(0, -1f, -5f);
        nav.floor.transform.rotation = Quaternion.Euler(Vector3.zero);
        nav.floor.transform.localScale = Vector3.one;
    }

    private void ResetPerspFloor() {
        VB25dTK nav = (VB25dTK)target;
        nav.floor.transform.position = new Vector3(0, -1f, -10f);
        nav.floor.transform.rotation = Quaternion.Euler(Vector3.zero);
        nav.floor.transform.localScale = Vector3.one;
    }

    private void ResetChar() {
        VB25dTK nav = (VB25dTK)target;
        nav.character.transform.position = Vector3.zero;
        nav.character.transform.rotation = Quaternion.Euler(Vector3.zero);
        nav.character.transform.localScale = Vector3.one;
    }

    private void ResetValues() {
        VB25dTK nav = (VB25dTK)target;
        nav.SendMessage("ResetValue");
        nav.SendMessage("ResetGroup");
        nav.isBeginWork = false;
        nav.use25dtkEnv = false;
    }
}

