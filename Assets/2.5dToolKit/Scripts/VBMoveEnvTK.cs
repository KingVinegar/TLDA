using UnityEngine;

class VBMoveEnvTK {
    private VB25dTK VB25d;
    public VBMoveEnvTK(VB25dTK target) {
        VB25d = target;
    }

    public void MoveAllObj() {
        Vector3 MoveTo = VB25d.MoveEnv;
        GameObject MoveAllEnv = GameObject.Find("VBMoveAllEnv");
        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject))) {
            if (obj.transform.parent == null) {
                if (obj.name == "VBBackgroundImgUI") {
                    obj.transform.SetParent(MoveAllEnv.transform, false);
                }
                else {
                    obj.transform.parent = MoveAllEnv.transform;
                }
            }
        }
        MoveAllEnv.transform.position = MoveTo;
        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject))) {
            if (obj.transform.parent == MoveAllEnv.transform) {
                if (obj.name == "VBBackgroundImgUI") {
                    obj.transform.SetParent(null, false);
                }
                else {
                    obj.transform.parent = null;
                }
            }
        }
        VB25d.SendMessage("ResetValue");
        VB25d.nGroup = 0;
        VB25d.listGroup.Clear();
        VB25d.listGroup.Add("none");
        VB25d.listIsWalkable.Clear();
        VB25d.labelStyle = null;
        if (VB25d.ListOfPointLists.list != null) {
            VB25d.ListOfPointLists.list.Clear();
            for (int i = 0; i < VB25d.ListOfPointLists.list.Count; i++) {
                VB25d.ListOfPointLists.list.RemoveAt(i);
            }
        }
        VB25d.isBeginWork = false;
        VB25d.use25dtkEnv = false;
    }
}

