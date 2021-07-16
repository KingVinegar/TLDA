using UnityEngine;
using UnityEditor;

partial class VB25dTKEditor {

    private void Tilemap() {
        Dimensions = GUI.skin.label.CalcSize(new GUIContent("Orthographic"));
        EditorGUIUtility.labelWidth = Dimensions.x + 20;
        VB25dTK nav = (VB25dTK)target;
        EditorGUILayout.HelpBox(StringHelp(14), MessageType.Info);
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        GameObject tileMapGrid = EditorGUILayout.ObjectField("Grid", nav.gridTilemap, typeof(GameObject), true) as GameObject;
        if (EditorGUI.EndChangeCheck()) {
            if (tileMapGrid != null) {
                if (nav.floor != null) {
                    EditorUtility.DisplayDialog("Info!", "Please remove Floor to continue.", "OK");
                    nav.floorTilemap = null;
                }
                else if (GameObject.Find("VBBGCamera")) {
                    EditorUtility.DisplayDialog("Info!", "Please delete Background Camera to continue.", "OK");
                    nav.floorTilemap = null;
                }
                else {
                    nav.gridTilemap = tileMapGrid;
                    if (nav.gridTilemap.GetComponent<Grid>() != null) {
                        PlaceChar();
                    }
                    else {
                        EditorUtility.DisplayDialog("Info!", "Object does not appear to be a Grid.\n" +
                                                             "Check Grid component is present.", "OK");
                        nav.gridTilemap = null;
                    }
                }
            }
            else {
                EmptyGrid();
            }
            GUIUtility.ExitGUI();
        }
        if (nav.gridTilemap == null || nav.floorTilemap == null) {
            GUI.enabled = false;
        }
        EditorGUI.BeginChangeCheck();
        bool rotate90Tilemap = EditorGUILayout.Toggle("Rotate 90°", nav.isRotate90Tilemap, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            nav.isRotate90Tilemap = rotate90Tilemap;
            bool goAhead = true;
            if (nav.listGroup.Count >= 2) {
                if (!EditorUtility.DisplayDialog("Caution!", "There seems to be work in progress.\n" +
                                                             "Do you wish to continue?\n" +
                                                             "Note: areas will not be rotate.", "Ok", "Cancel")) {
                    goAhead = false;
                }
            }
            if (goAhead) {
                if (nav.isRotate90Tilemap) {
                    Rotate90();
                    PlaceChar();
                }
                else {
                    nav.SendMessage("RestoreGridTilemap");
                    PlaceChar();
                }
            }
            else {
                nav.isRotate90Tilemap = !nav.isRotate90Tilemap;
            }
            GUIUtility.ExitGUI();
        }
        GUI.enabled = true;
        EditorGUILayout.HelpBox(StringHelp(15), MessageType.Warning);
        GUI.enabled = false;
        if (nav.gridTilemap != null) {
            GUI.enabled = true;
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "remove.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Empty Grid", icon);
        if (GUILayout.Button(icon_con)) {
            EmptyGrid();
            GUIUtility.ExitGUI();
        }
        GUI.enabled = true;
        EditorGUILayout.Space();
        if (nav.gridTilemap != null) {
            EditorGUI.BeginChangeCheck();
            GameObject tileMapFloor = EditorGUILayout.ObjectField("Tilemap", nav.floorTilemap, typeof(GameObject), true) as GameObject;
            if (EditorGUI.EndChangeCheck()) {
                if (tileMapFloor != null) {
                    if (nav.floor != null) {
                        EditorUtility.DisplayDialog("Info!", "Please remove Floor to continue.", "OK");
                        nav.floorTilemap = null;
                    }
                    else if (GameObject.Find("VBBGCamera")) {
                        EditorUtility.DisplayDialog("Info!", "Please delete Background Camera to continue.", "OK");
                        nav.floorTilemap = null;
                    }
                    else {
                        nav.floorTilemap = tileMapFloor;
                        if (nav.floorTilemap.GetComponent<Renderer>() != null) {
                            // 
                        }
                        else {
                            EditorUtility.DisplayDialog("Info!", "Object does not appear to be a Tilemap.\n" +
                                                                 "Check Tilemap Renderer component is present.", "OK");
                            nav.floorTilemap = null;
                        }
                    }
                }
                else {
                    nav.floorTilemap = tileMapFloor;
                }
                GUIUtility.ExitGUI();
            }
            if (nav.floorTilemap == null) {
                GUI.enabled = false;
            }
            icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "remove.png", typeof(Texture2D));
            icon_con = new GUIContent(" " + "Empty Tilemap", icon);
            if (GUILayout.Button(icon_con)) {
                nav.floorTilemap = null;
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            if (nav.cam != null && nav.character != null) {
                lblCol.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField(" Main camera", lblCol);
                lblCol.fontStyle = FontStyle.Normal;
                EditorGUI.BeginChangeCheck();
                bool camOff = EditorGUILayout.Toggle("Camera off", !nav.cam.enabled, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    nav.cam.enabled = !camOff;
                }
                lblCol.normal.textColor = new Color(0.490f, 0.043f, 0.074f);
                if (nav.cam.orthographic) {
                    EditorGUILayout.LabelField(" Projection: Orthographic", lblCol);
                    nav.isCamOrtho = true;
                }
                else {
                    EditorGUILayout.LabelField(" Projection: Perspective", lblCol);
                    nav.isCamOrtho = false;
                }
                lblCol.normal.textColor = Color.black;
                EditorGUI.BeginChangeCheck();
                bool projectionMode = EditorGUILayout.Toggle("Orthographic", nav.isCamOrtho, EditorStyles.toggle);
                if (EditorGUI.EndChangeCheck()) {
                    if (projectionMode) {
                        nav.cam.orthographic = nav.isCamOrtho = true;
                        nav.spotSize = 20f;
                    }
                    else {
                        nav.cam.orthographic = nav.isCamOrtho = false;
                        nav.spotSize = 100f;
                        nav.cam.fieldOfView = nav.fovCam = 60;
                    }
                }
                if (nav.isAccuracy) {
                    oldColor = GUI.color;
                    GUI.color = boxColAccuracy;
                    GUILayout.BeginVertical("box");
                }
                EditorGUI.BeginChangeCheck();
                Vector3 CamPos = EditorGUILayout.Vector3Field("Position", nav.posCamV3);
                if (EditorGUI.EndChangeCheck()) {
                    nav.startCamPos = nav.posCamV3;
                    nav.posCamV3 = CamPos;
                    nav.SendMessage("PosCam");
                }
                if (nav.isAccuracy) {
                    GUILayout.EndVertical();
                    GUI.color = oldColor;
                }
                EditorGUI.BeginChangeCheck();
                Vector3 CamRot = EditorGUILayout.Vector3Field("Rotation", nav.rotCam);
                if (EditorGUI.EndChangeCheck()) {
                    nav.rotCam = CamRot;
                    nav.cam.transform.rotation = Quaternion.Euler(nav.rotCam);
                }
                EditorGUI.BeginChangeCheck();
                if (nav.isCamOrtho) {
                    float camSize = EditorGUILayout.FloatField("Size", nav.cam.orthographicSize);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.cam.orthographicSize = camSize;
                    }
                }
                else {
                    float camFov = EditorGUILayout.Slider("Fov", nav.cam.fieldOfView, 5, 179);
                    if (EditorGUI.EndChangeCheck()) {
                        nav.cam.fieldOfView = camFov;
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
                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "ok.png", typeof(Texture2D));
                icon_con = new GUIContent(" " + "Place character to grid center", icon);
                if (GUILayout.Button(icon_con)) {
                    PlaceChar();
                }
                GUI.enabled = true;
            }
            else {
                GUI.enabled = true;
                lblCol.normal.textColor = Color.red;
                EditorGUILayout.LabelField(" Missing Camera and / or Character", lblCol);
                lblCol.normal.textColor = Color.black;
            }
        }
    }

