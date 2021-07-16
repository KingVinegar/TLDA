using UnityEngine;
using UnityEditor;

partial class VB25dTKEditor {

    private void DrawSceneView() {
        VB25dTK nav = (VB25dTK)target;
        // keep active selection on the "VB25DNav" object to prevent Plane
        // from being selected during points insertion.
        if (nav.state != VB25dTK.EditingState.None) {
            if (Selection.activeGameObject != keepSelection) {
                Selection.activeGameObject = keepSelection;
            }
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(worldRay, out hitInfo)) {
                nav.mousePositionWorld = hitInfo.point;
                nav.transform.position = hitInfo.point;
                nav.hitPosPoint = hitInfo.point;
            }
            else if (nav.drawGizmosLine) {
                //nav.drawGizmosLine = false;
            }
            if (hitInfo.collider != null && nav.floor != null) {
                Event e = Event.current;
                int controlID = GUIUtility.GetControlID(FocusType.Passive);
                switch (e.GetTypeForControl(controlID)) {
                    case EventType.MouseDown:
                        GUIUtility.hotControl = controlID;
                        // left click to draw the line
                        if (Event.current.button == 0) {
                            if (nav.ListOfPointLists.list != null && nav.nGroup > 0) {
                                if (nav.ListOfPointLists.list[nav.nGroup - 1].list.Count > 0) {
                                    if (nav.ListOfPointLists.list[nav.nGroup - 1].list.IndexOf(nav.ListOfPointLists.list[nav.nGroup - 1].list[0], 1) < 0) {
                                        nav.drawGizmosLine = true;
                                        nav.previousPosPoint = nav.currentPosPoint;
                                        nav.currentPosPoint = nav.hitPosPoint;
                                        nav.SendMessage("DrawPoint");
                                    }
                                }
                                else {
                                    nav.drawGizmosLine = true;
                                    nav.previousPosPoint = nav.currentPosPoint;
                                    nav.currentPosPoint = nav.hitPosPoint;
                                    nav.SendMessage("DrawPoint");
                                }
                            }
                        }
                        // right click to show/hide line
                        else if (Event.current.button == 1) {
                            if (nav.ListOfPointLists.list != null && nav.nGroup > 0) {
                                if (nav.ListOfPointLists.list[nav.nGroup - 1].list.Count > 0) {
                                    if (nav.ListOfPointLists.list[nav.nGroup - 1].list.IndexOf(nav.ListOfPointLists.list[nav.nGroup - 1].list[0], 1) < 0) {
                                        nav.drawGizmosLine = !nav.drawGizmosLine;
                                    }
                                }
                            }
                        }
                        // middle click to delete the last inserted point
                        else if (Event.current.button == 2) {
                            nav.SendMessage("RemoveLastPoint");
                        }
                        e.Use();
                        break;
                    case EventType.MouseUp:
                        GUIUtility.hotControl = 0;
                        if (Event.current.button == 0) {
                            // for future use
                        }
                        e.Use();
                        break;
                    case EventType.MouseDrag:
                        // for future use
                        break;
                }
            }
        }
    }
}

