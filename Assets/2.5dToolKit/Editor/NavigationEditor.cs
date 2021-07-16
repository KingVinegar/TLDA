using UnityEngine;
using UnityEditor;

partial class VB25dTKEditor {

    private void Navigation() {
        VB25dTK nav = (VB25dTK)target;
        EditorGUILayout.Space();
        if (!Application.isPlaying && !nav.isExitingEditMode) {
            Dimensions = GUI.skin.label.CalcSize(new GUIContent("Draw in Scene View"));
            EditorGUIUtility.labelWidth = Dimensions.x + 20;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), lblStyle, GUILayout.ExpandWidth(false))) {
                showMsg[6] = !showMsg[6];
            }
            EditorGUILayout.LabelField("Prepare NavMesh", lblStyle);
            GUILayout.EndHorizontal();
            if (showMsg[6]) {
                EditorGUILayout.HelpBox(StringHelp(6), MessageType.Info);
            }
            EditorGUILayout.Space();
            bool imgBGMissing = false;
            if (GameObject.Find("VBBGCamera") != null) {
                if (nav.isUseImgBackground == false) {
                    imgBGMissing = true;
                }
            }
            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "new-area.png", typeof(Texture2D));
            icon_con = new GUIContent(" " + "Create new area", icon);
            if (GUILayout.Button(icon_con)) {
                if (!nav.isInitMissing && !imgBGMissing && !nav.isSelectPoint) {
                    nav.SendMessage("CreateNewArea");
                }
            }
            if (nav.isInitMissing || imgBGMissing) {
                //lblCol.normal.textColor = Color.red;
                EditorGUILayout.LabelField("End initialization to begin", lblCol);
            }
            if (nav.use25dtkEnv || nav.currentState != "Editing.") {
                nav.isDrawInSceneView = false;
                GUI.enabled = false;
            }
            EditorGUI.BeginChangeCheck();
            bool drawInSceneView = EditorGUILayout.Toggle("Draw in Scene View", nav.isDrawInSceneView, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck()) {
                nav.isDrawInSceneView = drawInSceneView;
            }
            GUI.enabled = true;
            if (nav.isSelectedSpot || nav.isInitMissing) {
                GUI.enabled = false;
            }
            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            nav.nGroup = EditorGUILayout.Popup("Current area", nav.nGroup, nav.listGroup.ToArray(), EditorStyles.popup);
            if (EditorGUI.EndChangeCheck()) {
                nav.SendMessage("OnChangeGroup");
            }
            string sTrash = "d_TreeEditor.Trash";
            if (nav.listGroup.Count <= 1 || nav.nGroup <= 0 || nav.isSelectPoint) {
                sTrash = "TreeEditor.Trash";
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent(sTrash), lblStyle, GUILayout.ExpandWidth(false))) {
                if (sTrash == "d_TreeEditor.Trash") {
                    nav.SendMessage("DeleteArea");
                }
            }
            GUILayout.EndHorizontal();
            GUI.enabled = true;
            if (nav.isSelectPoint) {
                GUI.enabled = false;
            }
            EditorGUI.BeginChangeCheck();
            if (nav.nGroup > 0) {
                nav.listIsWalkable[nav.nGroup - 1] = EditorGUILayout.Popup("Type", nav.listIsWalkable[nav.nGroup - 1], isWalkable, EditorStyles.popup);
            }
            else {
                GUI.enabled = false;
                EditorGUILayout.Popup("Type", nav.nGroup, nav.listGroup.ToArray(), EditorStyles.label);
                GUI.enabled = true;
            }
            if (EditorGUI.EndChangeCheck()) {
                if (nav.ListOfPointLists.list[nav.nGroup - 1].list.Count > 0) {
                    if (nav.ListOfPointLists.list[nav.nGroup - 1].list[0] == nav.ListOfPointLists.list[nav.nGroup - 1].list[nav.ListOfPointLists.list[nav.nGroup - 1].list.Count - 1]) {
                        nav.SendMessage("RemoveMeshForChange");
                        nav.SendMessage("CreateAreasToBake");
                        Debug.Log("2.5D Toolkit:\nArea type changed.");
                    }
                }
            }
            GUI.enabled = true;
            if (nav.isInitMissing) {
                GUI.enabled = false;
            }
            EditorGUI.BeginChangeCheck();
            int mouseSpotSize = EditorGUILayout.IntSlider("Spot radius", nav.radiusMouseSpot, 1, 20);
            if (EditorGUI.EndChangeCheck()) {
                nav.radiusMouseSpot = mouseSpotSize;
                SceneView.RepaintAll();
            }
            if (nav.isSelectPoint || nav.isInitMissing) {
                GUI.enabled = false;
            }
            EditorGUI.BeginChangeCheck();
            int mouseLineSize = EditorGUILayout.IntSlider("Line radius", nav.radiusMouseLine, 1, 20);
            if (EditorGUI.EndChangeCheck()) {
                nav.radiusMouseLine = mouseLineSize;
                SceneView.RepaintAll();
            }
            if (nav.isShowLineDegrees) {
                GUILayout.BeginVertical("box");
            }
            EditorGUI.BeginChangeCheck();
            bool ShowLineDegrees = EditorGUILayout.Toggle("Show line degrees", nav.isShowLineDegrees, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck()) {
                nav.isShowLineDegrees = ShowLineDegrees;
                if (!nav.isShowLineDegrees) {
                    nav.isLabelDegrees = false;
                    nav.lineDegrees = "0.0";
                    nav.lineTo90 = "0.0";
                }
            }
            if (nav.isShowLineDegrees) {
                EditorGUILayout.TextField("360 degrees", nav.lineDegrees);
                EditorGUILayout.TextField("Every 90°", nav.lineTo90);
                EditorGUI.BeginChangeCheck();
                bool LabelDegrees = EditorGUILayout.Toggle("Show label", nav.isLabelDegrees, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    nav.isLabelDegrees = LabelDegrees;
                }
                if (nav.isLabelDegrees) {
                    EditorGUI.BeginChangeCheck();
                    nav.nPlaceLabelDegrees = EditorGUILayout.Popup("Position", nav.nPlaceLabelDegrees, placeLabelDegrees, EditorStyles.popup);
                    if (EditorGUI.EndChangeCheck()) {

                    }
                }
                EditorGUI.BeginChangeCheck();
                int size = EditorGUILayout.IntSlider("Label font size", nav.fontSizeDegrees, 6, 30);
                if (EditorGUI.EndChangeCheck()) {
                    nav.fontSizeDegrees = size;
                    nav.labelDegrees = null;
                    SceneView.RepaintAll();
                }
            }
            if (nav.isShowLineDegrees) {
                GUILayout.EndVertical();
            }
            GUILayout.BeginVertical("box");
            //lblCol.normal.textColor = Color.red;
            if (!nav.isMiddleBtnDisables) {
                EditorGUILayout.LabelField(" Middle mouse button removes last point", lblCol);
            }
            else {
                EditorGUILayout.LabelField(" Middle mouse button doesn't remove last point", lblCol);
            }
            lblCol.normal.textColor = Color.black;
            EditorGUI.BeginChangeCheck();
            bool MiddleBtnDisables = EditorGUILayout.Toggle("Disable function", nav.isMiddleBtnDisables, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck()) {
                nav.isMiddleBtnDisables = MiddleBtnDisables;
            }
            GUILayout.EndVertical();
            if (nav.cam != null) {
                if (nav.floorTilemap != null || nav.cam.orthographic) {
                    GUILayout.BeginVertical("box");
                    //lblCol.normal.textColor = Color.red;
                    EditorGUILayout.LabelField(" Keep line straight", lblCol);
                    //lblCol.normal.textColor = Color.black;
                    EditorGUI.BeginChangeCheck();
                    int keepOffsetStraight = EditorGUILayout.IntSlider("Line offset", nav.keepStraightOffset, 1, 200);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.keepStraightOffset = keepOffsetStraight;
                        SceneView.RepaintAll();
                    }
                    EditorGUI.BeginChangeCheck();
                    bool keepHorzLine = EditorGUILayout.Toggle("Horizontal", nav.isKeepHorzLine, EditorStyles.toggle);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.isKeepHorzLine = keepHorzLine;
                    }
                    EditorGUI.BeginChangeCheck();
                    bool keepVertLine = EditorGUILayout.Toggle("Vertical", nav.isKeepVertLine, EditorStyles.toggle);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.isKeepVertLine = keepVertLine;
                    }
                    GUILayout.EndVertical();
                }
            }
            if (nav.nGroup <= 0) {
                GUI.enabled = false;
            }
            if (nav.moveSprite != null) {
                GUI.enabled = false;
            }
            else if (nav.nGroup > 0 && nav.ListOfPointLists.list.Count > 0) {
                if (nav.ListOfPointLists.list[nav.nGroup - 1].list.Count > 0 && !nav.isUnityNav && !nav.isMeshNav) {
                    GUI.enabled = true;
                }
                else {
                    GUI.enabled = false;
                }
            }
            if (nav.isSelectPoint) {
                GUI.enabled = false;
            }
            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "remove-last-point.png", typeof(Texture2D));
            icon_con = new GUIContent(" " + "Remove last Point", icon);
            if (GUILayout.Button(icon_con)) {
                nav.SendMessage("RemoveLastPoint");
            }
            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "remove-all-point.png", typeof(Texture2D));
            icon_con = new GUIContent(" " + "Remove ALL Point", icon);
            if (GUILayout.Button(icon_con)) {
                nav.SendMessage("RemoveAllPoint");
            }
            GUI.enabled = true;
            if (nav.nGroup <= 0) {
                GUI.enabled = false;
            }
            if (nav.nGroup > 0 && nav.ListOfPointLists.list.Count > 0) {
                if (nav.ListOfPointLists.list[nav.nGroup - 1].list.Count < 3
                    || GameObject.Find("VBNavMeshWalkable" + nav.nGroup.ToString())
                    || GameObject.Find("VBNavMeshNotWalkable" + nav.nGroup.ToString())
                    || nav.isSelectPoint
                    || nav.moveSprite != null) {
                    GUI.enabled = false;
                }
                else {
                    GUI.enabled = true;
                }
            }
            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "close-perimeter.png", typeof(Texture2D));
            icon_con = new GUIContent(" " + "Close perimeter", icon);
            if (GUILayout.Button(icon_con)) {
                if (nav.isDrawInSceneView) {
                    nav.isDrawInSceneView = false;
                }
                nav.SendMessage("ClosePerimeter");
            }
            if (nav.nGroup != 0) {
                GUI.enabled = true;
            }
            if (nav.isAccuracy) {
                oldColor = GUI.color;
                GUI.color = boxColAccuracy;
                GUILayout.BeginVertical("box");
            }
            EditorGUI.BeginChangeCheck();
            bool selectedPoint = EditorGUILayout.Toggle("Select point", nav.isSelectPoint, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck()) {
                nav.isSelectPoint = selectedPoint;
                if (selectedPoint) {
                    if (nav.ListOfPointLists.list[nav.nGroup - 1].list.Count - 1 > 0
                        && nav.ListOfPointLists.list[nav.nGroup - 1].list[nav.ListOfPointLists.list[nav.nGroup - 1].list.Count - 1] == nav.ListOfPointLists.list[nav.nGroup - 1].list[0]) {
                        //
                    }
                    else {
                        nav.isSelectPoint = false;
                        EditorUtility.DisplayDialog("Info!", "Area must be closed", "Ok");
                        GUIUtility.ExitGUI();
                    }
                }
                if (!nav.isSelectPoint) {
                    if (nav.isSelectedSpot) {
                        if (EditorUtility.DisplayDialog("Info!", "Do you want to store or discard changes?", "Save", "Cancel")) {
                            nav.SendMessage("ConfirmSpotPos");
                        }
                        else {
                            nav.ListOfPointLists.list[nav.nGroup - 1].list[nav.nSelectedSpot] = nav.oldPointPos;
                            nav.SendMessage("ConfirmSpotPos");
                        }
                        GUIUtility.ExitGUI();
                    }
                    else {
                        nav.isSelectedSpot = false;
                        nav.nSelectedSpot = -1;
                    }
                }
            }
            if (nav.isAccuracy) {
                GUILayout.EndVertical();
                GUI.color = oldColor;
            }
            if (nav.isSelectPoint) {
                if (nav.isBtnRepeat) {
                    oldColor = GUI.color;
                    GUI.color = boxColHoldDown;
                    GUILayout.BeginVertical("box");
                }
                GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                EditorGUILayout.BeginHorizontal();
                if (!nav.isSelectedSpot) {
                    GUI.enabled = false;
                }
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "leftX.png", typeof(Texture2D));
                icon_con = new GUIContent("" + "", icon);
                if (!nav.isBtnRepeat) {
                    if (GUILayout.Button(icon_con)) {
                        nav.SendMessage("spotBtnMove", 1);
                    }
                }
                else {
                    if (GUILayout.RepeatButton(icon_con)) {
                        nav.SendMessage("spotBtnMove", 1);
                    }
                }
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "rightX.png", typeof(Texture2D));
                icon_con = new GUIContent("" + "", icon);
                if (!nav.isBtnRepeat) {
                    if (GUILayout.Button(icon_con)) {
                        nav.SendMessage("spotBtnMove", 2);
                    }
                }
                else {
                    if (GUILayout.RepeatButton(icon_con)) {
                        nav.SendMessage("spotBtnMove", 2);
                    }
                }
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "forwardZ.png", typeof(Texture2D));
                icon_con = new GUIContent("" + "", icon);
                if (!nav.isBtnRepeat) {
                    if (GUILayout.Button(icon_con)) {
                        nav.SendMessage("spotBtnMove", 3);
                    }
                }
                else {
                    if (GUILayout.RepeatButton(icon_con)) {
                        nav.SendMessage("spotBtnMove", 3);
                    }
                }
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "backZ.png", typeof(Texture2D));
                icon_con = new GUIContent("" + "", icon);
                if (!nav.isBtnRepeat) {
                    if (GUILayout.Button(icon_con)) {
                        nav.SendMessage("spotBtnMove", 4);
                    }
                }
                else {
                    if (GUILayout.RepeatButton(icon_con)) {
                        nav.SendMessage("spotBtnMove", 4);
                    }
                }
                EditorGUILayout.EndHorizontal();
                GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                if (nav.isBtnRepeat) {
                    GUILayout.EndVertical();
                    GUI.color = oldColor;
                }
                GUILayout.BeginHorizontal();
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "ok.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + "Confirm position", icon);
                if (GUILayout.Button(icon_con)) {
                    nav.SendMessage("ConfirmSpotPos");
                }
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + "Restore position", icon);
                if (GUILayout.Button(icon_con)) {
                    nav.ListOfPointLists.list[nav.nGroup - 1].list[nav.nSelectedSpot] = nav.oldPointPos;
                    nav.SendMessage("ConfirmSpotPos");
                }
                GUILayout.EndHorizontal();
                GUI.enabled = true;

            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "destroy.png", typeof(Texture2D));
            icon_con = new GUIContent(" " + "Destroy all areas", icon);
            if (GUILayout.Button(icon_con)) {
                if (EditorUtility.DisplayDialog("Caution!", "All areas will be destroyed.", "OK", "Cancel")) {
                    nav.SendMessage("ResetValue");
                    nav.SendMessage("ResetGroup");
                }
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.HelpBox(StringHelp(10), MessageType.Warning);
            GUILayout.BeginHorizontal();
            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "save.png", typeof(Texture2D));
            icon_con = new GUIContent(" " + "Save areas data", icon);
            if (GUILayout.Button(icon_con)) {
                nav.SendMessage("MakeFile");
            }
            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "load.png", typeof(Texture2D));
            icon_con = new GUIContent(" " + "Load areas data", icon);
            if (GUILayout.Button(icon_con)) {
                nav.SendMessage("ReadFile");
            }
            GUILayout.EndHorizontal();
            GUI.enabled = true;
            EditorGUILayout.Space();
            if (nav.currentState == " Not editing.") {
                //lblCol.normal.textColor = Color.red;
            }
            else {
                lblCol.normal.textColor = Color.black;
            }
            EditorGUILayout.LabelField(" Current state: " + nav.currentState, lblCol);
            lblCol.normal.textColor = Color.black;
            GUI.enabled = true;
        }
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), lblStyle, GUILayout.ExpandWidth(false))) {
            showMsg[8] = !showMsg[8];
        }
        EditorGUILayout.LabelField("View Navigation", lblStyle);
        GUILayout.EndHorizontal();
        if (showMsg[8]) {
            EditorGUILayout.HelpBox(StringHelp(8), MessageType.Info);
        }
        if (!Application.isPlaying) {
            GUI.enabled = false;
        }
        else {
            GUI.enabled = true;
        }
        if (!GameObject.Find("VBMeshTK")) {
            GUI.enabled = false;
        }
        Dimensions = GUI.skin.label.CalcSize(new GUIContent("Draw in Scene View"));
        EditorGUIUtility.labelWidth = Dimensions.x + 20;
        EditorGUI.BeginChangeCheck();
        bool hideMesh = EditorGUILayout.Toggle("Hide mesh", nav.isHideMesh, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            nav.isHideMesh = hideMesh;
            nav.SendMessage("HideMesh");
        }
        if (!Application.isPlaying) {
            GUI.enabled = false;
        }
        else {
            GUI.enabled = true;
        }
        EditorGUI.BeginChangeCheck();
        bool unityNav = EditorGUILayout.Toggle("Hide areas", nav.isUnityNav, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            if (nav.character != null) {
                nav.isUnityNav = unityNav;
                nav.SendMessage("HideUnityNav");
            }
            else {
                nav.isUnityNav = false;
                EditorUtility.DisplayDialog("Info!", "Please drag character into its field.", "OK");
            }
            GUIUtility.ExitGUI();
        }
        EditorGUI.BeginChangeCheck();
        bool hidePath = EditorGUILayout.Toggle("Hide path line", nav.isHidePath, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            nav.isHidePath = hidePath;
        }
        if (nav.use25dtkEnv == true) {
            EditorGUI.BeginChangeCheck();
            bool hideFloor = EditorGUILayout.Toggle("Hide floor", !nav.isShowFloor, EditorStyles.toggle);
            if (EditorGUI.EndChangeCheck()) {
                nav.isShowFloor = !hideFloor;
            }
        }
        GUI.enabled = true;
    }
}

