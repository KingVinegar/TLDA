using UnityEngine;
using UnityEditor;

partial class VB25dTKEditor {

    private void Objects() {
        VB25dTK nav = (VB25dTK)target;
        EditorGUILayout.Space();
        Dimensions = GUI.skin.label.CalcSize(new GUIContent("Object"));
        EditorGUIUtility.labelWidth = Dimensions.x + 20;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), lblStyle, GUILayout.ExpandWidth(false))) {
            showMsg[9] = !showMsg[9];
        }
        EditorGUILayout.LabelField("Place Object", lblStyle);
        GUILayout.EndHorizontal();
        if (showMsg[9]) {
            EditorGUILayout.HelpBox(StringHelp(9), MessageType.Info);
        }
        EditorGUI.BeginChangeCheck();
        Object imgSprite = EditorGUILayout.ObjectField("Object", nav.SpriteImg, typeof(Object), true) as Object;
        if (EditorGUI.EndChangeCheck()) {
            if (!nav.isInitMissing) {
                nav.SpriteImg = imgSprite;
                nav.moveSprite = null;
                if (imgSprite != null) {
                    if (GameObject.Find(nav.SpriteImg.name)) {
                        nav.moveSprite = GameObject.Find(nav.SpriteImg.name);
                        nav.SendMessage("InitializeSprite");
                        if (GameObject.Find(nav.SpriteImg.name).GetComponent<SpriteRenderer>() != null) {
                            nav.rotV3 = nav.cam.transform.rotation.eulerAngles;
                            nav.isCamRot = true;
                            nav.SendMessage("RotSprite");
                        }
                        else {
                            nav.isCamRot = false;
                        }
                        GUIUtility.ExitGUI();
                    }
                    else {
                        nav.SendMessage("EmptyObject");
                        imgSprite = null;
                        nav.SpriteImg = imgSprite;
                    }
                }
                else {
                    nav.SendMessage("EmptyObject");
                }
            }
        }
        if (nav.isInitMissing) {
            //lblCol.normal.textColor = Color.red;
            EditorGUILayout.LabelField("End initialization to begin", lblCol);
        }
        if (nav.SpriteImg == null) {
            if (nav.isHideAreas) {
                nav.SendMessage("EmptyObject");
            }
            GUI.enabled = false;
        }
        else {
            GUI.enabled = true;
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "remove.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Empty object", icon);
        if (GUILayout.Button(icon_con)) {
            hideObj = false;
            nav.SendMessage("EmptyObject");
        }
        EditorGUILayout.Space();
        Dimensions = GUI.skin.label.CalcSize(new GUIContent("More accuracy"));
        EditorGUIUtility.labelWidth = Dimensions.x + 20;
        EditorGUI.BeginChangeCheck();
        hideObj = EditorGUILayout.Toggle("Hide object", nav.isCheckedHideObj, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            if (hideObj) {
                if (nav.moveSprite.GetComponent<SpriteRenderer>() != null) {
                    nav.moveSprite.GetComponent<SpriteRenderer>().enabled = false;
                }
                else if (nav.moveSprite.GetComponent<Animator>() != null) {
                    nav.moveSprite.SetActive(false);
                }
            }
            else {
                if (nav.moveSprite.GetComponent<SpriteRenderer>() != null) {
                    nav.moveSprite.GetComponent<SpriteRenderer>().enabled = true;
                }
                else if (nav.moveSprite.GetComponent<Animator>() != null) {
                    nav.moveSprite.SetActive(true);
                }
            }
            nav.isCheckedHideObj = hideObj;
        }
        if (hideObj) {
            GUI.enabled = false;
        }
        else {
            if (nav.SpriteImg != null) {
                GUI.enabled = true;
            }
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "reset-object.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Reload object", icon);
        if (GUILayout.Button(icon_con)) {
            GUI.FocusControl(null);
            if (GameObject.Find(nav.SpriteImg.name) && nav.moveSprite != null) {
                nav.SendMessage("ReloadObject");
            }
            else {
                EditorUtility.DisplayDialog("Info!", "Object must be in scene or initialized.", "OK");
            }
            GUIUtility.ExitGUI();
        }
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        bool AutoPosObj = EditorGUILayout.Toggle("Auto position", nav.isAutoPosObj, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            nav.isAutoPosObj = AutoPosObj;
            if (nav.moveSprite != null && nav.isAutoPosObj) {
                nav.handleObjPivot = nav.moveSprite.transform.position;
            }
        }
        EditorGUILayout.HelpBox(StringHelp(16), MessageType.Info);
        EditorGUILayout.Space();
        if (nav.isAccuracy) {
            oldColor = GUI.color;
            GUI.color = boxColAccuracy;
            GUILayout.BeginVertical("box");
        }
        EditorGUI.BeginChangeCheck();
        Vector3 v3Pos = EditorGUILayout.Vector3Field("Position", nav.posV3);
        if (EditorGUI.EndChangeCheck()) {
            nav.startPos = nav.posV3;
            nav.posV3 = v3Pos;
            nav.SendMessage("PosSprite");
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
                nav.SendMessage("objBtnMove", 1);
            }
        }
        else {
            if (GUILayout.RepeatButton(icon_con)) {
                nav.SendMessage("objBtnMove", 1);
            }
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "rightX.png", typeof(Texture2D));
        icon_con = new GUIContent("" + "", icon);
        if (!nav.isBtnRepeat) {
            if (GUILayout.Button(icon_con)) {
                nav.SendMessage("objBtnMove", 2);
            }
        }
        else {
            if (GUILayout.RepeatButton(icon_con)) {
                nav.SendMessage("objBtnMove", 2);
            }
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "upY.png", typeof(Texture2D));
        icon_con = new GUIContent("" + "", icon);
        if (!nav.isBtnRepeat) {
            if (GUILayout.Button(icon_con)) {
                nav.SendMessage("objBtnMove", 3);
            }
        }
        else {
            if (GUILayout.RepeatButton(icon_con)) {
                nav.SendMessage("objBtnMove", 3);
            }
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "downY.png", typeof(Texture2D));
        icon_con = new GUIContent("" + "", icon);
        if (!nav.isBtnRepeat) {
            if (GUILayout.Button(icon_con)) {
                nav.SendMessage("objBtnMove", 4);
            }
        }
        else {
            if (GUILayout.RepeatButton(icon_con)) {
                nav.SendMessage("objBtnMove", 4);
            }
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "forwardZ.png", typeof(Texture2D));
        icon_con = new GUIContent("" + "", icon);
        if (!nav.isBtnRepeat) {
            if (GUILayout.Button(icon_con)) {
                nav.SendMessage("objBtnMove", 5);
            }
        }
        else {
            if (GUILayout.RepeatButton(icon_con)) {
                nav.SendMessage("objBtnMove", 5);
            }
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "backZ.png", typeof(Texture2D));
        icon_con = new GUIContent("" + "", icon);
        if (!nav.isBtnRepeat) {
            if (GUILayout.Button(icon_con)) {
                nav.SendMessage("objBtnMove", 6);
            }
        }
        else {
            if (GUILayout.RepeatButton(icon_con)) {
                nav.SendMessage("objBtnMove", 6);
            }
        }
        EditorGUILayout.EndHorizontal();
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
        if (nav.isBtnRepeat) {
            GUILayout.EndVertical();
            GUI.color = oldColor;
        }
        EditorGUI.BeginChangeCheck();
        Vector3 v3Rot = EditorGUILayout.Vector3Field("Rotation", nav.rotV3);
        if (EditorGUI.EndChangeCheck()) {
            nav.rotV3 = v3Rot;
            nav.SendMessage("RotSprite");
        }
        GUILayout.BeginHorizontal();
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "plus.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Scale +0.5", icon);
        if (GUILayout.Button(icon_con)) {
            nav.scaleV3 += nav.scaleV3 * 0.5f;
            nav.SendMessage("Scale05");
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "minus.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Scale -0.5", icon);
        if (GUILayout.Button(icon_con)) {
            nav.scaleV3 -= nav.scaleV3 * 0.5f;
            nav.SendMessage("Scale05");
        }
        GUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();
        bool lockScale = EditorGUILayout.Toggle("Lock scale", nav.isLockScale, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            nav.isLockScale = lockScale;
        }
        if (nav.isAccuracy) {
            oldColor = GUI.color;
            GUI.color = boxColAccuracy;
            GUILayout.BeginVertical("box");
        }
        EditorGUI.BeginChangeCheck();
        Vector3 v3Scale = EditorGUILayout.Vector3Field("Scale", nav.scaleV3);
        if (EditorGUI.EndChangeCheck()) {
            nav.startScale = nav.scaleV3;
            nav.scaleV3 = v3Scale;
            nav.SendMessage("ScaleSprite");
        }
        if (nav.isAccuracy) {
            GUILayout.EndVertical();
            GUI.color = oldColor;
        }
        if (!nav.isLockScale) {
            GUI.enabled = false;
        }
        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.BeginHorizontal();
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "leftX.png", typeof(Texture2D));
        icon_con = new GUIContent("" + "", icon);

        if (GUILayout.Button(icon_con)) {
            nav.SendMessage("ObjBtnScale", 1);
        }

        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "rightX.png", typeof(Texture2D));
        icon_con = new GUIContent("" + "", icon);

        if (GUILayout.Button(icon_con)) {
            nav.SendMessage("ObjBtnScale", 2);
        }

        EditorGUILayout.EndHorizontal();
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
        if ((nav.moveSprite != null) && (!nav.cam.orthographic)) {
            GUI.enabled = false;
            EditorGUILayout.Vector3Field("True obj scale", nav.moveSprite.transform.localScale);
            GUI.enabled = true;
        }
        if (nav.SpriteImg == null || nav.isCheckedHideObj) {
            GUI.enabled = false;
        }
        GUILayout.BeginHorizontal();
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "copy.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Copy scale", icon);
        if (GUILayout.Button(icon_con)) {
            nav.copiedScale = nav.scaleV3;
        }
        icon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath + "paste.png", typeof(Texture2D));
        icon_con = new GUIContent(" " + "Assign scale", icon);
        if (GUILayout.Button(icon_con)) {
            nav.scaleV3 = nav.copiedScale;
            bool unityBehaviourChecked = nav.isAccuracy;
            if (unityBehaviourChecked) {
                nav.isAccuracy = false;
            }
            nav.SendMessage("ScaleSprite");
            if (unityBehaviourChecked) {
                nav.isAccuracy = true;
            }
        }
        GUILayout.EndHorizontal();
        GUI.enabled = false;
        EditorGUILayout.Vector3Field("Copied scale", nav.copiedScale);
        if (nav.SpriteImg != null) {
            GUI.enabled = true;
        }
        EditorGUILayout.Space();
        Dimensions = GUI.skin.label.CalcSize(new GUIContent("Don't keep proportions"));
        EditorGUIUtility.labelWidth = Dimensions.x + 20;
        EditorGUI.BeginChangeCheck();
        bool assignCamRot = EditorGUILayout.Toggle("Assign cam rotation", nav.isCamRot, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            nav.isCamRot = assignCamRot;
            if (assignCamRot) {
                nav.rotV3 = nav.cam.transform.rotation.eulerAngles;
                nav.SendMessage("RotSprite");
            }
            else {
                nav.rotV3 = Vector3.zero;
                nav.SendMessage("RotSprite");
            }
        }
        EditorGUI.BeginChangeCheck();
        bool hidePivot = EditorGUILayout.Toggle("Hide pivot", nav.isHidePivot, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            nav.isHidePivot = hidePivot;
        }
        if (Application.isPlaying) {
            GUI.enabled = true;
        }
        EditorGUI.BeginChangeCheck();
        bool hideAreas = EditorGUILayout.Toggle("Hide areas", nav.isHideAreas, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            nav.isHideAreas = hideAreas;
            nav.SendMessage("HideAllAreas");
        }
        if (nav.cam != null && nav.SpriteImg != null) {
            if (!nav.cam.orthographic) {
                GUI.enabled = true;
            }
            else {
                GUI.enabled = false;
            }
        }
        EditorGUI.BeginChangeCheck();
        bool noKeepProp = EditorGUILayout.Toggle("Don't keep proportions", nav.isNoKeepProp, EditorStyles.toggle);
        if (EditorGUI.EndChangeCheck()) {
            nav.isNoKeepProp = noKeepProp;
            if (nav.moveSprite != null) {
                if (nav.isNoKeepProp) {
                    nav.moveSprite.transform.localScale = nav.scaleV3;
                }
                else {
                    nav.moveSprite.transform.localScale = nav.scaleV3 * (Mathf.Abs((new Plane(nav.cam.transform.forward,
                                                                                              nav.cam.transform.position).GetDistanceToPoint(
                                                                                              nav.moveSprite.transform.position)) / nav.initialDist));
                }
            }
        }
        GUI.enabled = true;
    }
}

