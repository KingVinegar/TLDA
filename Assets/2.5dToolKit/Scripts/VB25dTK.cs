using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using System.Threading;
using UnityEngine.AI;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class Startup {
    static Startup() {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
    }
}

[System.Serializable]
public class Point {
    public List<Vector3> list;
}

[System.Serializable]
public class PointList {
    public List<Point> list;
}

[ExecuteInEditMode]
[System.Serializable]

public class VB25dTK : MonoBehaviour {

    [HideInInspector] public GameObject setBackgroundImg;
    [HideInInspector] public GameObject floorTilemap;
    [HideInInspector] public GameObject gridTilemap;
    [HideInInspector] public GameObject moveSprite;
    [HideInInspector] public GameObject newMainObj;
    [HideInInspector] public GameObject character;
    [HideInInspector] public GameObject CubeUI;
    [HideInInspector] public GameObject floor;
    [HideInInspector] public Object objDestroy;
    [HideInInspector] public Object SpriteImg;
    [HideInInspector] public Plane horPlane;
    [HideInInspector] public Camera cam;
    [HideInInspector] public List<Vector3> listNearPointScale = new List<Vector3>();
    [HideInInspector] public List<Vector3> listNearPointPos = new List<Vector3>();
    [HideInInspector] public PointList ListOfPointLists = new PointList();
    [HideInInspector] public List<string> listGroup = new List<string>();
    [HideInInspector] public List<int> listIsWalkable = new List<int>();
    [HideInInspector] public EditingState state = EditingState.None;
    [HideInInspector] public enum EditingState { None, Adding };
    [HideInInspector] public string currentState = "Not editing.";
    [HideInInspector] public string meshName = "Missing";
    [HideInInspector] public string lineDegrees = "0.0";
    [HideInInspector] public string lineTo90 = "0.0";
    [HideInInspector] public string objectPath;
    [HideInInspector] public string scene = "";
    [HideInInspector] public Vector3[] storeWidthPos = new Vector3[] { Vector3.negativeInfinity, Vector3.negativeInfinity };
    [HideInInspector] public Vector3[] storeDepthPos = new Vector3[] { Vector3.negativeInfinity, Vector3.negativeInfinity };
    [HideInInspector] public Vector3 previousPosPoint = Vector3.zero;
    [HideInInspector] public Vector3 currentPosPoint = Vector3.zero;
    [HideInInspector] public Vector3 startCubeScale = Vector3.one;
    [HideInInspector] public Vector3 hitPosPoint = Vector3.zero;
    [HideInInspector] public Vector3 oldCharScale = Vector3.one;
    [HideInInspector] public Vector3 scaleCharV3 = Vector3.one;
    [HideInInspector] public Vector3 copiedScale = Vector3.one;
    [HideInInspector] public Vector3 scaleCubeV3 = Vector3.one;
    [HideInInspector] public Vector3 posCube = Vector3.zero;
    [HideInInspector] public Vector3 rotCube = Vector3.zero;
    [HideInInspector] public Vector3 MoveEnv = Vector3.zero;
    [HideInInspector] public Vector3 mousePositionWorld;
    [HideInInspector] public Vector2 keepStraightLineV2;
    [HideInInspector] public Vector3 keepStraightLine;
    [HideInInspector] public Vector3 startMeterPoint;
    [HideInInspector] public Vector3 gridTilemapRot;
    [HideInInspector] public Vector3 gridTilemapPos;
    [HideInInspector] public Vector3 nearPointScale;
    [HideInInspector] public Vector3 startCharScale;
    [HideInInspector] public Vector3 handleObjPivot;
    [HideInInspector] public Vector3 camTilemapRot;
    [HideInInspector] public Vector3 camTilemapPos;
    [HideInInspector] public Vector3 endMeterPoint;
    [HideInInspector] public Vector3 farPointScale;
    [HideInInspector] public Vector3 nearPointPos;
    [HideInInspector] public Vector3 startCamRot;
    [HideInInspector] public Vector3 startCamPos;
    [HideInInspector] public Vector3 farPointPos;
    [HideInInspector] public Vector3 oldPointPos;
    [HideInInspector] public Vector3 startScale;
    [HideInInspector] public Vector3 startBGPos;
    [HideInInspector] public Vector3 keepHorz;
    [HideInInspector] public Vector3 posCamV3;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Vector3 posBGV3;
    [HideInInspector] public Vector3 RotBGV3;
    [HideInInspector] public Vector3 scaleV3;
    [HideInInspector] public Vector3 posChar;
    [HideInInspector] public Vector3 charPos;
    [HideInInspector] public Vector3 rotChar;
    [HideInInspector] public Vector3 rotCam;
    [HideInInspector] public Vector3 posV3;
    [HideInInspector] public Vector3 rotV3;
    [HideInInspector] public int[] selectedSpot = new int[] { -1, -1 };
    [HideInInspector] public int selectedTabExtensions = 0;
    [HideInInspector] public int keepStraightOffset = 10;
    [HideInInspector] public int selectedTabBottom = 0;
    [HideInInspector] public int selectedTabState = 0;
    [HideInInspector] public int radiusMouseSpot = 10;
    [HideInInspector] public int radiusMouseLine = 10;
    [HideInInspector] public int fontSizeDegrees = 12;
    [HideInInspector] public int selectedTabTop = 0;
    [HideInInspector] public int nPlaceLabelDegrees;
    [HideInInspector] public int nSelectedSpot = -1;
    [HideInInspector] public int nScalePoints = -1;
    [HideInInspector] public int nSelectedTab = 0;
    [HideInInspector] public int selectedTab = 0;
    [HideInInspector] public int nSquare = 10;
    [HideInInspector] public int fontSize = 12;
    [HideInInspector] public int regionScale;
    [HideInInspector] public int nGroup = 0;
    [HideInInspector] public Color inactiveLineColor;
    [HideInInspector] public Color mouseOverSpotCol;
    [HideInInspector] public Color notWalkCol;
    [HideInInspector] public Color floorColor;
    [HideInInspector] public Color lineColor;
    [HideInInspector] public Color textColor;
    [HideInInspector] public Color boxColor;
    [HideInInspector] public Color meshCol;
    [HideInInspector] public Color walkCol;
    [HideInInspector] public bool[] treeState = new bool[] { false, false, false };
    [HideInInspector] public bool isAutoDistanceDisabled;
    [HideInInspector] public bool isBackgroundImgSetView;
    [HideInInspector] public bool isHideGeneralSetting;
    [HideInInspector] public bool isHideLineAutoMeter;
    [HideInInspector] public bool isHideGizmosSetting;
    [HideInInspector] public bool isMiddleBtnDisables;
    [HideInInspector] public bool isDontDestroyOnSave;
    [HideInInspector] public bool drawGizmosLineMeter;
    [HideInInspector] public bool isHideResizeSetting;
    [HideInInspector] public bool activeWork = false;
    [HideInInspector] public bool isUseImgBackground;
    [HideInInspector] public bool lockCharScaleAuto;
    [HideInInspector] public bool isExitingEditMode;
    [HideInInspector] public bool isDrawInSceneView;
    [HideInInspector] public bool wasIsMoreAccuracy;
    [HideInInspector] public bool isDoubleSidedMesh;
    [HideInInspector] public bool isBGManualSetting;
    [HideInInspector] public bool isRotate90Tilemap;
    [HideInInspector] public bool isShowLineDegrees;
    [HideInInspector] public bool isShowFixedDegree;
    [HideInInspector] public bool isExtremeAccuracy;
    [HideInInspector] public bool isLabelAutoMeter;
    [HideInInspector] public bool isNoKeepCubeProp;
    [HideInInspector] public bool isUseProportions;
    [HideInInspector] public bool isSetProportions;
    [HideInInspector] public bool dragAndCloseArea;
    [HideInInspector] public bool isCheckedHideObj;
    [HideInInspector] public bool isHideColSetting;
    [HideInInspector] public bool isLockCubeScale;
    [HideInInspector] public bool isUseImgInScene;
    [HideInInspector] public bool isMouseOverSpot;
    [HideInInspector] public bool isMouseOverLine;
    [HideInInspector] public bool isPreviewResize;
    [HideInInspector] public bool moveCharToPoint;
    [HideInInspector] public bool isLockCharScale;
    [HideInInspector] public bool isLabelDegrees;
    [HideInInspector] public bool isKeepHorzLine;
    [HideInInspector] public bool isKeepVertLine;
    [HideInInspector] public bool wasIsBtnRepeat;
    [HideInInspector] public bool isSetUpScaling;
    [HideInInspector] public bool isSelectedSpot;
    [HideInInspector] public bool isMoreAccuracy;
    [HideInInspector] public bool drawGizmosLine;
    [HideInInspector] public bool showCubeEdges;
    [HideInInspector] public bool isInitMissing;
    [HideInInspector] public bool wasIsAccuracy;
    [HideInInspector] public bool isEnableMeter;
    [HideInInspector] public bool isSelectPoint;
    [HideInInspector] public bool isReverseMesh;
    [HideInInspector] public bool isViewFrustum;
    [HideInInspector] public bool isRestoreGrid;
    [HideInInspector] public bool isTranspCube;
    [HideInInspector] public bool isEnableChar;
    [HideInInspector] public bool isHiddenChar;
    [HideInInspector] public bool isSetCharPos;
    [HideInInspector] public bool isNoKeepProp;
    [HideInInspector] public float nStoreDepth;
    [HideInInspector] public float nStoreWidth;
    [HideInInspector] public bool isStoreWidth;
    [HideInInspector] public bool isStoreDepth;
    [HideInInspector] public bool isAutoPosObj;
    [HideInInspector] public bool isMoveObject;
    [HideInInspector] public bool isAutoMeter;
    [HideInInspector] public bool isHidePivot;
    [HideInInspector] public bool onHoverSpot;
    [HideInInspector] public bool isHideAreas;
    [HideInInspector] public bool isLockScale;
    [HideInInspector] public bool firstIsLast;
    [HideInInspector] public bool isBeginWork;
    [HideInInspector] public bool use25dtkEnv;
    [HideInInspector] public bool isHideCamBg;
    [HideInInspector] public bool isShowFloor;
    [HideInInspector] public bool isBtnRepeat;
    [HideInInspector] public bool isPlaceCube;
    [HideInInspector] public bool isUseMeter;
    [HideInInspector] public bool isWireSpot;
    [HideInInspector] public bool isAccuracy;
    [HideInInspector] public bool isHidePath;
    [HideInInspector] public bool isCamOrtho;
    [HideInInspector] public bool hideLabels;
    [HideInInspector] public bool isUnityNav;
    [HideInInspector] public bool isHideMesh;
    [HideInInspector] public bool isUseCube;
    [HideInInspector] public bool isMeshNav;
    [HideInInspector] public bool moveSpot;
    [HideInInspector] public bool isCamRot;
    [HideInInspector] public float measurementMeter = 1f;
    [HideInInspector] public float saveForMeter = 1f;
    [HideInInspector] public float linkToMeter = 1f;
    [HideInInspector] public float linkedMeter = 1f;
    [HideInInspector] public float spotSize = 100f;
    [HideInInspector] public float initialCubeDist;
    [HideInInspector] public float startFloorPos;
    [HideInInspector] public float fovCam = 60f;
    [HideInInspector] public float sizeCam = 5f;
    [HideInInspector] public float startCamSize;
    [HideInInspector] public float initialDist;
    [HideInInspector] public float startCamFov;
    [HideInInspector] public float vert = -1f;
    [HideInInspector] public float diffScale;
    [HideInInspector] public float startZ;
    [HideInInspector] public float diffZ;
    [HideInInspector] public GUIStyle labelStyle = null;
    [HideInInspector] public GUIStyle labelDegrees = null;
    [HideInInspector] public Sprite backgroundImg;
    [HideInInspector] public Texture2D MouseCurs;
    [HideInInspector] public Material notwalkMat;
    [HideInInspector] public Material walkMat;
    private int[] fromToSpot = new int[] { -1, -1 };
    private Vector2 mousePositionScreen;
    private Vector3 insertSpot;
    private Vector3 v3FrontTopLeft;
    private Vector3 v3FrontTopRight;
    private Vector3 v3FrontBottomLeft;
    private Vector3 v3FrontBottomRight;
    private Vector3 v3BackTopLeft;
    private Vector3 v3BackTopRight;
    private Vector3 v3BackBottomLeft;
    private Vector3 v3BackBottomRight;
    private bool showSpotOnLine;
    private bool isHideObj;
    private Event e;
    private Texture2D whiteTex;
    private Material mat;
    private VBMoveEnvTK VBMoveEnv;
    private VBFilesTK VBFiles;
    private VBObjsTK VBObjs;
    private VBNavTK VBNav;

