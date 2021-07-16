/* 
 * This is a simple script to move character with mouse click which can be optimized.
 * It is provided for the purpose of character's movement testing in the scene built with 2.5D Toolkit.
 * It is later recommended to use scripts provided with the various assets to create games that offer greater functions.
 * Click on plane or mesh to move character, double click to run.
*/

using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]

public class CharControl : MonoBehaviour {
    public float constWalk = 1;
    public float trueConstWalk;
    // The further the character moves away from camera, the more it slows down
    // You can disable this behavior
    public bool disableSlowDown;
    public float constRun = 3;
    public Animator anim;
    private Vector3 destinationPosition;
    private Transform myTransform;
    private GameObject VB25d;
    private bool useCharScale;
    private bool activeWork;
    private bool isRunning;
    private bool drawLine;
    private NavMeshAgent agent;
    private NavMeshPath path;
    private string scene;
    private float InitialTouch;
    private float diffScale;
    private float startZ;
    private float diffZ;

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Input.GetMouseButtonDown(0)) {
            if (Time.time < InitialTouch + 0.5f) {
                isRunning = true;
            }
            InitialTouch = Time.time;
            if (!isRunning) {
                if (Physics.Raycast(ray, out hitInfo)) {
                    activeWork = true;
                    destinationPosition = hitInfo.point;
                }
                if (activeWork) {
                    if (NavMesh.CalculatePath(transform.position, destinationPosition, NavMesh.AllAreas, path)) {
                        Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
                        myTransform.rotation = targetRotation;
                        drawLine = true;
                        if (isRunning) {
                            agent.speed = constRun;
                            anim.SetFloat("MoveSpeed", constRun);
                        }
                        else {
                            agent.speed = constWalk;
                            anim.SetFloat("MoveSpeed", constWalk);
                        }
                        agent.SetPath(path);
                    }
                }
                activeWork = false;
            }
            else {
                agent.speed = constRun;
                anim.SetFloat("MoveSpeed", constRun);
            }
        }
        if (agent.speed > 0) {
            if (useCharScale) {
                if (myTransform.position.z > startZ) {
                    float c = (((myTransform.position.z - startZ) * diffScale) / diffZ);
                    myTransform.localScale -= new Vector3(c, c, c);
                }
                else {
                    float c = (((startZ - myTransform.position.z) * diffScale) / diffZ);
                    myTransform.localScale += new Vector3(c, c, c);
                }
                startZ = myTransform.position.z;
                if (!isRunning && !disableSlowDown) {
                    trueConstWalk = (constWalk * myTransform.localScale.z) / VB25d.GetComponent<VB25dTK>().listNearPointScale[1].z;
                    if (trueConstWalk > constWalk) {
                        trueConstWalk = constWalk;
                    }
                    agent.speed = trueConstWalk;
                    anim.SetFloat("MoveSpeed", trueConstWalk);
                }
            }
            if (!agent.pathPending && !agent.hasPath) {
                agent.speed = 0f;
                anim.SetFloat("MoveSpeed", 0f);
                isRunning = false;
                drawLine = false;
            }
        }
        if (drawLine) {
            NavMesh.CalculatePath(transform.position, destinationPosition, NavMesh.AllAreas, path);
            for (int i = 0; i < path.corners.Length - 1; i++) {
                if (!VB25d.GetComponent<VB25dTK>().isHidePath) {
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                }
            }
        }
    }

    void Start() {
        VB25d = GameObject.Find("VB25dTK");
        anim = GetComponent<Animator>();
        myTransform = transform;
        useCharScale = false;
        if (VB25d.GetComponent<VB25dTK>().isSetUpScaling && VB25d.GetComponent<VB25dTK>().nScalePoints == 1 
                && VB25d.GetComponent<VB25dTK>().cam.orthographic) {
            myTransform.localScale = VB25d.GetComponent<VB25dTK>().listNearPointScale[0];
            diffZ = VB25d.GetComponent<VB25dTK>().diffZ;
            diffScale = VB25d.GetComponent<VB25dTK>().diffScale;
            float ch = VB25d.GetComponent<VB25dTK>().listNearPointPos[0].z - myTransform.position.z;
            float c = ((ch * diffScale) / diffZ);
            myTransform.localScale += new Vector3(c, c, c);
            startZ = myTransform.position.z;
            useCharScale = true;
        }
        destinationPosition = myTransform.position;
        path = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
        scene = SceneManager.GetActiveScene().name;
        // set Animator and character speed for each demo scene
        switch (scene) {
            case "Test_Cableway_Orthographic":
                anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                                  "Assets/2.5dToolKit/Demo/Controller/Animator Controller.controller",
                                  typeof(RuntimeAnimatorController)
                                  );
                constWalk = 1.2f;
                constRun = 2.5f;
                break;
            case "Test_Cableway_Perspective":
                anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                                  "Assets/2.5dToolKit/Demo/Controller/Animator Controller.controller",
                                  typeof(RuntimeAnimatorController)
                                  );
                constWalk = 1f;
                constRun = 2f;
                break;
            case "Test_IBM":
                anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                                  "Assets/2.5dToolKit/Demo/Controller/Animator Controller.controller",
                                  typeof(RuntimeAnimatorController)
                                  );
                constWalk = 1.3f;
                constRun = 4f;
                break;
            case "Test_Mainframe":
                anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                                  "Assets/2.5dToolKit/Demo/Controller/Animator Controller.controller",
                                  typeof(RuntimeAnimatorController)
                                  );
                constWalk = 1.7f;
                constRun = 3.85f;
                break;
            case "Test_NicoHome":
                anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                                  "Assets/2.5dToolKit/Demo/Controller/Animator Controller 2.controller",
                                  typeof(RuntimeAnimatorController)
                                  );
                constWalk = 6.8f;
                constRun = 12f;
                break;
            case "Test_Pools":
                anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                                  "Assets/2.5dToolKit/Demo/Controller/Animator Controller IBM.controller",
                                  typeof(RuntimeAnimatorController)
                                  );
                constWalk = 2.1f;
                constRun = 5.1f;
                break;
            case "Test_RPG":
                anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                                  "Assets/2.5dToolKit/Demo/Controller/Animator Controller 3.controller",
                                  typeof(RuntimeAnimatorController)
                                  );
                constWalk = 0.7f;
                constRun = 1.5f;
                break;
            case "Test_Salon":
                anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                                  "Assets/2.5dToolKit/Demo/Controller/Animator Controller.controller",
                                  typeof(RuntimeAnimatorController)
                                  );
                constWalk = 1f;
                constRun = 2.3f;
                break;
            case "Test_Villa":
                anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                                  "Assets/2.5dToolKit/Demo/Controller/Animator Controller.controller",
                                  typeof(RuntimeAnimatorController)
                                  );
                constWalk = 1.8f;
                constRun = 3.3f;
                break;
        }
    }
}
