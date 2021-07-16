using UnityEngine;
using UnityEditor;

partial class VB25dTKEditor {

    private void Mesh() {
        VB25dTK nav = (VB25dTK)target;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), lblStyle, GUILayout.ExpandWidth(false))) {
            showMsg[7] = !showMsg[7];
        }
        EditorGUILayout.LabelField("Make mesh", lblStyle);
        GUILayout.EndHorizontal();
        if (showMsg[7]) {
            EditorGUILayout.HelpBox(StringHelp(7), MessageType.Info);
        }
        if (!GameObject.Find("VBNavMeshWalkable1") || nav.isUnityNav || nav.isMeshNav || nav.isSelectPoint) {
            GUI.enabled = false;
        }
        EditorGUI.BeginChangeCheck();
        colMesh = EditorGUILayout.ColorField("Mesh color", nav.meshCol);
        if (EditorGUI.EndChangeCheck()) {
            nav.meshCol = colMesh;
            nav.SendMessage("ChangeMeshCol");
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "create-mesh.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Create mesh", icon);
        if (GameObject.Find("VBMeshTK")) {
            GUI.enabled = false;
        }
        if (GUILayout.Button(icon_con)) {
            nav.isDoubleSidedMesh = true;
            nav.SendMessage("CreateMeshToExport");
        }
        GUI.enabled = true;
        if (!GameObject.Find("VBMeshTK") || nav.isSelectPoint) {
            GUI.enabled = false;
        }
        else {
            string meshState = "";
            if (nav.isDoubleSidedMesh) {
                meshState = "Double-sided";
            }
            else {
                meshState = "Reverse";
            }
            EditorGUILayout.LabelField("Mesh state: " + meshState);
        }
        GUILayout.BeginHorizontal();
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "mesh-double-sided.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Double-sided", icon);
        if (GUILayout.Button(icon_con)) {
            nav.isDoubleSidedMesh = true;
            nav.isReverseMesh = false;
            nav.SendMessage("CreateMeshToExport");
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "mesh-reverse.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Reverse", icon);
        if (GUILayout.Button(icon_con)) {
            nav.isDoubleSidedMesh = false;
            nav.isReverseMesh = !nav.isReverseMesh;
            nav.SendMessage("CreateMeshToExport");
        }
        GUILayout.EndHorizontal();
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "destroy.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Destroy mesh", icon);
        if (GUILayout.Button(icon_con)) {
            nav.SendMessage("DeleteMesh");
        }
        Dimensions = GUI.skin.label.CalcSize(new GUIContent("Save as"));
        EditorGUIUtility.labelWidth = Dimensions.x + 20;
        nav.meshName = EditorGUILayout.TextField("Save as", nav.meshName);
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "save.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Save mesh", icon);
        if (GUILayout.Button(icon_con)) {
            nav.SendMessage("SaveMesh");
        }
        Dimensions = GUI.skin.label.CalcSize(new GUIContent("Don't destroy on save"));
        EditorGUIUtility.labelWidth = Dimensions.x + 20;
        EditorGUI.BeginChangeCheck();
        bool dontDestroyOnSave = EditorGUILayout.Toggle("Don't destroy on save", nav.isDontDestroyOnSave, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            nav.isDontDestroyOnSave = dontDestroyOnSave;
        }
        GUI.enabled = true;
    }
}