    public void OnGUI() {
        if (!Application.isPlaying) {
            if (cam != null && character != null) {
                if (floor != null || use25dtkEnv || floorTilemap != null) {
                    if (moveSpot) {
                        StartCoroutine("WaitSpot");
                    }
                    if (use25dtkEnv || floorTilemap != null) {
                        CreateFloor();
                        e = Event.current;
                        Vector3 mousePosition = e.mousePosition;
                        mousePosition.y = Screen.height - mousePosition.y;
                        mousePositionScreen = mousePosition;
                        Ray ray = cam.ScreenPointToRay(mousePosition);
                        float distance = 0f;
                        if (horPlane.Raycast(ray, out distance)) {
                            activeWork = true;
                            Vector3 MouseGetPosold = ray.GetPoint(distance);
                            mousePositionWorld = MouseGetPosold;
                            transform.position = MouseGetPosold;
                            hitPosPoint = MouseGetPosold;
                        }
                        if (isEnableChar) {
                            charPos = mousePositionWorld = ray.GetPoint(distance);
                            if (charPos.z > cam.farClipPlane) {
                                return;
                            }
                            if (isSetUpScaling) {
                                startZ = character.transform.position.z;
                            }
                        }
                    }
                    else {
                        e = Event.current;
                        Vector3 mousePosition = e.mousePosition;
                        mousePosition.y = Screen.height - mousePosition.y;
                        mousePositionScreen = mousePosition;
                        Ray ray = cam.ScreenPointToRay(mousePosition);
                        RaycastHit hitInfo;
                        if (Physics.Raycast(ray, out hitInfo)) {
                            activeWork = true;
                            mousePositionWorld = hitInfo.point;
                            transform.position = hitInfo.point;
                            hitPosPoint = hitInfo.point;
                        }
                        if (isEnableChar) {
                            charPos = mousePositionWorld = hitInfo.point;
                            if (charPos.z > cam.farClipPlane) {
                                return;
                            }
                            if (isSetUpScaling) {
                                startZ = character.transform.position.z;
                            }
                        }
                    }
                    if (activeWork) {
                        if (e.rawType == EventType.MouseDown) {
                            switch (e.button) {
                                case 0:

                                    if (!dragAndCloseArea) {
                                        if (moveSprite != null) {
                                            moveObject();
                                            isMoveObject = true;
                                        }
                                        else if (isMouseOverSpot) {
                                            if (ListOfPointLists.list[selectedSpot[0]].list[ListOfPointLists.list[selectedSpot[0]].list.Count - 1] ==
                                                ListOfPointLists.list[selectedSpot[0]].list[0]) {
                                                RemoveMesh();
                                            }
                                            if (isSelectPoint) {
                                                isSelectedSpot = true;
                                                nSelectedSpot = selectedSpot[1];
                                                oldPointPos = ListOfPointLists.list[nGroup - 1].list[nSelectedSpot];
                                            }
                                            else {
                                                moveSpot = true;
                                            }
                                        }
                                        else {
                                            if (ListOfPointLists.list != null && nGroup > 0) {
                                                if (ListOfPointLists.list[nGroup - 1].list.Count > 0) {
                                                    if (ListOfPointLists.list[nGroup - 1].list.IndexOf(ListOfPointLists.list[nGroup - 1].list[0], 1) < 0) {
                                                        drawGizmosLine = true;
                                                        previousPosPoint = currentPosPoint;
                                                        currentPosPoint = hitPosPoint;
                                                        DrawPoint();
                                                    }
                                                }
                                                else {
                                                    drawGizmosLine = true;
                                                    previousPosPoint = currentPosPoint;
                                                    currentPosPoint = hitPosPoint;
                                                    DrawPoint();
                                                }
                                            }
                                        }
                                    }
                                    if (isEnableMeter && selectedTab == 1 && !isPlaceCube) {
                                        if (!drawGizmosLineMeter) {
                                            startMeterPoint = mousePositionWorld;
                                            drawGizmosLineMeter = true;
                                        }
                                        else {
                                            endMeterPoint = mousePositionWorld;
                                            drawGizmosLineMeter = false;
                                            if (cam.orthographic && isUseProportions) {
                                                if (isSetProportions) {
                                                    endMeterPoint = keepHorz;
                                                }
                                                measurementMeter = Vector3.Distance(startMeterPoint, endMeterPoint);
                                                if (isStoreWidth) {
                                                    nStoreWidth = measurementMeter;
                                                    storeWidthPos[0] = startMeterPoint;
                                                    storeWidthPos[1] = endMeterPoint;
                                                    isStoreWidth = false;
                                                }
                                                else if (isStoreDepth) {
                                                    nStoreDepth = measurementMeter;
                                                    storeDepthPos[0] = startMeterPoint;
                                                    storeDepthPos[1] = endMeterPoint;
                                                    isStoreDepth = false;
                                                }
                                                if (isSetProportions) {
                                                    saveForMeter = measurementMeter;
                                                }
                                                float height = 2f * cam.orthographicSize;
                                                float width = height * cam.aspect;
                                                float x = (linkToMeter * width) / saveForMeter;
                                                linkedMeter = (measurementMeter * x) / width;
                                                Debug.Log("2.5D Toolkit:\nLength: " + linkedMeter + " (True measurement: " + measurementMeter + ")");
                                            }
                                            else {
                                                measurementMeter = Vector3.Distance(startMeterPoint, endMeterPoint);
                                                if (isStoreWidth) {
                                                    nStoreWidth = measurementMeter;
                                                    storeWidthPos[0] = startMeterPoint;
                                                    storeWidthPos[1] = endMeterPoint;
                                                    isStoreWidth = false;
                                                }
                                                else if (isStoreDepth) {
                                                    nStoreDepth = measurementMeter;
                                                    storeDepthPos[0] = startMeterPoint;
                                                    storeDepthPos[1] = endMeterPoint;
                                                    isStoreDepth = false;
                                                }
                                                Debug.Log("2.5D Toolkit:\nLength: " + measurementMeter);
                                            }
                                            startMeterPoint = endMeterPoint = Vector3.zero;
                                        }
                                    }
                                    else if (isPlaceCube && selectedTab == 1) {
                                        moveCube();
                                    }
                                    break;
                                case 1:
                                    if (isMouseOverLine) {
                                        showSpotOnLine = true;
                                    }
                                    else if (drawGizmosLineMeter) {
                                        drawGizmosLineMeter = false;
                                        startMeterPoint = endMeterPoint = Vector3.zero;
                                    }
                                    else if (isEnableChar && (selectedTab == 0 || selectedTab == 1 || selectedTab == 3)) {
                                        if (isPreviewResize) {
                                            character.transform.position = charPos;
                                            posChar = charPos;
                                            previewResize();
                                        }
                                        else {
                                            character.transform.position = charPos;
                                            posChar = charPos;
                                        }
                                    }
                                    else {
                                        if (ListOfPointLists.list != null && nGroup > 0) {
                                            if (ListOfPointLists.list[nGroup - 1].list.Count > 0) {
                                                if (ListOfPointLists.list[nGroup - 1].list.IndexOf(ListOfPointLists.list[nGroup - 1].list[0], 1) < 0) {
                                                    drawGizmosLine = !drawGizmosLine;
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        else if (e.rawType == EventType.MouseUp) {
                            switch (e.button) {
                                case 0:
                                    if (moveSprite != null) {
                                        isMoveObject = false;
                                    }
                                    else if (isMouseOverSpot) {
                                        moveSpot = false;
                                        firstIsLast = false;
                                        if (dragAndCloseArea) {
                                            if (drawGizmosLine) {
                                                ClosePerimeterFromLine();
                                            }
                                            else {
                                                ClosePerimeterFromSpot();
                                            }
                                            dragAndCloseArea = false;
                                        }
                                        else if (ListOfPointLists.list[selectedSpot[0]].list[ListOfPointLists.list[selectedSpot[0]].list.Count - 1] ==
                                                 ListOfPointLists.list[selectedSpot[0]].list[0]
                                            && !dragAndCloseArea
                                            && ListOfPointLists.list[selectedSpot[0]].list.Count - 1 > 0) {
                                            CreateAreasToBake();
                                        }
                                        else if (ListOfPointLists.list[selectedSpot[0]].list.Count - 1 == selectedSpot[1]) {
                                            currentPosPoint = ListOfPointLists.list[selectedSpot[0]].list[selectedSpot[1]];
                                        }
                                    }
                                    break;
                                case 1:
                                    if (showSpotOnLine && isMouseOverLine) {
                                        ListOfPointLists.list[fromToSpot[0]].list.Insert(fromToSpot[1], insertSpot);
                                    }
                                    if (showSpotOnLine) {
                                        showSpotOnLine = false;
                                    }
                                    break;
                                case 2:
                                    if (isMouseOverSpot) {
                                        DeleteSpot();
                                    }
                                    else if (!isMiddleBtnDisables) {
                                        if (ListOfPointLists.list != null && nGroup > 0 && !isMouseOverLine && !isSelectPoint) {
                                            RemoveLastPoint();
                                        }
                                    }
                                    break;
                            }
                        }
                        else if (e.rawType == EventType.MouseDrag) {
                            switch (e.button) {
                                case 0:
                                    if (moveSprite != null) {
                                        moveObject();
                                        isMoveObject = true;
                                    }
                                    else if (isPlaceCube && selectedTab == 1) {
                                        moveCube();
                                    }
                                    break;
                                case 1:
                                    if (isEnableChar && (selectedTab == 0 || selectedTab == 1 || selectedTab == 3)) {
                                        character.transform.position = charPos;
                                        posChar = charPos;
                                        if (isPreviewResize) {
                                            previewResize();
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    activeWork = false;
                }
            }
        }
    }

    // Move spot
    public IEnumerator WaitSpot() {
        Vector3 newVector3 = new Vector3(Mathf.Round(mousePositionWorld.x * 1000f) / 1000f,
                                         Mathf.Round(mousePositionWorld.y * 1000f) / 1000f,
                                         Mathf.Round(mousePositionWorld.z * 1000f) / 1000f);
        ListOfPointLists.list[selectedSpot[0]].list[selectedSpot[1]] = newVector3;
        if (firstIsLast) {
            ListOfPointLists.list[selectedSpot[0]].list[ListOfPointLists.list[selectedSpot[0]].list.Count - 1] =
            ListOfPointLists.list[nGroup - 1].list[selectedSpot[1]];
        }
        yield return new WaitForSeconds(0.1f);
    }

    public void moveCube() {
        CubeUI.transform.position = mousePositionWorld;
        if ((!isNoKeepCubeProp) && (!cam.orthographic)) {
            CubeUI.transform.localScale = scaleCubeV3 * (Mathf.Abs((new Plane(cam.transform.forward,
                                                                    cam.transform.position).GetDistanceToPoint(
                                                                    CubeUI.transform.position)) / initialCubeDist));
        }
        posCube = CubeUI.transform.position;
    }

    // Move 2d and 3d objects in Game View
    public void moveObject() {
        moveSprite.transform.position = mousePositionWorld;
        if (isAutoPosObj) {
            AutoPosObj();
        }
        else {
            if ((!isNoKeepProp) && (!cam.orthographic)) {
                moveSprite.transform.localScale = scaleV3 * (Mathf.Abs((new Plane(cam.transform.forward,
                                                                                  cam.transform.position).GetDistanceToPoint(
                                                                                  moveSprite.transform.position)) / initialDist));
            }
            posV3 = moveSprite.transform.position;
        }
    }

    // Scale objects
    private void ScaleSprite() {
        VBObjs._ScaleSprite();
    }

    // Restore object to its inizial conditions
    private void ResetSprite() {
        VBObjs._ResetSprite();
    }

    // Resize objects to +0.5, -05
    private void Scale05() {
        VBObjs._Scale05();
    }

    // Assign camera rotation to object
    private void RotSprite() {
        VBObjs._RotSprite();
    }

    // Move 2d and 3d objects in the Game View
    private void PosSprite() {
        VBObjs._PosSprite();
    }

    private void PosCam() {
        VBObjs._PosCam();
    }

    private void RotCam() {
        VBObjs._RotCam();
    }

    private void MoveFloor() {
        VBObjs._MoveFloor();
    }

    /* [Temporarily disabled]
    private void fovsizeCam(int camType) {
        VBObjs._fovsizeCam(camType);
    }
    */

    private void PosBG() {
        VBObjs._PosBG();
    }

    // Initialize the object dragged in the "Object" field
    private void InitializeSprite() {
        VBObjs._InitializeSprite();
    }

    public void EmptyObject() {
        VBObjs._EmptyObject();
    }

    private void EditorSceneManager_sceneOpened(Scene arg0, OpenSceneMode mode) {
        switch (arg0.name) {
            case "Test_Cableway_Orthographic":
                Debug.Log("2.5D Toolkit:\n[Scene " + arg0.name + "] Set Aspect Ratio to 1670:924");
                break;
            case "Test_Cableway_Perspective":
                Debug.Log("2.5D Toolkit:\n[Scene " + arg0.name + "] Set Aspect Ratio to 1670:924");
                break;
            case "Test_IBM":
                Debug.Log("2.5D Toolkit:\n[Scene " + arg0.name + "] Set Aspect Ratio to 1080:699");
                break;
            case "Test_Mainframe":
                Debug.Log("2.5D Toolkit:\n[Scene " + arg0.name + "] Set Aspect Ratio to 1480:800");
                break;
            case "Test_NicoHome":
                Debug.Log("2.5D Toolkit:\n[Scene " + arg0.name + "] Set Aspect Ratio to 3734:2161");
                break;
            case "Test_Pools":
                Debug.Log("2.5D Toolkit:\n[Scene " + arg0.name + "] Set Aspect Ratio to 770:577");
                break;
            case "Test_RPG":
                Debug.Log("2.5D Toolkit:\n[Scene " + arg0.name + "] Set Aspect Ratio to 650:430");
                break;
            case "Test_Salon":
                Debug.Log("2.5D Toolkit:\n[Scene " + arg0.name + "] Set Aspect Ratio to 1565:1020");
                break;
            case "Test_Villa":
                Debug.Log("2.5D Toolkit:\n[Scene " + arg0.name + "] Set Aspect Ratio to 4200:2800");
                break;
        }
    }

    private void OnEditorChangedPlayMode(PlayModeStateChange state) {
        if (Selection.activeGameObject != GameObject.Find("VB25dTK")) {
            Selection.activeGameObject = GameObject.Find("VB25dTK");
        }
        if (state == PlayModeStateChange.EnteredPlayMode) {
            if (!isInitMissing) {
                if (character != null) {
                    character.SetActive(true);
                    if (character.GetComponent<NavMeshAgent>() != null) {
                        character.GetComponent<NavMeshAgent>().enabled = true;
                    }
                    if (character.GetComponent<CharControl>() != null) {
                        character.GetComponent<CharControl>().enabled = true;
                    }
                }
                if (CubeUI != null) {
                    CubeUI.SetActive(false);
                }
            }
            else {
                Debug.Log("2.5D Toolkit:\nEnd initialization to enter play mode.");
                EditorApplication.isPlaying = false;
            }
        }
        else if (state == PlayModeStateChange.ExitingEditMode) {
            if (SpriteImg != null) {
                VBObjs._EmptyObject();
            }
            if (nGroup > 0) {
                nGroup = 0;
            }
            isExitingEditMode = true;
            selectedTabState = selectedTab;
            selectedTab = 2;
            treeState[0] = isHideResizeSetting;
            isHideColSetting = true;
            isHideCamBg = true;
            isHideResizeSetting = true;
            if (isSetUpScaling && !use25dtkEnv) {
                previewResize();
            }
        }
        else if (state == PlayModeStateChange.ExitingPlayMode) {
            //
        }
        else if (state == PlayModeStateChange.EnteredEditMode) {
            isHideResizeSetting = treeState[0];
            selectedTab = selectedTabState;
            isExitingEditMode = false;
            isEnableChar = true;
            if (character != null) {
                character.SetActive(true);
                EnableCharacter();
            }
            if (CubeUI != null) {
                CubeUI.SetActive(true);
            }
        }
    }

    private void Awake() {
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        mat = new Material(shader);
        moveSprite = null;
    }

    // Change cursor
    public void ChangeCursor() {
        Vector2 cursorHotspot = Vector2.zero;
        if (MouseCurs != null) {
            if (MouseCurs.name != "point") {
                cursorHotspot = new Vector2(MouseCurs.width / 2, MouseCurs.height / 2);
            }
            Cursor.SetCursor(MouseCurs, cursorHotspot, CursorMode.Auto);
        }
        else {
            Cursor.SetCursor(null, cursorHotspot, CursorMode.Auto);
        }
    }

    // Changes color of the walkable areas
    public void ChangeWalkCol() {
        GameObject[] gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
        for (int i = 0; i < gos.Length; i++)
            if (gos[i].name.Contains("VBNavMeshWalkable"))
                gos[i].GetComponent<MeshRenderer>().sharedMaterial.color = new Color(walkCol.r, walkCol.g, walkCol.b, walkCol.a);
    }

    // Changes color of the non-walkable area
    public void ChangeNotWalkCol() {
        GameObject[] gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
        for (int i = 0; i < gos.Length; i++)
            if (gos[i].name.Contains("VBNavMeshNotWalkable"))
                gos[i].GetComponent<MeshRenderer>().sharedMaterial.color = new Color(notWalkCol.r, notWalkCol.g, notWalkCol.b, notWalkCol.a);
    }

    // Changes mesh color
    private void ChangeMeshCol() {
        if (nGroup > 0) {
            if (GameObject.Find("VBMeshTK")) {
                GameObject newMesh = GameObject.Find("VBMeshTK");
                newMesh.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(meshCol.r, meshCol.g, meshCol.b, meshCol.a);
            }
        }
    }

    // Hide mesh
    public void HideMesh() {
        if (GameObject.Find("VBMeshTK")) {
            GameObject findVBNavMesh = GameObject.Find("VBMeshTK");
            if (findVBNavMesh.GetComponent<MeshRenderer>().enabled == true) {
                findVBNavMesh.GetComponent<MeshRenderer>().enabled = false;
            }
            else {
                findVBNavMesh.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private void HideUnityNav() {
        /* Search for navmesh. [Temporarily disabled]
		bool foundNavMesh = false;
		string[] searchNavMesh = AssetDatabase.FindAssets("NavMesh", null);
		foreach (string findNavMesh in searchNavMesh) {
			string dirName = new FileInfo(AssetDatabase.GUIDToAssetPath(findNavMesh)).DirectoryName;
			dirName = new DirectoryInfo(dirName).Name;
			if (dirName == scene) {
				foundNavMesh = true;
				break;
			}
		}
		if (foundNavMesh == false) {
			if (EditorUtility.DisplayDialog("Info!", "Bake area or mesh to use this option", "OK")) {
				isUnityNav = false;
				return;
			}
		}
        */
        if (!GameObject.Find("VBAreaTK")) {
            isUnityNav = isHideObj = false;
            return;
        }
        if (isUnityNav) {
            HideObjects(false, true);
        }
        else {
            HideObjects(true, false);
        }
    }

    private void HideObjects(bool changeObjState, bool changeHideState) {
        if (GameObject.Find("VBAreaTK")) {
            GameObject findVBMesh = GameObject.Find("VBAreaTK");
            foreach (Transform findMesh in findVBMesh.transform) {
                findMesh.GetComponent<MeshRenderer>().enabled = changeObjState;
            }
            string[] meshNames = new string[] { "Plane" };
            MeshFilter[] allMeshFilters = FindObjectsOfType(typeof(MeshFilter)) as MeshFilter[];
            foreach (MeshFilter thisMeshFilter in allMeshFilters) {
                foreach (string primName in meshNames) {
                    if (primName == thisMeshFilter.sharedMesh.name) {
                        thisMeshFilter.GetComponent<MeshRenderer>().enabled = changeObjState;
                    }
                }
            }
            /* [Temporarily disabled]
			if (GameObject.Find("VBMeshTK")) {
				GameObject findVBNavMesh = GameObject.Find("VBMeshTK");
				findVBNavMesh.GetComponent<MeshRenderer>().enabled = changeObjState;
			}
            */
            isHideObj = changeHideState;
            isHideMesh = false;
        }
    }

    // Create mesh
    private void CreateMeshToExport() {
        string scene = SceneManager.GetActiveScene().name;
        meshName = scene + "Mesh";
        VBNav._CreateMeshToExport();
    }

    // Clear created mesh
    public void DeleteMesh() {
        if (GameObject.Find("VBMeshTK")) {
            GameObject destroyObj = GameObject.Find("VBMeshTK");
            DestroyImmediate(destroyObj);
        }
    }

    // Save created mesh
    private void SaveMesh() {
        string meshPath = "Assets/2.5dToolKit/Resources/" + scene + "/";
        if (!Directory.Exists(meshPath)) {
            Directory.CreateDirectory(meshPath);
        }
        if (GameObject.Find(meshName)) {
            GameObject destroyObj = GameObject.Find(meshName);
            DestroyImmediate(destroyObj);
        }
        VBNav.MeshSave(meshPath, meshName);
    }

    // Read areas coordinates 
    private void ReadFile() {
        if (VBFiles != null) {
            string path = "Assets/2.5dToolKit/VBDataAreaTK/" + scene + "/";
            VBFiles.ReadFiles(path);
            if (ListOfPointLists.list != null) {
                if (ListOfPointLists.list.Count > 0) {
                    if (ListOfPointLists.list[0].list.Count > 0) {
                        if (ListOfPointLists.list[0].list.IndexOf(ListOfPointLists.list[0].list[0], 1) > 0) {
                            for (int k = 1; k <= ListOfPointLists.list.Count; k++) {
                                if (ListOfPointLists.list[k - 1].list.IndexOf(ListOfPointLists.list[k - 1].list[0], 1) > 0) {
                                    nGroup = k;
                                    if (ListOfPointLists.list[nGroup - 1].list[ListOfPointLists.list[nGroup - 1].list.Count - 1] ==
                                         ListOfPointLists.list[nGroup - 1].list[0]) {
                                        CreateAreasToBake();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            nGroup = 0;
        }
    }

    // Save areas coordinates
    private void MakeFile() {
        scene = SceneManager.GetActiveScene().name;
        string path = "Assets/2.5dToolKit/VBDataAreaTK/" + scene + "/";
        VBNav._ResetAreasFiles(path);
        VBFiles.MakeFiles(path);
    }

    // Closes perimeter of a new area by connecting the last point inserted with the first one
    private void ClosePerimeter() {
        state = EditingState.None;
        currentState = "Not editing.";
        drawGizmosLine = false;
        previousPosPoint = currentPosPoint;
        currentPosPoint = ListOfPointLists.list[nGroup - 1].list[0];
        DrawPoint();
        CreateAreasToBake();
    }

    private void ClosePerimeterFromLine() {
        state = EditingState.None;
        currentState = "Not editing.";
        drawGizmosLine = false;
        ListOfPointLists.list[nGroup - 1].list.Add(ListOfPointLists.list[nGroup - 1].list[0]);
        CreateAreasToBake();
    }

    private void ClosePerimeterFromSpot() {
        state = EditingState.None;
        currentState = "Not editing.";
        drawGizmosLine = false;
        ListOfPointLists.list[nGroup - 1].list[ListOfPointLists.list[nGroup - 1].list.Count - 1] = ListOfPointLists.list[nGroup - 1].list[0];
        CreateAreasToBake();
    }

    // Insert a vertex
    public void DrawPoint() {
        if (!isKeepHorzLine && !isKeepVertLine || previousPosPoint == Vector3.zero) {
            Vector3 newVector3 = new Vector3(Mathf.Round(currentPosPoint.x * 1000f) / 1000f,
                                             Mathf.Round(currentPosPoint.y * 1000f) / 1000f,
                                             Mathf.Round(currentPosPoint.z * 1000f) / 1000f);
            ListOfPointLists.list[nGroup - 1].list.Add(newVector3);
        }
        else {
            currentPosPoint = keepStraightLine;
            Vector3 newVector3 = new Vector3(Mathf.Round(currentPosPoint.x * 1000f) / 1000f,
                                             Mathf.Round(currentPosPoint.y * 1000f) / 1000f,
                                             Mathf.Round(currentPosPoint.z * 1000f) / 1000f);
            ListOfPointLists.list[nGroup - 1].list.Add(newVector3);
        }
    }

    // Remove a created area
    public void RemoveMesh() {
        string objName;
        if (listIsWalkable[nGroup - 1] == 0) {
            objName = "VBNavMeshWalkable" + nGroup.ToString();
        }
        else {
            objName = "VBNavMeshNotWalkable" + nGroup.ToString();
        }
        if (GameObject.Find(objName)) {
            GameObject destroyObj = GameObject.Find(objName);
            DestroyImmediate(destroyObj);
        }
    }

    public void RemoveMeshForChange() {
        string objName;
        if (listIsWalkable[nGroup - 1] == 0) {
            objName = "VBNavMeshNotWalkable" + nGroup.ToString();
        }
        else {
            objName = "VBNavMeshWalkable" + nGroup.ToString();
        }
        if (GameObject.Find(objName)) {
            GameObject destroyObj = GameObject.Find(objName);
            DestroyImmediate(destroyObj);
        }
    }

    // Removes all the vertices of an area
    private void RemoveAllPoint() {
        state = EditingState.Adding;
        currentState = "Editing.";
        RemoveMesh();
        ListOfPointLists.list[nGroup - 1].list.Clear();
        ResetValue();
    }

    // Removes the last inserted vertex of an area
    public void RemoveLastPoint() {
        state = EditingState.Adding;
        currentState = "Editing.";
        int listSize = ListOfPointLists.list[nGroup - 1].list.Count - 1;
        if (listSize >= 0) {
            RemoveMesh();
            ListOfPointLists.list[nGroup - 1].list.RemoveAt(listSize);
            if (listSize > 0) {
                currentPosPoint = ListOfPointLists.list[nGroup - 1].list[listSize - 1];
            }
            else {
                currentPosPoint = Vector3.zero;
                previousPosPoint = Vector3.zero;
            }
        }
    }

    // Reset areas coordinates
    private void ResetAreasFiles() {
        string path = "Assets/2.5dToolKit/VBDataAreaTK/" + scene + "/";
        VBNav._ResetAreasFiles(path);
    }

    // Reset objects settings
    private void ResetObjectsFiles() {
        string path = "Assets/2.5dToolKit/VBDataObjTK/" + scene + "/";
        int nScene = 0;
        nScene = scene.Length;
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.txt");
        int nFiles = info.Length;
        if (nFiles > 0) {
            for (int i = 0; i < nFiles; i++) {
                string nameObj = "";
                nameObj = info[i].ToString();
                string nameFile = Path.GetFileNameWithoutExtension(nameObj);
                nameFile = nameFile.Substring(nScene);
                if (GameObject.Find(nameFile)) {
                    GameObject destroyObj = GameObject.Find(nameFile);
                    DestroyImmediate(destroyObj);
                }
            }
        }
        if (Directory.Exists(path)) {
            Directory.Delete(path, true);
        }
        Directory.CreateDirectory(path);
    }

    // Delete all areas
    private void ResetGroup() {
        state = EditingState.None;
        currentState = "Not editing.";
        if (GameObject.Find("VBAreaTK") != null) {
            GameObject findObj = GameObject.Find("VBAreaTK");
            DestroyImmediate(findObj);
        }
        nGroup = 0;
        listGroup.Clear();
        listGroup.Add("none");
        listIsWalkable.Clear();
        isSelectedSpot = false;
        nSelectedSpot = -1;
        isSelectPoint = false;
        labelStyle = null;
        if (ListOfPointLists.list != null) {
            ListOfPointLists.list.Clear();
            for (int i = 0; i < ListOfPointLists.list.Count; i++) {
                ListOfPointLists.list.RemoveAt(i);
            }
        }
        if (GameObject.Find("VBAreaTK") == null) {
            newMainObj = new GameObject("VBAreaTK");
            newMainObj.transform.position = Vector3.zero;
        }
        SceneView.RepaintAll();
    }

    // Reset some area values 
    public void ResetValue() {
        currentPosPoint = Vector3.zero;
        previousPosPoint = Vector3.zero;
    }

    // Creates a new area to draw
    private void CreateNewArea() {
        state = EditingState.Adding;
        currentState = "Editing.";
        if (labelStyle == null) {
            SetStyle();
            whiteTex.Apply();
        }
        VBNav._CreateNewArea();
    }

    // Select an area
    private void OnChangeGroup() {
        if (nGroup != 0) {
            int listSize = ListOfPointLists.list[nGroup - 1].list.Count - 1;
            if (listSize >= 0) {
                currentPosPoint = ListOfPointLists.list[nGroup - 1].list[listSize];
            }
            else {
                currentPosPoint = Vector3.zero;
            }
            state = EditingState.Adding;
            currentState = "Editing.";
        }
        else {
            state = EditingState.None;
            currentState = "Not editing.";
            isSelectPoint = false;
            nSelectedSpot = -1;
        }
    }

    // Hides all areas
    public void HideAllAreas() {
        if (isHideAreas) {
            HideObjects(false, true);
        }
        else {
            HideObjects(true, false);
        }
    }

    // Save a file with settings of all objects in scene
    private void ExportAllData() {
        VBFiles._ExportAllData();
    }

    private void ResetChar() {
        if (character != null) {
            if (character.GetComponent<NavMeshAgent>() != null) {
                character.GetComponent<NavMeshAgent>().enabled = true;
            }
            if (character.GetComponent<CharControl>() != null) {
                character.GetComponent<CharControl>().enabled = true;
            }
            character.SetActive(true);
        }
    }

    // Show character
    public void EnableCharacter() {
        if (character != null) {
            if (isEnableChar) {
                if (character.GetComponent<NavMeshAgent>() != null) {
                    character.GetComponent<NavMeshAgent>().enabled = false;
                }
                if (character.GetComponent<CharControl>() != null) {
                    character.GetComponent<CharControl>().enabled = false;
                }
            }
            else {
                if (character.GetComponent<NavMeshAgent>() != null) {
                    character.GetComponent<NavMeshAgent>().enabled = true;
                }
                if (character.GetComponent<CharControl>() != null) {
                    character.GetComponent<CharControl>().enabled = true;
                }
            }
        }
        else {
            isEnableChar = false;
        }
    }

    // Reverses triangles of the new mesh
    private void DoubleSidedMesh(Mesh msh) {
        VBNav.MeshDoubleSided(msh);
    }

    // Create the drawn mesh and set flags needed to bake
    private void CreateAreasToBake() {
        VBNav._CreateAreasToBake();
    }

    // Delete vertex
    private void DeleteSpot() {
        if ((ListOfPointLists.list[nGroup - 1].list[ListOfPointLists.list[nGroup - 1].list.Count - 1] ==
             ListOfPointLists.list[nGroup - 1].list[0]) && selectedSpot[1] == 0 &&
             ListOfPointLists.list[nGroup - 1].list.Count > 4) {
            RemoveMesh();
            ListOfPointLists.list[nGroup - 1].list.RemoveAt(selectedSpot[1]);
            ListOfPointLists.list[nGroup - 1].list.RemoveAt(ListOfPointLists.list[nGroup - 1].list.Count - 1);
            ListOfPointLists.list[nGroup - 1].list.Add(ListOfPointLists.list[nGroup - 1].list[0]);
            if (ListOfPointLists.list[nGroup - 1].list.Count > 3) {
                CreateAreasToBake();
            }
        }
        else if (ListOfPointLists.list[nGroup - 1].list.Count > 4 &&
                 ListOfPointLists.list[nGroup - 1].list[ListOfPointLists.list[nGroup - 1].list.Count - 1] ==
                 ListOfPointLists.list[nGroup - 1].list[0]) {
            RemoveMesh();
            ListOfPointLists.list[nGroup - 1].list.RemoveAt(selectedSpot[1]);
            if (ListOfPointLists.list[nGroup - 1].list.Count > 3) {
                CreateAreasToBake();
            }
        }
        else if (ListOfPointLists.list[nGroup - 1].list[ListOfPointLists.list[nGroup - 1].list.Count - 1] !=
                 ListOfPointLists.list[nGroup - 1].list[0]) {
            ListOfPointLists.list[nGroup - 1].list.RemoveAt(selectedSpot[1]);
            currentPosPoint = ListOfPointLists.list[nGroup - 1].list[ListOfPointLists.list[nGroup - 1].list.Count - 1];
        }
    }

    private void ScaleChar() {
        VBObjs._ScaleChar();
    }

    private void ScaleCube() {
        VBObjs._ScaleCube();
    }

    private void CreateFloor() {
        if (use25dtkEnv) {
            horPlane = new Plane(Vector3.up, new Vector3(0, vert, 0));
        }
        else {
            vert = gridTilemap.transform.position.y;
            if (!isRotate90Tilemap) {
                horPlane = new Plane(Vector3.forward, new Vector3(0, vert, 0));
            }
            else {
                horPlane = new Plane(Vector3.up, new Vector3(0, vert, 0));
            }
        }
    }

    public void DeleteArea() {
        if (GameObject.Find("VBAreaTK") != null) {
            GameObject findObj = GameObject.Find("VBAreaTK");
            DestroyImmediate(findObj);
            newMainObj = new GameObject("VBAreaTK");
            newMainObj.transform.position = Vector3.zero;
        }
        int selectedArea = nGroup - 1;
        nGroup -= 1;
        int size = listGroup.Count;
        listGroup.Clear();
        listGroup.Add("none");
        for (int i = 1; i <= size - 2; i++) {
            listGroup.Add(i.ToString());
        }
        listIsWalkable.RemoveAt(selectedArea);
        ListOfPointLists.list.RemoveAt(selectedArea);
        for (int i = 1; i <= size - 2; i++) {
            nGroup = i;
            if (ListOfPointLists.list != null) {
                if (ListOfPointLists.list.Count > 0) {
                    if (ListOfPointLists.list[i - 1].list.Count > 0) {
                        if (ListOfPointLists.list[i - 1].list.IndexOf(ListOfPointLists.list[i - 1].list[0], 1) > 0) {
                            if (ListOfPointLists.list[nGroup - 1].list[ListOfPointLists.list[nGroup - 1].list.Count - 1] ==
                                ListOfPointLists.list[nGroup - 1].list[0]) {
                                CreateAreasToBake();
                            }
                        }
                    }
                }
            }
        }
        OnChangeGroup();
        Debug.Log("2.5D Toolkit:\nArea successfully deleted.");
    }

    public void ObjBtnScale(int direction) {
        VBObjs._ObjBtnScale(direction);
    }

    public void objBtnMove(int direction) {
        VBObjs._objBtnMove(direction);
    }

    public void bgBtnMove(int direction) {
        VBObjs._bgBtnMove(direction);
    }

    public void spotBtnMove(int direction) {
        VBObjs._spotBtnMove(direction);
    }

    /* [Temporarily disabled]
    public void fovsizeCamBtnMove(int camType) {
        VBObjs._fovsizeCamBtnMove(camType);
    }
    */

    public void SetDistanceOnFOV() {
        float frustumHeight = backgroundImg.bounds.size.x / cam.aspect;
        float distance = frustumHeight * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        Vector3 newPos = new Vector3(cam.transform.position.x, cam.transform.position.y, -distance);
        cam.transform.position = posCamV3 = newPos;
    }

    public void previewResize() {
        float ch = 0;
        ch = startZ - character.transform.position.z;
        float value = character.transform.position.z;
        diffZ = Mathf.Abs((listNearPointPos[0].z) - (listNearPointPos[1].z));
        diffScale = Mathf.Abs((listNearPointScale[0].z) - (listNearPointScale[1].z));
        float c = ((ch * diffScale) / diffZ);
        character.transform.localScale += new Vector3(c, c, c);
        scaleCharV3 = character.transform.localScale;
    }

    public void resetFarResize() {
        farPointPos = Vector3.zero;
        farPointScale = Vector3.one;
        resetValueResize();
    }

    public void resetNearResize() {
        listNearPointPos[0] = Vector3.zero;
        listNearPointScale[0] = Vector3.one;
        resetValueResize();
    }

    public void resetValueResize() {
        diffZ = 0;
        diffScale = 0;
        isPreviewResize = false;
    }

    public void AutoPosObj() {
        if (cam.orthographic) {
            posV3 = moveSprite.transform.position;
            float distanceFromCam = Mathf.Abs(moveSprite.transform.position.z);
            Vector3 objCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distanceFromCam));
            moveSprite.transform.position = posV3 = objCenter;

        }
        else {
            posV3 = moveSprite.transform.position;
            moveSprite.transform.localScale = scaleV3 * (Mathf.Abs((new Plane(cam.transform.forward,
                                                cam.transform.position).GetDistanceToPoint(
                                                moveSprite.transform.position)) / initialDist));
            float distanceFromCam = Mathf.Abs(cam.transform.position.z - moveSprite.transform.position.z); 
            Vector3 objCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distanceFromCam));
            moveSprite.transform.position = posV3 = objCenter;
            moveSprite.transform.localScale = scaleV3 * (Mathf.Abs((new Plane(cam.transform.forward,
                                                                              cam.transform.position).GetDistanceToPoint(
                                                                              moveSprite.transform.position)) / initialDist));
        }
    }

    public void centerBG() {
        float distanceFromCam = Mathf.Abs(cam.transform.position.z);
        Vector3 bgCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distanceFromCam));
        setBackgroundImg.transform.position = posBGV3 = bgCenter;
    }

    public void SearchBGSettings() {
        posBGV3 = setBackgroundImg.transform.position;
        RotBGV3 = TransformUtils.GetInspectorRotation(cam.transform);
        setBackgroundImg.transform.rotation = Quaternion.Euler(RotBGV3);
        centerBG();
    }

    public void SimpleSearchBGSettingsSize() {
        if (setBackgroundImg != null) {
            Vector3 previousCamRot = TransformUtils.GetInspectorRotation(cam.transform);
            Vector3 previousCamPos = cam.transform.position;
            setBackgroundImg.transform.rotation = Quaternion.Euler(previousCamRot);
            setBackgroundImg.transform.position = Vector3.zero;
            float distanceFromCam = Mathf.Abs(previousCamPos.z);
            cam.transform.position = new Vector3(0, 0, -distanceFromCam);
            float sizeCam = backgroundImg.rect.height / (2f * backgroundImg.pixelsPerUnit);
            cam.orthographicSize = sizeCam;
            Vector3 bgCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distanceFromCam));
            setBackgroundImg.transform.position = bgCenter;
        }
    }

    public void SimpleSearchBGSettingsFOV() {
        if (setBackgroundImg != null) {
            Vector3 previousCamRot = TransformUtils.GetInspectorRotation(cam.transform);
            setBackgroundImg.transform.rotation = Quaternion.Euler(previousCamRot);
            setBackgroundImg.transform.position = Vector3.zero;
            float frustumHeight = backgroundImg.bounds.size.x / cam.aspect;
            float distance = frustumHeight * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -distance);
            float distanceFromCam = Mathf.Abs(distance);
            Vector3 bgCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distanceFromCam));
            setBackgroundImg.transform.position = bgCenter;
        }
    }

    public void MoveWholeEnv() {
        if (listGroup.Count > 1) {
            if (EditorUtility.DisplayDialog("Info!", "Once your environment has moved, it will no longer be possible to work on it but just move it where you want.\n" +
                                                     "Any references to your items will be removed and it will not be possible to restore them.\n" +
                                                     "If you use Unity navigation, clear and re-bake.\n" +
                                                     "In orthographic projection you will need to set up a new character scale.\n" +
                                                     "Is this scene done?", "OK", "Cancel")) {
                GameObject MoveAllEnv = new GameObject("VBMoveAllEnv");
                if (isUseImgInScene) {
                    MoveAllEnv.transform.position = setBackgroundImg.transform.position;
                }
                else {
                    MoveAllEnv.transform.position = cam.transform.position;
                }
                VBMoveEnv.MoveAllObj();
                DestroyImmediate(MoveAllEnv);
                EditorUtility.DisplayDialog("Info!", "Environment has been moved. Save scene.\n" +
                                            "Create a new scene to continue.", "OK");
            }
        }
        else {
            GameObject MoveAllEnv = new GameObject("VBMoveAllEnv");
            if (isUseImgInScene) {
                MoveAllEnv.transform.position = setBackgroundImg.transform.position;
            }
            else {
                MoveAllEnv.transform.position = cam.transform.position;
            }
            VBMoveEnv.MoveAllObj();
            DestroyImmediate(MoveAllEnv);
            EditorUtility.DisplayDialog("Info!", "Environment has been moved. Save scene.\n" +
                                        "Create a new scene to continue.", "OK");
        }
    }

    public void ConfirmSpotPos() {
        if (ListOfPointLists.list[nGroup - 1].list[ListOfPointLists.list[nGroup - 1].list.Count - 1] == ListOfPointLists.list[nGroup - 1].list[0]
            && ListOfPointLists.list[nGroup - 1].list.Count - 1 > 0) {
            CreateAreasToBake();
        }
        else if (ListOfPointLists.list[nGroup - 1].list.Count - 1 == nSelectedSpot) {
            currentPosPoint = ListOfPointLists.list[nGroup - 1].list[nSelectedSpot];
        }
        isSelectedSpot = false;
        nSelectedSpot = -1;
    }

    public void RemoveCube() {
        isPlaceCube = false;
        isUseCube = false;
        showCubeEdges = false;
        isTranspCube = false;
        CubeUI = GameObject.Find("VBCubeUI");
        if (CubeUI != null) {
            DestroyImmediate(CubeUI);
        }
    }

    public void reset25dTKEnv() {
        isPlaceCube = false;
        isUseCube = false;
        isEnableMeter = false;
        vert = -1;
        cam.transform.position = posCamV3 = new Vector3(0f, 0f, -10f);
        cam.transform.rotation = Quaternion.Euler(new Vector3(3f, 0f, 0f));
        rotCam = new Vector3(3f, 0f, 0f);
        character.transform.position = posChar = new Vector3(0, -1, 1);
        character.transform.localScale = scaleCharV3 = Vector3.one;
    }

    public void ReloadObject() {
        string parent = "";
        if (moveSprite.GetComponent<SpriteRenderer>() != null) {
            SpriteRenderer renderer = moveSprite.GetComponent<SpriteRenderer>();
            string path = AssetDatabase.GetAssetPath(renderer.sprite);
            string spriteName = SpriteImg.name;
            GameObject removeObj = GameObject.Find(SpriteImg.name);
            if (removeObj.transform.parent != null) {
                parent = removeObj.transform.parent.name;
            }
            SpriteImg = null;
            moveSprite = null;
            DestroyImmediate(removeObj);
            moveSprite = new GameObject(spriteName);
            moveSprite.AddComponent<SpriteRenderer>();
            renderer = moveSprite.GetComponent<SpriteRenderer>();
            renderer.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
            if (parent != "") {
                GameObject findObj = GameObject.Find(parent);
                moveSprite.transform.SetParent(findObj.transform, false);
            }
            SpriteImg = moveSprite as Object;
            VBObjs._InitializeSprite();
            rotV3 = cam.transform.rotation.eulerAngles;
            VBObjs._RotSprite();
            Debug.Log("2.5D Toolkit:\nSprite successfully reloaded.");
        }
        else if (moveSprite.GetComponent<Animator>() != null) {
            GameObject removeObj = GameObject.Find(SpriteImg.name);
            string objName = SpriteImg.name;
            if (removeObj.transform.parent != null) {
                parent = removeObj.transform.parent.name;
            }
            Object getPath = PrefabUtility.GetCorrespondingObjectFromSource(removeObj);
            string path = AssetDatabase.GetAssetPath(getPath);
            SpriteImg = null;
            moveSprite = null;
            DestroyImmediate(removeObj);
            moveSprite = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(path, typeof(GameObject))) as GameObject;
            moveSprite.name = objName;
            if (parent != "") {
                GameObject findObj = GameObject.Find(parent);
                moveSprite.transform.SetParent(findObj.transform, false);
            }
            SpriteImg = moveSprite as Object;
            VBObjs._InitializeSprite();
            Debug.Log("2.5D Toolkit:\n3D object successfully reloaded.");
        }
        posV3 = moveSprite.transform.position;
    }

    void MakeTranspCube() {
        string matPath = "CubeUI/Materials/transparent";
        Material cubeMat = Resources.Load(matPath) as Material;
        GameObject childCube = CubeUI.transform.GetChild(0).gameObject;
        GameObject side;
        for (int i = 0; i <= 5; i++) {
            side = childCube.transform.GetChild(i).gameObject;
            MeshRenderer mr = side.GetComponent<MeshRenderer>();
            mr.sharedMaterial = cubeMat;
        }
        showCubeEdges = true;
    }

    void RevertTranspCube() {
        string matPathBlue = "CubeUI/Materials/blue";
        string matPathGreen = "CubeUI/Materials/green";
        string matPathRed = "CubeUI/Materials/red";
        Material cubeMatBlue = Resources.Load(matPathBlue) as Material;
        Material cubeMatGreen = Resources.Load(matPathGreen) as Material;
        Material cubeMatRed = Resources.Load(matPathRed) as Material;
        GameObject childCube = CubeUI.transform.GetChild(0).gameObject;
        GameObject side;
        MeshRenderer mr;
        side = childCube.transform.GetChild(0).gameObject;
        mr = side.GetComponent<MeshRenderer>();
        mr.sharedMaterial = cubeMatRed;
        side = childCube.transform.GetChild(1).gameObject;
        mr = side.GetComponent<MeshRenderer>();
        mr.sharedMaterial = cubeMatBlue;
        side = childCube.transform.GetChild(2).gameObject;
        mr = side.GetComponent<MeshRenderer>();
        mr.sharedMaterial = cubeMatRed;
        side = childCube.transform.GetChild(3).gameObject;
        mr = side.GetComponent<MeshRenderer>();
        mr.sharedMaterial = cubeMatGreen;
        side = childCube.transform.GetChild(4).gameObject;
        mr = side.GetComponent<MeshRenderer>();
        mr.sharedMaterial = cubeMatGreen;
        side = childCube.transform.GetChild(5).gameObject;
        mr = side.GetComponent<MeshRenderer>();
        mr.sharedMaterial = cubeMatBlue;
        showCubeEdges = false;
    }

    private void RestoreGridTilemap() {
        if (cam != null && gridTilemap != null) {
            isRotate90Tilemap = false;
            cam.transform.position = posCamV3 = camTilemapPos;
            cam.transform.rotation = Quaternion.Euler(camTilemapRot);
            rotCam = new Vector3(cam.transform.rotation.x, cam.transform.rotation.y, cam.transform.rotation.z);
            gridTilemap.transform.position = gridTilemapPos;
            gridTilemap.transform.rotation = Quaternion.Euler(gridTilemapRot);
        }
    }

    // Draw Gizmos
    private void OnDrawGizmos() {
        if (cam != null) {
            if (isViewFrustum) {
                Matrix4x4 temp = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(cam.transform.position, cam.transform.rotation, Vector3.one);
                if (cam.orthographic) {
                    float spread = cam.farClipPlane - cam.nearClipPlane;
                    float center = (cam.farClipPlane + cam.nearClipPlane) * 0.5f;
                    Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(cam.orthographicSize * 2 * cam.aspect, cam.orthographicSize * 2, spread));
                }
                else {
                    Gizmos.DrawFrustum(new Vector3(0, 0, (cam.nearClipPlane)), cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
                }
                Gizmos.matrix = temp;
            }
        }

        if (moveSprite != null && !isHidePivot) {
            Vector3 pivotSize = Vector3.zero;
            if (!cam.orthographic) {
                pivotSize = Vector3.one * (Mathf.Abs((new Plane(cam.transform.forward, cam.transform.position).GetDistanceToPoint(moveSprite.transform.position)) / spotSize));
            }
            else {
                pivotSize = ((new Vector3(0.7f, 0.7f, 0.7f) * (cam.orthographicSize)) / (spotSize));
            }
            Gizmos.color = Color.green;
            Gizmos.DrawCube(moveSprite.transform.position, pivotSize * 1.2f);
            if (isAutoPosObj && isMoveObject) {
                handleObjPivot = mousePositionWorld;
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(handleObjPivot, pivotSize * 1.2f);
                Gizmos.DrawLine(moveSprite.transform.position, handleObjPivot);
            }
            else if (isAutoPosObj) {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(handleObjPivot, pivotSize * 1.2f);
                Gizmos.DrawLine(moveSprite.transform.position, handleObjPivot);
            }
        }

        if (isUseCube && showCubeEdges && !Application.isPlaying) {
            CalcPositons();
            DrawBox();
        }

        if ((isBeginWork || floorTilemap != null) && !isInitMissing) {
            //If meter function is enabled
            if (isLabelAutoMeter) {
                if (labelDegrees == null) {
                    StyleDegrees();
                    whiteTex.Apply();
                }
                Vector3 leftTop = cam.ScreenToWorldPoint(new Vector3(30f, Screen.height - 30f, cam.nearClipPlane));
                if (cam.orthographic && isUseProportions) {
                    float height = 2f * cam.orthographicSize;
                    float width = height * cam.aspect;
                    float x = (linkToMeter * width) / saveForMeter;
                    float nPropWidth = (nStoreWidth * x) / width;
                    if (float.IsNaN(nPropWidth)) {
                        nPropWidth = 0.0f;
                    }
                    float nPropDepth = (nStoreDepth * x) / width;
                    if (float.IsNaN(nPropDepth)) {
                        nPropDepth = 0.0f;
                    }
                    Handles.Label(leftTop, "Width: " + nPropWidth + "  Depth: " + nPropDepth, labelDegrees);
                    if (!isHideLineAutoMeter) {
                        Handles.DrawAAPolyLine(1 * 5, new Vector3[] { storeWidthPos[0], storeWidthPos[1] });
                        Handles.DrawAAPolyLine(1 * 5, new Vector3[] { storeDepthPos[0], storeDepthPos[1] });
                    }
                    SceneView.RepaintAll();
                }
                else {
                    if (float.IsNaN(nStoreWidth)) {
                        nStoreWidth = 0.0f;
                    }
                    if (float.IsNaN(nStoreDepth)) {
                        nStoreDepth = 0.0f;
                    }
                    Handles.Label(leftTop, "Width: " + nStoreWidth + "  Depth: " + nStoreDepth, labelDegrees);
                    if (!isHideLineAutoMeter) {
                        Handles.DrawAAPolyLine(1 * 5, new Vector3[] { storeWidthPos[0], storeWidthPos[1] });
                        Handles.DrawAAPolyLine(1 * 5, new Vector3[] { storeDepthPos[0], storeDepthPos[1] });
                    }
                    SceneView.RepaintAll();
                }
            }
            if (drawGizmosLineMeter && selectedTab == 1) {
                if (startMeterPoint != Vector3.zero) {
                    Handles.color = lineColor;
                    if (isSetProportions) {
                        Handles.color = Color.yellow;
                        keepHorz = new Vector3(mousePositionWorld.x, startMeterPoint.y, startMeterPoint.z);
                        Handles.DrawAAPolyLine(1 * 5, new Vector3[] { startMeterPoint, keepHorz });
                    }
                    else {
                        Handles.color = Color.green;
                        Handles.DrawAAPolyLine(1 * 5, new Vector3[] { startMeterPoint, mousePositionWorld });
                    }
                }
            }
            var center = new Vector3(0, vert, 0);
            if (!cam.orthographic && !Application.isPlaying && gridTilemap == null) {
                bool wrongSide = false;
                if ((vert >= cam.transform.position.y)) {
                    wrongSide = true;
                }
                Vector3 q = cam.WorldToViewportPoint(new Vector3(0.0f, vert, cam.farClipPlane));
                Vector3 v3Horz = new Vector3(0.0f, q.y, cam.farClipPlane);
                Vector3 v3HorzLeft = cam.ViewportToWorldPoint(v3Horz);
                v3Horz = new Vector3(1f, q.y, cam.farClipPlane);
                Vector3 v3HorzRight = cam.ViewportToWorldPoint(v3Horz);
                if (!wrongSide) {
                    Handles.color = Color.yellow;
                }
                else {
                    Handles.color = Color.red;
                }
                Handles.DrawAAPolyLine(1 * 5, new Vector3[] { v3HorzLeft, v3HorzRight });
            }
            if (!isShowFloor) {
                //
            }
            else {
                Handles.color = Color.yellow;
                Gizmos.color = floorColor;
                var matrix = Gizmos.matrix;
                if (horPlane.normal != Vector3.zero) {
                    Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.LookRotation(horPlane.normal), Vector3.one);
                }
                int count = 0;
                for (int i = -nSquare; i <= nSquare; i++) {
                    count++;
                    Gizmos.DrawLine(new Vector3(i, Mathf.Abs(cam.transform.position.z), 0),
                                    new Vector3(i, Mathf.Abs(cam.transform.position.z) - (nSquare), 0));
                }
                Gizmos.DrawLine(new Vector3(-nSquare, Mathf.Abs(cam.transform.position.z), 0),
                                new Vector3((nSquare), Mathf.Abs(cam.transform.position.z), 0));
                for (int i = 1; i <= nSquare; i++) {
                    Gizmos.DrawLine(new Vector3(-nSquare, Mathf.Abs(cam.transform.position.z) - Mathf.Abs((2 * i) / 2), 0),
                                    new Vector3((nSquare), Mathf.Abs(cam.transform.position.z) - Mathf.Abs((2 * i) / 2), 0));
                }
                Gizmos.matrix = matrix;
            }
        }
        if (listGroup.Count > 1) {
            Gizmos.color = lineColor;
            Handles.color = lineColor;
            if (hideLabels) {
                for (int i = 0; i < ListOfPointLists.list.Count; i++) {
                    if (ListOfPointLists.list[i].list.Count > 0) {
                        if (labelStyle == null) {
                            SetStyle();
                            whiteTex.Apply();
                        }
                        Handles.Label(ListOfPointLists.list[i].list[0], "Area" + (i + 1).ToString("000"), labelStyle);
                        SceneView.RepaintAll();
                    }
                }
            }
            if (drawGizmosLine == true) {
                if (currentPosPoint != Vector3.zero) {
                    Vector3 linePos = Vector3.zero;
                    if (isKeepHorzLine || isKeepVertLine) {
                        bool lineStraight = false;
                        Vector2 worldToScreenLn = cam.WorldToScreenPoint(currentPosPoint);
                        Vector2 screenMousePos = mousePositionScreen;
                        if (isKeepHorzLine) {
                            Vector2 screenLn = new Vector2(screenMousePos.x, worldToScreenLn.y);
                            float mouseLnOffset = Vector2.Distance(screenLn, screenMousePos);
                            if (mouseLnOffset <= keepStraightOffset) {
                                if (!isRotate90Tilemap && floorTilemap != null) {
                                    keepStraightLine = new Vector3(mousePositionWorld.x, currentPosPoint.y, mousePositionWorld.z);
                                }
                                else {
                                    keepStraightLine = new Vector3(mousePositionWorld.x, mousePositionWorld.y, currentPosPoint.z);
                                }
                                lineStraight = true;
                            }
                        }
                        if (isKeepVertLine && !lineStraight) {
                            Vector2 screenLn = new Vector2(worldToScreenLn.x, screenMousePos.y);
                            float mouseLnOffset = Vector2.Distance(screenLn, screenMousePos);
                            if (mouseLnOffset <= keepStraightOffset) {
                                keepStraightLine = new Vector3(currentPosPoint.x, mousePositionWorld.y, mousePositionWorld.z);
                                lineStraight = true;
                            }
                        }
                        GetDegrees(linePos);
                        if (!lineStraight) {
                            keepStraightLine = linePos = mousePositionWorld;
                            Gizmos.DrawLine(currentPosPoint, keepStraightLine);
                            Gizmos.DrawLine(currentPosPoint - new Vector3(currentPosPoint.x - 0.5f, currentPosPoint.y - 0.5f, currentPosPoint.z - 0.5f),
                                            keepStraightLine - new Vector3(keepStraightLine.x - 0.5f, keepStraightLine.y - 0.5f, keepStraightLine.z - 0.5f));

                        }
                        else {
                            Handles.color = Color.blue;
                            Handles.DrawAAPolyLine(1 * 5, new Vector3[] { currentPosPoint, keepStraightLine });
                            linePos = keepStraightLine;
                        }
                    }
                    else {
                        Gizmos.DrawLine(currentPosPoint, mousePositionWorld);
                        Gizmos.DrawLine(currentPosPoint - new Vector3(currentPosPoint.x - 0.5f, currentPosPoint.y - 0.5f, currentPosPoint.z - 0.5f),
                                        mousePositionWorld - new Vector3(mousePositionWorld.x - 0.5f, mousePositionWorld.y - 0.5f, mousePositionWorld.z - 0.5f));
                        linePos = mousePositionWorld;
                    }
                    if (isShowLineDegrees) {
                        GetDegrees(linePos);
                    }
                }
            }
            else if (lineDegrees != "0.0") {
                lineDegrees = "0.0";
                lineTo90 = "0.0";
            }
            if (!isUnityNav && listGroup.Count > 1) {
                dragAndCloseArea = false;
                isMouseOverLine = false;
                fromToSpot[0] = -1;
                fromToSpot[1] = -1;
                if (!moveSpot) {
                    isMouseOverSpot = false;
                }
                for (int k = 1; k <= ListOfPointLists.list.Count; k++) {
                    int listSize = ListOfPointLists.list[k - 1].list.Count;
                    for (int b = 0; b <= listSize - 1; b++) {
                        if (listSize >= 1) {
                            Vector3 p1 = ListOfPointLists.list[k - 1].list[b];
                            if (listSize >= 2 && (b + 1 < listSize)) {
                                Vector3 p2 = ListOfPointLists.list[k - 1].list[b + 1];
                                if (nGroup != 0) {
                                    if (k != nGroup) {
                                        Handles.color = inactiveLineColor;
                                    }
                                    else {
                                        Handles.color = lineColor;
                                    }
                                }
                                else {
                                    if (listIsWalkable[k - 1] == 0) {
                                        Handles.color = walkCol;
                                    }
                                    else {
                                        Handles.color = notWalkCol;
                                    }
                                }
                                Handles.DrawAAPolyLine(1 * 2, new Vector3[] { p1, p2 });
                                if (selectedTab == 2 && k == nGroup && !isSelectedSpot) {
                                    Vector2 worldToScreenP1 = cam.WorldToScreenPoint(p1);
                                    Vector2 worldToScreenP2 = cam.WorldToScreenPoint(p2);
                                    Vector2 screenSpotP1 = new Vector2(worldToScreenP1.x, worldToScreenP1.y);
                                    Vector2 screenSpotP2 = new Vector2(worldToScreenP2.x, worldToScreenP2.y);
                                    Vector2 screenMousePosP1P2 = mousePositionScreen;
                                    Vector2 segmentDirection = (screenSpotP2 - screenSpotP1).normalized;
                                    Vector2 point1ToMouse = screenMousePosP1P2 - screenSpotP1;
                                    Vector2 mouseToPoint2 = screenSpotP2 - screenMousePosP1P2;
                                    if (Vector2.Dot(segmentDirection, point1ToMouse.normalized) >= 0f && Vector2.Dot(segmentDirection, mouseToPoint2.normalized) >= 0f) {
                                        Vector2 closestPointOnSegment = screenSpotP1 + segmentDirection * Vector2.Dot(point1ToMouse, segmentDirection);
                                        if ((closestPointOnSegment - screenMousePosP1P2).magnitude <= radiusMouseLine && selectedSpot[0] == -1 && !drawGizmosLine && !isMouseOverLine) {
                                            if (nGroup != k) {
                                                //
                                            }
                                            Handles.DrawAAPolyLine(1 * 5, new Vector3[] { p1, p2 });
                                            isMouseOverLine = true;
                                            if (showSpotOnLine) {
                                                float firstSpot = Vector3.Distance(p1, mousePositionWorld);
                                                float secondSpot = Vector3.Distance(p2, mousePositionWorld);
                                                float sum = firstSpot + secondSpot;
                                                float vector3Lerp = firstSpot / sum;
                                                Vector3 newVector3 = Vector3.Lerp(p1, p2, vector3Lerp);
                                                insertSpot = new Vector3(Mathf.Round(newVector3.x * 1000f) / 1000f,
                                                                         Mathf.Round(newVector3.y * 1000f) / 1000f,
                                                                         Mathf.Round(newVector3.z * 1000f) / 1000f);
                                                Vector3 SpotOnLine;
                                                if (!cam.orthographic) {
                                                    SpotOnLine = Vector3.one * (Mathf.Abs((new Plane(cam.transform.forward, 
                                                                                                     cam.transform.position).GetDistanceToPoint(insertSpot)) / spotSize));
                                                }
                                                else {
                                                    SpotOnLine = ((new Vector3(0.7f, 0.7f, 0.7f) * (cam.orthographicSize)) / (spotSize));
                                                }
                                                Gizmos.DrawCube(insertSpot, SpotOnLine);
                                                fromToSpot[0] = k - 1;
                                                fromToSpot[1] = b + 1;
                                            }
                                        }
                                    }
                                }
                            }
                            Vector3 SpotSize;
                            if (!cam.orthographic) {
                                SpotSize = Vector3.one * (Mathf.Abs((new Plane(cam.transform.forward, 
                                                                               cam.transform.position).GetDistanceToPoint(p1)) / spotSize));
                            }
                            else {
                                SpotSize = ((new Vector3(0.7f, 0.7f, 0.7f) * (cam.orthographicSize)) / (spotSize));
                            }
                            if (selectedTab == 2 && k == nGroup && !isSelectedSpot) {
                                Gizmos.color = lineColor;
                                Vector2 worldToScreenSpot = cam.WorldToScreenPoint(p1);
                                Vector2 screenSpot = new Vector2(worldToScreenSpot.x, worldToScreenSpot.y);
                                Vector2 screenMousePos = mousePositionScreen;
                                float mouseOverSpot = Vector2.Distance(screenSpot, screenMousePos);
                                if (mouseOverSpot <= radiusMouseSpot) {
                                    if (!drawGizmosLine && !moveSpot) {
                                        Gizmos.color = mouseOverSpotCol;
                                        onHoverSpot = true;
                                        selectedSpot[0] = k - 1;
                                        if (selectedTab == 1) {
                                            if (nGroup != k) {
                                                //
                                            }
                                        }
                                        if (ListOfPointLists.list[k - 1].list[0] == ListOfPointLists.list[k - 1].list[b] && b == listSize - 1) {
                                            firstIsLast = true;
                                            selectedSpot[1] = 0;
                                        }
                                        else {
                                            selectedSpot[1] = b;
                                        }
                                        isMouseOverSpot = true;
                                    }
                                    else if ((drawGizmosLine || moveSpot) && ListOfPointLists.list[k - 1].list.Count >= 3) {
                                        if (drawGizmosLine) {
                                            if (b == 0) {
                                                Gizmos.color = mouseOverSpotCol;
                                                onHoverSpot = true;
                                                dragAndCloseArea = true;
                                                isMouseOverSpot = true;
                                            }
                                        }
                                        else if ((b == 0 && selectedSpot[1] == listSize - 1) ||
                                          (b == listSize - 1 && selectedSpot[1] == 0)) {
                                            Gizmos.color = mouseOverSpotCol;
                                            onHoverSpot = true;
                                            dragAndCloseArea = true;
                                            isMouseOverSpot = true;
                                        }
                                    }
                                }
                            }
                            else if (isSelectedSpot && k == nGroup) {
                                if (b == nSelectedSpot || (nSelectedSpot == 0 && b == listSize - 1)) {
                                    Gizmos.color = mouseOverSpotCol;
                                    onHoverSpot = true;
                                }
                            }
                            DefineSpotColor(k);
                            if (isWireSpot) {
                                Gizmos.DrawWireCube(p1, SpotSize);
                            }
                            else {
                                Gizmos.DrawCube(p1, SpotSize);
                            }

                        }
                    }
                }
                if (!isMouseOverSpot) {
                    selectedSpot[0] = -1;
                    selectedSpot[1] = -1;
                    firstIsLast = false;
                    dragAndCloseArea = false;
                }
            }
        }
    }

    private void DefineSpotColor(int k) {
        if (onHoverSpot) {
            Gizmos.color = mouseOverSpotCol;
        }
        else {
            if (nGroup != 0) {
                if (k != nGroup) {
                    Gizmos.color = inactiveLineColor;
                }
                else {
                    Gizmos.color = lineColor;
                }
            }
            else {
                if (listIsWalkable[k - 1] == 0) {
                    Gizmos.color = new Color(walkCol.r, walkCol.g, walkCol.b);
                }
                else {
                    Gizmos.color = Gizmos.color = new Color(notWalkCol.r, notWalkCol.g, notWalkCol.b);
                }
            }
        }
        onHoverSpot = false;
    }

    private void DefineLinesColor(int k) {
        if (nGroup != 0) {
            if (k != nGroup) {
                Gizmos.color = inactiveLineColor;
            }
            else {
                Gizmos.color = lineColor;
            }
        }
        else {
            if (listIsWalkable[k - 1] == 0) {
                Gizmos.color = new Color(walkCol.r, walkCol.g, walkCol.b);
            }
            else {
                Gizmos.color = new Color(notWalkCol.r, notWalkCol.g, notWalkCol.b);
            }
        }
    }

    // Style of notes
    private void SetStyle() {
        labelStyle = new GUIStyle("Box");
        whiteTex = new Texture2D(1, 1);
        Color backgroundCol = new Color(boxColor.r, boxColor.g, boxColor.b);
        whiteTex.SetPixel(0, 0, backgroundCol);
        labelStyle.normal.background = whiteTex;
        labelStyle.fontSize = fontSize;
        labelStyle.fontStyle = FontStyle.Normal;
        labelStyle.fixedHeight = 0;
        labelStyle.padding = new RectOffset(4, 0, 4, 0);
        labelStyle.wordWrap = true;
        labelStyle.alignment = TextAnchor.UpperLeft;
        Color textCol = new Color(textColor.r, textColor.g, textColor.b);
        labelStyle.normal.textColor = textCol;
    }

    private void StyleDegrees() {
        labelDegrees = new GUIStyle("Box");
        whiteTex = new Texture2D(1, 1);
        Color backgroundCol = new Color(boxColor.r, boxColor.g, boxColor.b);
        whiteTex.SetPixel(0, 0, backgroundCol);
        labelDegrees.normal.background = whiteTex;
        labelDegrees.fontSize = fontSizeDegrees;
        labelDegrees.fontStyle = FontStyle.Normal;
        labelDegrees.fixedHeight = 0;
        labelDegrees.padding = new RectOffset(4, 0, 4, 0);
        labelDegrees.wordWrap = true;
        labelDegrees.alignment = TextAnchor.UpperLeft;
        Color textCol = new Color(textColor.r, textColor.g, textColor.b);
        labelDegrees.normal.textColor = textCol;
    }

    public void InitializeEditor() {
        if (lineColor.a == 0)
            lineColor = Color.red;
        if (inactiveLineColor.a == 0)
            inactiveLineColor = Color.blue;
        if (mouseOverSpotCol.a == 0)
            mouseOverSpotCol = Color.black;
        if (floorColor.a == 0)
            floorColor = Color.cyan;
        if (boxColor.a == 0)
            boxColor = Color.yellow;
        if (textColor.a == 0)
            textColor = Color.black;
        if (walkCol.a == 0)
            walkCol = new Color(0, 1, 1, 0.2f);
        if (notWalkCol.a == 0)
            notWalkCol = new Color(1, 0, 1, 0.2f);
        if (meshCol.a == 0)
            meshCol = new Color(1, 1, 1, 1);
        if (use25dtkEnv) {
            isBeginWork = true;
        }
        transform.hideFlags = HideFlags.HideInInspector;
        if (cam != null) {
            posCamV3 = cam.transform.position;
            rotCam = TransformUtils.GetInspectorRotation(cam.transform);
            if (cam.orthographic) {
                sizeCam = cam.orthographicSize;
            }
            else {
                fovCam = cam.fieldOfView;
            }
        }
        if (setBackgroundImg != null) {
            posBGV3 = setBackgroundImg.transform.position;
            RotBGV3 = TransformUtils.GetInspectorRotation(setBackgroundImg.transform);
        }
        if (character != null) {
            posChar = character.transform.position;
            rotChar = TransformUtils.GetInspectorRotation(character.transform);
            scaleCharV3 = character.transform.localScale;
        }
        if (SpriteImg != null) {
            if (moveSprite == null) {
                moveSprite = GameObject.Find(SpriteImg.name);
            }
            posV3 = moveSprite.transform.position;
            rotV3 = TransformUtils.GetInspectorRotation(moveSprite.transform);
        }
    }

    public void InitializeVB25dTK() {
        if (GameObject.Find("VBAreaTK") == null) {
            newMainObj = new GameObject("VBAreaTK");
            newMainObj.transform.position = Vector3.zero;
        }
        if (cam != null) {
            if (cam.orthographic) {
                isCamOrtho = true;
            }
        }
        ChangeCursor();
        if (listGroup.Count == 0) {
            listGroup.Add("none");
        }
        if (listNearPointPos.Count == 0) {
            listNearPointPos.Add(Vector3.zero);
            listNearPointPos.Add(Vector3.zero);
        }
        if (listNearPointScale.Count == 0) {
            listNearPointScale.Add(Vector3.one);
            listNearPointScale.Add(Vector3.one);
        }
        drawGizmosLine = false;
        labelStyle = null;
        labelDegrees = null;
        isSelectPoint = isSelectedSpot = false;
        nSelectedSpot = -1;
        SceneView.RepaintAll();
        walkMat = null;
        walkMat = new Material(Shader.Find("Standard"));
        walkMat.color = new Color(walkCol.r, walkCol.g, walkCol.b, walkCol.a);
        notwalkMat = null;
        notwalkMat = new Material(Shader.Find("Standard"));
        notwalkMat.color = new Color(notWalkCol.r, notWalkCol.g, notWalkCol.b, notWalkCol.a);
        isExitingEditMode = false;
        isEnableChar = true;
        nGroup = 0;
        currentState = "Not editing.";
        lineDegrees = "0.0";
        lineTo90 = "0.0";
        if (treeState != null) {
            if (treeState.Length < 3)
                treeState = new bool[] { false, false, false };
        }
        else {
            treeState = new bool[] { false, false, false };
        }
    }

    void CalcPositons() {
        Bounds bounds;
        GameObject cubeChild = GameObject.Find("Cube");
        BoxCollider bc = cubeChild.GetComponent<BoxCollider>();
        if (bc != null) {
            bounds = cubeChild.GetComponent<MeshFilter>().sharedMesh.bounds;
        }
        else {
            return;
        }
        Vector3 v3Center = bounds.center;
        Vector3 v3Extents = bounds.extents;
        v3FrontTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        v3FrontTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        v3FrontBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        v3BackTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        v3BackTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        v3BackBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        v3BackBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

        v3FrontTopLeft = cubeChild.transform.TransformPoint(v3FrontTopLeft);
        v3FrontTopRight = cubeChild.transform.TransformPoint(v3FrontTopRight);
        v3FrontBottomLeft = cubeChild.transform.TransformPoint(v3FrontBottomLeft);
        v3FrontBottomRight = cubeChild.transform.TransformPoint(v3FrontBottomRight);
        v3BackTopLeft = cubeChild.transform.TransformPoint(v3BackTopLeft);
        v3BackTopRight = cubeChild.transform.TransformPoint(v3BackTopRight);
        v3BackBottomLeft = cubeChild.transform.TransformPoint(v3BackBottomLeft);
        v3BackBottomRight = cubeChild.transform.TransformPoint(v3BackBottomRight);
    }

    void DrawBox() {
        Color color = Color.green;
        Debug.DrawLine(v3FrontTopLeft, v3FrontTopRight, color);
        Debug.DrawLine(v3FrontTopRight, v3FrontBottomRight, color);
        Debug.DrawLine(v3FrontBottomRight, v3FrontBottomLeft, color);
        Debug.DrawLine(v3FrontBottomLeft, v3FrontTopLeft, color);

        Debug.DrawLine(v3BackTopLeft, v3BackTopRight, color);
        Debug.DrawLine(v3BackTopRight, v3BackBottomRight, color);
        Debug.DrawLine(v3BackBottomRight, v3BackBottomLeft, color);
        Debug.DrawLine(v3BackBottomLeft, v3BackTopLeft, color);

        Debug.DrawLine(v3FrontTopLeft, v3BackTopLeft, color);
        Debug.DrawLine(v3FrontTopRight, v3BackTopRight, color);
        Debug.DrawLine(v3FrontBottomRight, v3BackBottomRight, color);
        Debug.DrawLine(v3FrontBottomLeft, v3BackBottomLeft, color);
    }

    void GetDegrees(Vector3 posLine) {
        float x = currentPosPoint.x - posLine.x;
        float y = 0.0f;
        if (!isRotate90Tilemap && floorTilemap != null) {
            y = currentPosPoint.y - posLine.y;
        }
        else {
            y = currentPosPoint.z - posLine.z;
        }
        float realValue = (float)((System.Math.Atan2(x, y) / System.Math.PI) * 180f);
        float value = Mathf.Round(realValue * 10f) / 10f;
        float valueTo90 = Mathf.Abs(Mathf.Abs(value) - 90);
        if (value < 0) {
            value += 360f;
        }
        lineDegrees = value.ToString("F1");
        if (lineDegrees.Length == 3) {
            lineDegrees = "0" + lineDegrees;
        }
        lineTo90 = valueTo90.ToString("F1");
        if (lineTo90.Length == 3) {
            lineTo90 = "0" + lineTo90;
        }
        if (isLabelDegrees) {
            if (labelDegrees == null) {
                StyleDegrees();
                whiteTex.Apply();
            }
            switch (nPlaceLabelDegrees) {
                case 0:
                    isShowFixedDegree = false;
                    break;
                case 1:
                    isShowFixedDegree = true;
                    break;
            }
            if (isShowFixedDegree) {
                Vector3 leftTop = cam.ScreenToWorldPoint(new Vector3(30f, Screen.height - 30f, cam.nearClipPlane));
                Handles.Label(leftTop, lineTo90 + "   (" + lineDegrees + ")", labelDegrees);
                SceneView.RepaintAll();
            }
            else {
                float nOffset = 40f;
                Vector3 labelOffset = cam.ScreenToWorldPoint(new Vector3(mousePositionScreen.x + nOffset, mousePositionScreen.y, cam.nearClipPlane));
                Handles.Label(labelOffset, lineTo90 + "   (" + lineDegrees + ")", labelDegrees);
                SceneView.RepaintAll();
            }
        }
    }

    private void OnEnable() {
        scene = SceneManager.GetActiveScene().name;
        VBObjs = new VBObjsTK(this);
        VBFiles = new VBFilesTK(this);
        VBNav = new VBNavTK(this);
        VBMoveEnv = new VBMoveEnvTK(this);
        EditorApplication.playModeStateChanged += OnEditorChangedPlayMode;
        EditorSceneManager.sceneOpened += EditorSceneManager_sceneOpened;
    }

    private void OnDisable() {
        EditorApplication.playModeStateChanged -= OnEditorChangedPlayMode;
        EditorSceneManager.sceneOpened -= EditorSceneManager_sceneOpened;
    }
}
