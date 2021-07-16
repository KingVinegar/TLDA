using UnityEngine;
using UnityEditor;

partial class VB25dTKEditor {

    private void Advanced() {
        VB25dTK nav = (VB25dTK)target;
        EditorGUILayout.Space();
        GUI.enabled = true;
        EditorGUILayout.LabelField(" Move whole environment", lblStyle);
        string checkEnv = " Move Main camera to:";
        if (nav.isUseImgInScene) {
            checkEnv = " Move background image to:";
        }
        //lblCol.normal.textColor = Color.red;
        EditorGUILayout.LabelField(checkEnv, lblCol);
        //lblCol.normal.textColor = Color.black;
        EditorGUI.BeginChangeCheck();
        Vector3 newPos = EditorGUILayout.Vector3Field("New position", nav.MoveEnv);
        if (EditorGUI.EndChangeCheck()) {
            nav.MoveEnv = newPos;
        }
        if (nav.isUseImgInScene) {
            GUI.enabled = false;
            EditorGUILayout.Vector3Field("Actual position", nav.setBackgroundImg.transform.position);
            GUI.enabled = true;
        }
        else if (nav.cam != null) {
            GUI.enabled = false;
            EditorGUILayout.Vector3Field("Actual position", nav.cam.transform.position);
            GUI.enabled = true;
        }
        if (nav.isInitMissing && (!nav.isUseImgInScene || !nav.isUseImgBackground)) {
            GUI.enabled = false;
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "ok.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Done", icon);
        if (GUILayout.Button(icon_con)) {
            nav.SendMessage("MoveWholeEnv");
            GUIUtility.ExitGUI();
        }
        GUI.enabled = true;
        EditorGUILayout.HelpBox(StringHelp(13), MessageType.Warning);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Save data", lblStyle);
        if (nav.isInitMissing) {
            GUI.enabled = false;
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "save.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Export all data", icon);
        if (GUILayout.Button(icon_con)) {
            nav.SendMessage("ExportAllData");
        }
        GUI.enabled = true;
        EditorGUILayout.HelpBox(StringHelp(11), MessageType.Info);
    }
}