    void Rotate90() {
        VB25dTK nav = (VB25dTK)target;
        if (nav.cam != null && nav.gridTilemap != null) {
            Vector3 camRot = TransformUtils.GetInspectorRotation(nav.cam.transform);
            Vector3 gridRot = TransformUtils.GetInspectorRotation(nav.gridTilemap.transform);
            nav.gridTilemapRot = gridRot;
            nav.camTilemapRot = camRot;
            nav.gridTilemapPos = nav.gridTilemap.transform.position;
            nav.camTilemapPos = nav.posCamV3 = nav.cam.transform.position;
            nav.gridTilemap.transform.parent = nav.cam.transform;
            nav.rotCam = new Vector3(90f, camRot.y, camRot.z);
            nav.cam.transform.rotation = Quaternion.Euler(nav.rotCam);
            nav.gridTilemap.transform.parent = null;
        }
    }

    void EmptyGrid() {
        VB25dTK nav = (VB25dTK)target;
        bool restoreDefault = true;
        if (nav.isRotate90Tilemap) {
            if (!EditorUtility.DisplayDialog("Info!", "Default rotation will be restored.\n" +
                                                         "Do you wish to continue?\n" +
                                                         "Note: areas will not be rotate.", "Ok", "Cancel")) {
                restoreDefault = false;
            }
        }
        if (restoreDefault) {
            bool goAhead = true;
            if (nav.listGroup.Count >= 2) {
                if (!EditorUtility.DisplayDialog("Caution!", "There seems to be work in progress.\n" +
                                                             "Do you wish to continue?", "Ok", "Cancel")) {
                    goAhead = false;
                }
            }
            if (goAhead) {
                nav.floorTilemap = null;
                nav.SendMessage("RestoreGridTilemap");
                PlaceChar();
                nav.gridTilemap = null;
            }
        }
    }

    void PlaceChar() {
        VB25dTK nav = (VB25dTK)target;
        if (nav.character != null) {
            float charRot;
            float charPosY;
            float charPosZ;
            if (nav.isRotate90Tilemap) {
                charRot = 0f;
                charPosY = nav.gridTilemap.transform.position.y;
                charPosZ = nav.cam.transform.position.z;
            }
            else {
                charRot = -90f;
                charPosY = nav.cam.transform.position.y;
                charPosZ = nav.gridTilemap.transform.position.z;
            }
            if (nav.gridTilemap.GetComponent<Grid>() != null && nav.cam != null) {
                Vector3 rotate90 = TransformUtils.GetInspectorRotation(nav.character.transform);
                rotate90 = new Vector3(charRot, rotate90.y, rotate90.z);
                nav.character.transform.rotation = Quaternion.Euler(rotate90);
                nav.rotChar = TransformUtils.GetInspectorRotation(nav.character.transform);
                nav.character.transform.position = nav.posChar = new Vector3(nav.gridTilemap.transform.position.x, charPosY, charPosZ);
                nav.scaleCharV3 = nav.character.transform.localScale;
            }
        }
    }
}
