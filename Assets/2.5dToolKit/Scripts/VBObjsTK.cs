using UnityEngine;
using UnityEditor;

class VBObjsTK {
    private VB25dTK VB25d;
    public VBObjsTK(VB25dTK target) {
        VB25d = target;
    }

    public void _Scale05() {
        if (VB25d.isNoKeepProp) {
            VB25d.moveSprite.transform.localScale = VB25d.scaleV3;
        }
        else {
            if (!VB25d.cam.orthographic) {
                VB25d.moveSprite.transform.localScale = VB25d.scaleV3 * (Mathf.Abs((new Plane(VB25d.cam.transform.forward,
                                                                                    VB25d.cam.transform.position).GetDistanceToPoint(
                                                                                    VB25d.moveSprite.transform.position)) / VB25d.initialDist));
            }
            else {
                VB25d.moveSprite.transform.localScale = VB25d.scaleV3;
            }
        }
    }

    public void _RotSprite() {
        VB25d.moveSprite.transform.rotation = Quaternion.Euler(VB25d.rotV3);
    }

    public void _objBtnMove(int direction) {
        float moved = 0;
        float accuracy = 0.01f;
        Vector3 moveS = new Vector3(VB25d.moveSprite.transform.position.x,
                                    VB25d.moveSprite.transform.position.y,
                                    VB25d.moveSprite.transform.position.z);
        if (!VB25d.isAccuracy) {
            accuracy = 0.05f;
        }
        else if (VB25d.isMoreAccuracy) {
            accuracy = 0.001f;
        }
        else if (VB25d.isExtremeAccuracy) {
            accuracy = 0.0001f;
        }
        switch (direction) {
            case 1:
                moved = moveS.x - accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moved, moveS.y, moveS.z);
                break;
            case 2:
                moved = moveS.x + accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moved, moveS.y, moveS.z);
                break;
            case 3:
                moved = moveS.y + accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moveS.x, moved, moveS.z);
                break;
            case 4:
                moved = moveS.y - accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moveS.x, moved, moveS.z);
                break;
            case 5:
                moved = moveS.z + accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moveS.x, moveS.y, moved);
                break;
            case 6:
                moved = moveS.z - accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moveS.x, moveS.y, moved);
                break;
        }
        if ((!VB25d.isNoKeepProp) && (!VB25d.cam.orthographic)) {

            VB25d.moveSprite.transform.localScale = VB25d.scaleV3 * (Mathf.Abs((new Plane(VB25d.cam.transform.forward,
                                                                    VB25d.cam.transform.position).GetDistanceToPoint(
                                                                    VB25d.moveSprite.transform.position)) / VB25d.initialDist));
        }
        VB25d.posV3 = VB25d.moveSprite.transform.position;
    }

    public void _ObjBtnScale(int direction) {
        float accuracy = 0.01f;
        if (VB25d.isMoreAccuracy) {
            accuracy = 0.001f;
        }
        else if (VB25d.isExtremeAccuracy) {
            accuracy = 0.0001f;
        }
        switch (direction) {
            case 1:
                VB25d.scaleV3 -= new Vector3(accuracy, accuracy, accuracy);
                break;
            case 2:
                VB25d.scaleV3 += new Vector3(accuracy, accuracy, accuracy);
                break;
        }
        if (VB25d.isNoKeepProp) {
            VB25d.moveSprite.transform.localScale = VB25d.scaleV3;
        }
        else {
            if (!VB25d.cam.orthographic) {
                VB25d.moveSprite.transform.localScale = VB25d.scaleV3 * (Mathf.Abs((new Plane(VB25d.cam.transform.forward,
                                                                                    VB25d.cam.transform.position).GetDistanceToPoint(
                                                                                    VB25d.moveSprite.transform.position)) / VB25d.initialDist));
            }
            else {
                VB25d.moveSprite.transform.localScale = VB25d.scaleV3;
            }
        }
    }

    public void _bgBtnMove(int direction) {
        float moved = 0;
        float accuracy = 0.01f;
        Vector3 moveS = new Vector3(VB25d.setBackgroundImg.transform.position.x,
                                    VB25d.setBackgroundImg.transform.position.y,
                                    VB25d.setBackgroundImg.transform.position.z);
        if (!VB25d.isAccuracy) {
            accuracy = 0.05f;
        }
        else if (VB25d.isMoreAccuracy) {
            accuracy = 0.001f;
        }
        else if (VB25d.isExtremeAccuracy) {
            accuracy = 0.0001f;
        }
        switch (direction) {
            case 1:
                moved = moveS.x - accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moved, moveS.y, moveS.z);
                break;
            case 2:
                moved = moveS.x + accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moved, moveS.y, moveS.z);
                break;
            case 3:
                moved = moveS.y + accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moveS.x, moved, moveS.z);
                break;
            case 4:
                moved = moveS.y - accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moveS.x, moved, moveS.z);
                break;
            case 5:
                moved = moveS.z + accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moveS.x, moveS.y, moved);
                break;
            case 6:
                moved = moveS.z - accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moveS.x, moveS.y, moved);
                break;
        }
        VB25d.posBGV3 = VB25d.setBackgroundImg.transform.position;
    }

    public void _spotBtnMove(int direction) {
        float moved = 0;
        float accuracy = 0.1f;
        Vector3 moveS = VB25d.ListOfPointLists.list[VB25d.nGroup - 1].list[VB25d.nSelectedSpot];
        if (!VB25d.isAccuracy) {
            accuracy = 0.5f;
        }
        else if (VB25d.isMoreAccuracy) {
            accuracy = 0.05f;
        }
        else if (VB25d.isExtremeAccuracy) {
            accuracy = 0.0001f;
        }
        switch (direction) {
            case 1:
                moved = moveS.x - accuracy;
                VB25d.ListOfPointLists.list[VB25d.nGroup - 1].list[VB25d.nSelectedSpot] = new Vector3(Mathf.Round(moved * 1000f) / 1000f, moveS.y, moveS.z);
                break;
            case 2:
                moved = moveS.x + accuracy;
                VB25d.ListOfPointLists.list[VB25d.nGroup - 1].list[VB25d.nSelectedSpot] = new Vector3(Mathf.Round(moved * 1000f) / 1000f, moveS.y, moveS.z);
                break;
            case 3:
                moved = moveS.z + accuracy;
                VB25d.ListOfPointLists.list[VB25d.nGroup - 1].list[VB25d.nSelectedSpot] = new Vector3(moveS.x, moveS.y, Mathf.Round(moved * 1000f) / 1000f);
                break;
            case 4:
                moved = moveS.z - accuracy;
                VB25d.ListOfPointLists.list[VB25d.nGroup - 1].list[VB25d.nSelectedSpot] = new Vector3(moveS.x, moveS.y, Mathf.Round(moved * 1000f) / 1000f);
                break;
        }
        if (VB25d.nSelectedSpot == 0) {
            VB25d.ListOfPointLists.list[VB25d.nGroup - 1].list[VB25d.ListOfPointLists.list[VB25d.nGroup - 1].list.Count - 1] = 
                VB25d.ListOfPointLists.list[VB25d.nGroup - 1].list[VB25d.nSelectedSpot];
        }
    }

    public void _PosSprite() {
        if (!VB25d.isAccuracy) {
            VB25d.moveSprite.transform.position = VB25d.posV3;
            if ((!VB25d.isNoKeepProp) && (!VB25d.cam.orthographic)) {
                VB25d.moveSprite.transform.localScale = VB25d.scaleV3 * (Mathf.Abs((new Plane(VB25d.cam.transform.forward,
                                                                                              VB25d.cam.transform.position).GetDistanceToPoint(
                                                                                              VB25d.moveSprite.transform.position)) / VB25d.initialDist));
            }
        }
        else {
            float moved;
            float accuracy = 0.01f;
            if (VB25d.isMoreAccuracy) {
                accuracy = 0.001f;
            }
            else if (VB25d.isExtremeAccuracy) {
                accuracy = 0.0001f;
            }
            Vector3 moveS = new Vector3(VB25d.moveSprite.transform.position.x,
                                        VB25d.moveSprite.transform.position.y,
                                        VB25d.moveSprite.transform.position.z);
            if (VB25d.posV3.x > VB25d.startPos.x) {
                moved = moveS.x + accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moved, moveS.y, moveS.z);
            }
            else if (VB25d.posV3.y > VB25d.startPos.y) {
                moved = moveS.y + accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moveS.x, moved, moveS.z);
            }
            else if (VB25d.posV3.z > VB25d.startPos.z) {
                moved = moveS.z + accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moveS.x, moveS.y, moved);
                if ((!VB25d.isNoKeepProp) && (!VB25d.cam.orthographic)) {
                    VB25d.moveSprite.transform.localScale = VB25d.scaleV3 * (Mathf.Abs((new Plane(VB25d.cam.transform.forward,
                                                                                                  VB25d.cam.transform.position).GetDistanceToPoint(
                                                                                                  VB25d.moveSprite.transform.position)) / VB25d.initialDist));
                }
            }
            else if (VB25d.posV3.x < VB25d.startPos.x) {
                moved = moveS.x - accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moved, moveS.y, moveS.z);
            }
            else if (VB25d.posV3.y < VB25d.startPos.y) {
                moved = moveS.y - accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moveS.x, moved, moveS.z);
            }
            else if (VB25d.posV3.z < VB25d.startPos.z) {
                moved = moveS.z - accuracy;
                VB25d.moveSprite.transform.position = new Vector3(moveS.x, moveS.y, moved);
                if ((!VB25d.isNoKeepProp) && (!VB25d.cam.orthographic)) {
                    VB25d.moveSprite.transform.localScale = VB25d.scaleV3 * (Mathf.Abs((new Plane(VB25d.cam.transform.forward,
                                                                                                  VB25d.cam.transform.position).GetDistanceToPoint(
                                                                                                  VB25d.moveSprite.transform.position)) / VB25d.initialDist));
                }
            }
            VB25d.posV3 = VB25d.moveSprite.transform.position;
        }
    }

    public void _ScaleSprite() {
        if (!VB25d.isAccuracy) {
            if (VB25d.isLockScale) {
                float value = 0;
                if (VB25d.scaleV3.x > VB25d.startScale.x || VB25d.scaleV3.x < VB25d.startScale.x) {
                    value = VB25d.scaleV3.x;
                }
                else if (VB25d.scaleV3.y > VB25d.startScale.y || VB25d.scaleV3.y < VB25d.startScale.y) {
                    value = VB25d.scaleV3.y;
                }
                else if (VB25d.scaleV3.z > VB25d.startScale.z || VB25d.scaleV3.z < VB25d.startScale.z) {
                    value = VB25d.scaleV3.z;
                }
                VB25d.scaleV3 = new Vector3(value, value, value);
            }
            if (VB25d.isNoKeepProp) {
                VB25d.moveSprite.transform.localScale = VB25d.scaleV3;
            }
            else {
                if (!VB25d.cam.orthographic) {
                    VB25d.moveSprite.transform.localScale = VB25d.scaleV3 * (Mathf.Abs((new Plane(VB25d.cam.transform.forward,
                                                                                                  VB25d.cam.transform.position).GetDistanceToPoint(
                                                                                                  VB25d.moveSprite.transform.position)) / VB25d.initialDist));
                }
                else {
                    VB25d.moveSprite.transform.localScale = VB25d.scaleV3;
                }
            }
        }
        else {
            float accuracy = 0.01f;
            if (VB25d.isMoreAccuracy) {
                accuracy = 0.001f;
            }
            else if (VB25d.isExtremeAccuracy) {
                accuracy = 0.0001f;
            }
            if (VB25d.isLockScale) {
                if (VB25d.scaleV3.x > VB25d.startScale.x
                     || VB25d.scaleV3.y > VB25d.startScale.y
                     || VB25d.scaleV3.z > VB25d.startScale.z) {
                    VB25d.scaleV3 = VB25d.startScale;
                    VB25d.scaleV3 += new Vector3(accuracy, accuracy, accuracy);
                }
                else if (VB25d.scaleV3.x < VB25d.startScale.x
               || VB25d.scaleV3.y < VB25d.startScale.y
               || VB25d.scaleV3.z < VB25d.startScale.z) {
                    VB25d.scaleV3 = VB25d.startScale;
                    VB25d.scaleV3 -= new Vector3(accuracy, accuracy, accuracy);
                }
            }
            else {
                if (VB25d.scaleV3.x > VB25d.startScale.x) {
                    VB25d.scaleV3 = VB25d.startScale;
                    VB25d.scaleV3 = new Vector3(VB25d.scaleV3.x + accuracy, VB25d.scaleV3.y, VB25d.scaleV3.z);
                }
                else if (VB25d.scaleV3.y > VB25d.startScale.y) {
                    VB25d.scaleV3 = VB25d.startScale;
                    VB25d.scaleV3 = new Vector3(VB25d.scaleV3.x, VB25d.scaleV3.y + accuracy, VB25d.scaleV3.z);
                }
                else if (VB25d.scaleV3.z > VB25d.startScale.z) {
                    VB25d.scaleV3 = VB25d.startScale;
                    VB25d.scaleV3 = new Vector3(VB25d.scaleV3.x, VB25d.scaleV3.y, VB25d.scaleV3.z + accuracy);
                }
                else if (VB25d.scaleV3.x < VB25d.startScale.x) {
                    VB25d.scaleV3 = VB25d.startScale;
                    VB25d.scaleV3 = new Vector3(VB25d.scaleV3.x - accuracy, VB25d.scaleV3.y, VB25d.scaleV3.z);
                }
                else if (VB25d.scaleV3.y < VB25d.startScale.y) {
                    VB25d.scaleV3 = VB25d.startScale;
                    VB25d.scaleV3 = new Vector3(VB25d.scaleV3.x, VB25d.scaleV3.y - accuracy, VB25d.scaleV3.z);
                }
                else if (VB25d.scaleV3.z < VB25d.startScale.z) {
                    VB25d.scaleV3 = VB25d.startScale;
                    VB25d.scaleV3 = new Vector3(VB25d.scaleV3.x, VB25d.scaleV3.y, VB25d.scaleV3.z - accuracy);
                }
            }
            if (VB25d.isNoKeepProp) {
                VB25d.moveSprite.transform.localScale = VB25d.scaleV3;
            }
            else {
                if (!VB25d.cam.orthographic) {
                    VB25d.moveSprite.transform.localScale = VB25d.scaleV3 * (Mathf.Abs((new Plane(VB25d.cam.transform.forward,
                                                                                                  VB25d.cam.transform.position).GetDistanceToPoint(
                                                                                                  VB25d.moveSprite.transform.position)) / VB25d.initialDist));
                }
                else {
                    VB25d.moveSprite.transform.localScale = VB25d.scaleV3;
                }
            }
        }
    }

    public void _InitializeSprite() {
        VB25d.isCamRot = false;
        VB25d.isNoKeepProp = false;
        VB25d.moveSprite.transform.position = VB25d.handleObjPivot = VB25d.posV3 = new Vector3(0f, 0f, VB25d.cam.farClipPlane);
        VB25d.moveSprite.transform.localScale = Vector3.one;
        VB25d.scaleV3 = VB25d.moveSprite.transform.localScale;
        VB25d.rotV3 = VB25d.moveSprite.transform.rotation.eulerAngles;
        VB25d.initialDist = Mathf.Abs(VB25d.cam.transform.position.z);
        VB25d.startScale = VB25d.scaleV3;
        if (VB25d.moveSprite.GetComponent<SpriteRenderer>() != null) {
            VB25d.isCamRot = true;
        }
    }

    public void _ResetSprite() {
        VB25d.isCamRot = false;
        VB25d.isNoKeepProp = false;
        VB25d.moveSprite.transform.position = VB25d.posV3 = Vector3.zero;
        VB25d.moveSprite.transform.localScale = VB25d.scaleV3 = new Vector3(1f, 1f, 1f);
        VB25d.rotV3 = Vector3.zero;
        VB25d.moveSprite.transform.rotation = Quaternion.Euler(Vector3.zero);
        if (VB25d.moveSprite.GetComponent<SpriteRenderer>() != null) {
            VB25d.isCamRot = true;
        }
    }

    public void _EmptyObject() {
        VB25d.moveSprite = null;
        VB25d.SpriteImg = null;
        VB25d.posV3 = Vector3.zero;
        VB25d.rotV3 = Vector3.zero;
        VB25d.scaleV3 = Vector3.one;
        VB25d.isHideAreas = false;
        VB25d.isCamRot = false;
        VB25d.isCheckedHideObj = false;
        VB25d.HideAllAreas();
        VB25d.isAutoPosObj = false;
        VB25d.handleObjPivot = Vector3.negativeInfinity;
    }

    /* [Temporarily disabled]
    public void _fovsizeCam(int camType) {
        if (!VB25d.isAccuracy) {
            if (camType == 1) {
                VB25d.cam.orthographicSize = VB25d.sizeCam;
            }
            else {
                VB25d.cam.fieldOfView = VB25d.fovCam;
            }
        }
        else {
            float moved = 0;
            float accuracy = 0.01f;
            if (VB25d.isMoreAccuracy) {
                accuracy = 0.001f;
            }
            if (camType == 1) {
                float moveS = VB25d.cam.orthographicSize;
                if (VB25d.sizeCam > VB25d.startCamSize) {
                    moved = moveS + accuracy;
                }
                else if (VB25d.sizeCam < VB25d.startCamSize) {
                    moved = moveS - accuracy;
                }
                VB25d.cam.orthographicSize = moved;
                VB25d.sizeCam = VB25d.cam.orthographicSize;
            }
            else {
                float moveS = VB25d.cam.fieldOfView;
                if (VB25d.fovCam > VB25d.startCamFov) {
                    moved = moveS + accuracy;
                }
                else if (VB25d.fovCam < VB25d.startCamFov) {
                    moved = moveS - accuracy;
                }
                VB25d.cam.fieldOfView = moved;
                VB25d.fovCam = VB25d.cam.fieldOfView;
            }
        }
    }
    */

    public void _PosCam() {
        if (!VB25d.isAccuracy) {
            VB25d.cam.transform.position = VB25d.posCamV3;
        }
        else {
            float moved;
            float accuracy = 0.01f;
            if (VB25d.isMoreAccuracy) {
                accuracy = 0.001f;
            }
            else if (VB25d.isExtremeAccuracy) {
                accuracy = 0.0001f;
            }
            Vector3 moveS = new Vector3(VB25d.cam.transform.position.x,
                                        VB25d.cam.transform.position.y,
                                        VB25d.cam.transform.position.z);
            if (VB25d.posCamV3.x > VB25d.startCamPos.x) {
                moved = moveS.x + accuracy;
                VB25d.cam.transform.position = new Vector3(moved, moveS.y, moveS.z);
            }
            else if (VB25d.posCamV3.y > VB25d.startCamPos.y) {
                moved = moveS.y + accuracy;
                VB25d.cam.transform.position = new Vector3(moveS.x, moved, moveS.z);
            }
            else if (VB25d.posCamV3.z > VB25d.startCamPos.z) {
                moved = moveS.z + accuracy;
                VB25d.cam.transform.position = new Vector3(moveS.x, moveS.y, moved);
            }
            else if (VB25d.posCamV3.x < VB25d.startCamPos.x) {
                moved = moveS.x - accuracy;
                VB25d.cam.transform.position = new Vector3(moved, moveS.y, moveS.z);
            }
            else if (VB25d.posCamV3.y < VB25d.startCamPos.y) {
                moved = moveS.y - accuracy;
                VB25d.cam.transform.position = new Vector3(moveS.x, moved, moveS.z);
            }
            else if (VB25d.posCamV3.z < VB25d.startCamPos.z) {
                moved = moveS.z - accuracy;
                VB25d.cam.transform.position = new Vector3(moveS.x, moveS.y, moved);
            }
            VB25d.posCamV3 = VB25d.cam.transform.position;
        }
    }

    public void _RotCam() {
        if (!VB25d.isAccuracy) {
            VB25d.cam.transform.rotation = Quaternion.Euler(VB25d.rotCam);
        }
        else {
            float moved;
            float accuracy = 0.01f;
            if (VB25d.isMoreAccuracy) {
                accuracy = 0.001f;
            }
            else if (VB25d.isExtremeAccuracy) {
                accuracy = 0.0001f;
            }
            Vector3 moveS = new Vector3(VB25d.cam.transform.rotation.eulerAngles.x,
                                        VB25d.cam.transform.rotation.eulerAngles.y,
                                        VB25d.cam.transform.rotation.eulerAngles.z);
            if (VB25d.rotCam.x > VB25d.startCamRot.x) {
                moved = moveS.x + accuracy;
                VB25d.cam.transform.rotation = Quaternion.Euler(new Vector3(moved, moveS.y, moveS.z));
            }
            else if (VB25d.rotCam.y > VB25d.startCamRot.y) {
                moved = moveS.y + accuracy;
                VB25d.cam.transform.rotation = Quaternion.Euler(new Vector3(moveS.x, moved, moveS.z));
            }
            else if (VB25d.rotCam.z > VB25d.startCamRot.z) {
                moved = moveS.z + accuracy;
                VB25d.cam.transform.rotation = Quaternion.Euler(new Vector3(moveS.x, moveS.y, moved));
            }
            else if (VB25d.rotCam.x < VB25d.startCamRot.x) {
                moved = moveS.x - accuracy;
                VB25d.cam.transform.rotation = Quaternion.Euler(new Vector3(moved, moveS.y, moveS.z));
            }
            else if (VB25d.rotCam.y < VB25d.startCamRot.y) {
                moved = moveS.y - accuracy;
                VB25d.cam.transform.rotation = Quaternion.Euler(new Vector3(moveS.x, moved, moveS.z));
            }
            else if (VB25d.rotCam.z < VB25d.startCamRot.z) {
                moved = moveS.z - accuracy;
                VB25d.cam.transform.rotation = Quaternion.Euler(new Vector3(moveS.x, moveS.y, moved));
            }
            VB25d.rotCam = TransformUtils.GetInspectorRotation(VB25d.cam.transform);
        }
    }

    public void _MoveFloor() {
        if (!VB25d.isAccuracy) {
            // 
        }
        else {
            float accuracy = 0.01f;
            if (VB25d.isMoreAccuracy) {
                accuracy = 0.001f;
            }
            else if (VB25d.isExtremeAccuracy) {
                accuracy = 0.0001f;
            }
            if (VB25d.vert > VB25d.startFloorPos) {
                VB25d.vert = VB25d.startFloorPos + accuracy;
            }
            else if (VB25d.vert < VB25d.startFloorPos) {
                VB25d.vert = VB25d.startFloorPos - accuracy;
            }
        }
    }

    public void _PosBG() {
        if (!VB25d.isAccuracy) {
            VB25d.setBackgroundImg.transform.position = VB25d.posBGV3;
        }
        else {
            float moved;
            float accuracy = 0.01f;
            if (VB25d.isMoreAccuracy) {
                accuracy = 0.001f;
            }
            else if (VB25d.isExtremeAccuracy) {
                accuracy = 0.0001f;
            }
            Vector3 moveS = new Vector3(VB25d.setBackgroundImg.transform.position.x,
                                        VB25d.setBackgroundImg.transform.position.y,
                                        VB25d.setBackgroundImg.transform.position.z);
            if (VB25d.posBGV3.x > VB25d.startBGPos.x) {
                moved = moveS.x + accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moved, moveS.y, moveS.z);
            }
            else if (VB25d.posBGV3.y > VB25d.startBGPos.y) {
                moved = moveS.y + accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moveS.x, moved, moveS.z);
            }
            else if (VB25d.posBGV3.z > VB25d.startBGPos.z) {
                moved = moveS.z + accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moveS.x, moveS.y, moved);
            }
            else if (VB25d.posBGV3.x < VB25d.startBGPos.x) {
                moved = moveS.x - accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moved, moveS.y, moveS.z);
            }
            else if (VB25d.posBGV3.y < VB25d.startBGPos.y) {
                moved = moveS.y - accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moveS.x, moved, moveS.z);
            }
            else if (VB25d.posBGV3.z < VB25d.startBGPos.z) {
                moved = moveS.z - accuracy;
                VB25d.setBackgroundImg.transform.position = new Vector3(moveS.x, moveS.y, moved);
            }
            VB25d.posBGV3 = VB25d.setBackgroundImg.transform.position;
        }
    }

    public void _ScaleChar() {
        if (!VB25d.isAccuracy) {
            if (VB25d.isLockCharScale || VB25d.lockCharScaleAuto) {
                float value = 0;
                if (VB25d.scaleCharV3.x > VB25d.startCharScale.x || VB25d.scaleCharV3.x < VB25d.startCharScale.x) {
                    value = VB25d.scaleCharV3.x;
                }
                else if (VB25d.scaleCharV3.y > VB25d.startCharScale.y || VB25d.scaleCharV3.y < VB25d.startCharScale.y) {
                    value = VB25d.scaleCharV3.y;
                }
                else if (VB25d.scaleCharV3.z > VB25d.startCharScale.z || VB25d.scaleCharV3.z < VB25d.startCharScale.z) {
                    value = VB25d.scaleCharV3.z;
                }
                VB25d.scaleCharV3 = new Vector3(value, value, value);
            }
            VB25d.lockCharScaleAuto = false;
            VB25d.character.transform.localScale = VB25d.scaleCharV3;
        }
        else {
            float accuracy = 0.01f;
            if (VB25d.isMoreAccuracy) {
                accuracy = 0.001f;
            }
            else if (VB25d.isExtremeAccuracy) {
                accuracy = 0.0001f;
            }
            if (VB25d.isLockCharScale || VB25d.lockCharScaleAuto) {
                if (VB25d.scaleCharV3.x > VB25d.startCharScale.x
                     || VB25d.scaleCharV3.y > VB25d.startCharScale.y
                     || VB25d.scaleCharV3.z > VB25d.startCharScale.z) {
                    VB25d.scaleCharV3 = VB25d.startCharScale;
                    VB25d.scaleCharV3 += new Vector3(accuracy, accuracy, accuracy);
                }
                else if (VB25d.scaleCharV3.x < VB25d.startCharScale.x
                          || VB25d.scaleCharV3.y < VB25d.startCharScale.y
                          || VB25d.scaleCharV3.z < VB25d.startCharScale.z) {
                    VB25d.scaleCharV3 = VB25d.startCharScale;
                    VB25d.scaleCharV3 -= new Vector3(accuracy, accuracy, accuracy);
                }
            }
            else {
                if (VB25d.scaleCharV3.x > VB25d.startCharScale.x) {
                    VB25d.scaleCharV3 = VB25d.startCharScale;
                    VB25d.scaleCharV3 = new Vector3(VB25d.scaleCharV3.x + accuracy, VB25d.scaleCharV3.y, VB25d.scaleCharV3.z);
                }
                else if (VB25d.scaleCharV3.y > VB25d.startCharScale.y) {
                    VB25d.scaleCharV3 = VB25d.startCharScale;
                    VB25d.scaleCharV3 = new Vector3(VB25d.scaleCharV3.x, VB25d.scaleCharV3.y + accuracy, VB25d.scaleCharV3.z);
                }
                else if (VB25d.scaleCharV3.z > VB25d.startCharScale.z) {
                    VB25d.scaleCharV3 = VB25d.startCharScale;
                    VB25d.scaleCharV3 = new Vector3(VB25d.scaleCharV3.x, VB25d.scaleCharV3.y, VB25d.scaleCharV3.z + accuracy);
                }
                else if (VB25d.scaleCharV3.x < VB25d.startCharScale.x) {
                    VB25d.scaleCharV3 = VB25d.startCharScale;
                    VB25d.scaleCharV3 = new Vector3(VB25d.scaleCharV3.x - accuracy, VB25d.scaleCharV3.y, VB25d.scaleCharV3.z);
                }
                else if (VB25d.scaleCharV3.y < VB25d.startCharScale.y) {
                    VB25d.scaleCharV3 = VB25d.startCharScale;
                    VB25d.scaleCharV3 = new Vector3(VB25d.scaleCharV3.x, VB25d.scaleCharV3.y - accuracy, VB25d.scaleCharV3.z);
                }
                else if (VB25d.scaleCharV3.z < VB25d.startCharScale.z) {
                    VB25d.scaleCharV3 = VB25d.startCharScale;
                    VB25d.scaleCharV3 = new Vector3(VB25d.scaleCharV3.x, VB25d.scaleCharV3.y, VB25d.scaleCharV3.z - accuracy);
                }
            }
            VB25d.lockCharScaleAuto = false;
            VB25d.character.transform.localScale = VB25d.scaleCharV3;
        }
    }

    public void _ScaleCube() {
        if (!VB25d.isAccuracy) {
            if (VB25d.isLockCubeScale) {
                float value = 0;
                if (VB25d.scaleCubeV3.x > VB25d.startCubeScale.x || VB25d.scaleCubeV3.x < VB25d.startCubeScale.x) {
                    value = VB25d.scaleCubeV3.x;
                }
                else if (VB25d.scaleCubeV3.y > VB25d.startCubeScale.y || VB25d.scaleCubeV3.y < VB25d.startCubeScale.y) {
                    value = VB25d.scaleCubeV3.y;
                }
                else if (VB25d.scaleCubeV3.z > VB25d.startCubeScale.z || VB25d.scaleCubeV3.z < VB25d.startCubeScale.z) {
                    value = VB25d.scaleCubeV3.z;
                }
                VB25d.scaleCubeV3 = new Vector3(value, value, value);
            }
            if (VB25d.isNoKeepCubeProp) {
                VB25d.CubeUI.transform.localScale = VB25d.scaleCubeV3;
            }
            else {
                if (!VB25d.cam.orthographic) {
                    VB25d.CubeUI.transform.localScale = VB25d.scaleCubeV3 * (Mathf.Abs((new Plane(VB25d.cam.transform.forward,
                                                                                                  VB25d.cam.transform.position).GetDistanceToPoint(
                                                                                                  VB25d.CubeUI.transform.position)) / VB25d.initialCubeDist));
                }
                else {
                    VB25d.CubeUI.transform.localScale = VB25d.scaleCubeV3;
                }
            }
        }
        else {
            float accuracy = 0.01f;
            if (VB25d.isMoreAccuracy) {
                accuracy = 0.001f;
            }
            else if (VB25d.isExtremeAccuracy) {
                accuracy = 0.0001f;
            }
            if (VB25d.isLockCubeScale) {
                if (VB25d.scaleCubeV3.x > VB25d.startCubeScale.x
                     || VB25d.scaleCubeV3.y > VB25d.startCubeScale.y
                     || VB25d.scaleCubeV3.z > VB25d.startCubeScale.z) {
                    VB25d.scaleCubeV3 = VB25d.startCubeScale;
                    VB25d.scaleCubeV3 += new Vector3(accuracy, accuracy, accuracy);
                }
                else if (VB25d.scaleCubeV3.x < VB25d.startCubeScale.x
                          || VB25d.scaleCubeV3.y < VB25d.startCubeScale.y
                          || VB25d.scaleCubeV3.z < VB25d.startCubeScale.z) {
                    VB25d.scaleCubeV3 = VB25d.startCubeScale;
                    VB25d.scaleCubeV3 -= new Vector3(accuracy, accuracy, accuracy);
                }
            }
            else {
                if (VB25d.scaleCubeV3.x > VB25d.startCubeScale.x) {
                    VB25d.scaleCubeV3 = VB25d.startCubeScale;
                    VB25d.scaleCubeV3 = new Vector3(VB25d.scaleCubeV3.x + accuracy, VB25d.scaleCubeV3.y, VB25d.scaleCubeV3.z);
                }
                else if (VB25d.scaleCubeV3.y > VB25d.startCubeScale.y) {
                    VB25d.scaleCubeV3 = VB25d.startCubeScale;
                    VB25d.scaleCubeV3 = new Vector3(VB25d.scaleCubeV3.x, VB25d.scaleCubeV3.y + accuracy, VB25d.scaleCubeV3.z);
                }
                else if (VB25d.scaleCubeV3.z > VB25d.startCubeScale.z) {
                    VB25d.scaleCubeV3 = VB25d.startCubeScale;
                    VB25d.scaleCubeV3 = new Vector3(VB25d.scaleCubeV3.x, VB25d.scaleCubeV3.y, VB25d.scaleCubeV3.z + accuracy);
                }
                else if (VB25d.scaleCubeV3.x < VB25d.startCubeScale.x) {
                    VB25d.scaleCubeV3 = VB25d.startCubeScale;
                    VB25d.scaleCubeV3 = new Vector3(VB25d.scaleCubeV3.x - accuracy, VB25d.scaleCubeV3.y, VB25d.scaleCubeV3.z);
                }
                else if (VB25d.scaleCubeV3.y < VB25d.startCubeScale.y) {
                    VB25d.scaleCubeV3 = VB25d.startCubeScale;
                    VB25d.scaleCubeV3 = new Vector3(VB25d.scaleCubeV3.x, VB25d.scaleCubeV3.y - accuracy, VB25d.scaleCubeV3.z);
                }
                else if (VB25d.scaleCubeV3.z < VB25d.startCubeScale.z) {
                    VB25d.scaleCubeV3 = VB25d.startCubeScale;
                    VB25d.scaleCubeV3 = new Vector3(VB25d.scaleCubeV3.x, VB25d.scaleCubeV3.y, VB25d.scaleCubeV3.z - accuracy);
                }
            }
            if (VB25d.isNoKeepCubeProp) {
                VB25d.CubeUI.transform.localScale = VB25d.scaleCubeV3;
            }
            else {
                if (!VB25d.cam.orthographic) {
                    VB25d.CubeUI.transform.localScale = VB25d.scaleCubeV3 * (Mathf.Abs((new Plane(VB25d.cam.transform.forward,
                                                                                                  VB25d.cam.transform.position).GetDistanceToPoint(
                                                                                                  VB25d.CubeUI.transform.position)) / VB25d.initialCubeDist));
                }
                else {
                    VB25d.CubeUI.transform.localScale = VB25d.scaleCubeV3;
                }
            }
        }
    }

    public void _HideAllAreas() {
        if (GameObject.Find("VBAreaTK")) {
            GameObject findObj = GameObject.Find("VBAreaTK");
            foreach (Renderer renderer in findObj.GetComponentsInChildren(typeof(Renderer))) {
                if (VB25d.isHideAreas) {
                    renderer.enabled = false;
                    if (!VB25d.isHideMesh) {
                        if (GameObject.Find("VBMeshTK")) {
                            GameObject findVBNavMesh = GameObject.Find("VBMeshTK");
                            findVBNavMesh.GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                }
                else {
                    renderer.enabled = true;
                    if (!VB25d.isHideMesh) {
                        if (GameObject.Find("VBMeshTK")) {
                            GameObject findVBNavMesh = GameObject.Find("VBMeshTK");
                            findVBNavMesh.GetComponent<MeshRenderer>().enabled = true;
                        }
                    }
                }
            }
        }
    }
}
