using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;

class VBFilesTK {
    private VB25dTK VB25d;
    public VBFilesTK(VB25dTK target) {
        VB25d = target;
    }
    private List<string> allData = new List<string>();
    private string isChild;

    public void ReadFiles(string PathDataArea) {
        if (!Directory.Exists(PathDataArea)) {
            return;
        }
        int nScene = 0;
        string scene = SceneManager.GetActiveScene().name;
        string fileName;
        DirectoryInfo dir = new DirectoryInfo(PathDataArea);
        FileInfo[] info = dir.GetFiles("*.txt");
        int nFiles = info.Length;
        if (nFiles > 0) {
            VB25d.nGroup = 0;
            if (VB25d.ListOfPointLists.list == null) {
                VB25d.ListOfPointLists.list = new List<Point>();
            }
            else {
                VB25d.ListOfPointLists.list.Clear();
                VB25d.listIsWalkable.Clear();
                VB25d.listGroup.Clear();
            }
            for (int i = 1; i <= nFiles; i++) {
                fileName = Path.GetFileNameWithoutExtension(info[i - 1].ToString());
                if (fileName.Contains(scene)) {
                    nScene++;
                    VB25d.ListOfPointLists.list.Add(new Point());
                    VB25d.ListOfPointLists.list[nScene - 1].list = new List<Vector3>();
                    string nameArea = "";
                    nameArea = scene + "Area" + nScene.ToString() + ".txt";
                    string line;
                    using (StreamReader reader = new StreamReader(PathDataArea + nameArea, false)) {
                        VB25d.listIsWalkable.Add(int.Parse(reader.ReadLine()));
                        while ((line = reader.ReadLine()) != null) {
                            Vector3 v3xyz = Vector3.zero;
                            string[] myData = line.Split(';');
                            v3xyz.x = float.Parse(myData[0]);
                            v3xyz.y = float.Parse(myData[1]);
                            v3xyz.z = float.Parse(myData[2]);
                            VB25d.ListOfPointLists.list[nScene - 1].list.Add(v3xyz);
                        }
                        reader.Close();
                    }
                }
            }
            VB25d.listGroup.Add("none");
            for (int i = 1; i <= nScene; i++) {
                VB25d.listGroup.Add(i.ToString());
            }
            VB25d.nGroup = 0;
            if (VB25d.listGroup.Count > 1) {
                Debug.Log("2.5D Toolkit:\nAreas data loaded from 2.5dToolKit/VBDataAreaTK/[scenename] folder.");
            }
        }
    }

    public void MakeFiles(string PathMakeFile) {
        string scene = SceneManager.GetActiveScene().name;
        int count = 1;
        for (int i = 1; i <= VB25d.listGroup.Count - 1; i++) {
            if (VB25d.ListOfPointLists.list != null) {
                if (VB25d.ListOfPointLists.list[i - 1].list.Count > 0) {
                    if (VB25d.ListOfPointLists.list[i - 1].list.IndexOf(VB25d.ListOfPointLists.list[i - 1].list[0], 1) > 0) {
                        string nameArea = scene + "Area" + count.ToString() + ".txt";
                        string path_ = PathMakeFile + nameArea;
                        using (StreamWriter writer = new StreamWriter(path_, false)) {
                            writer.WriteLine(VB25d.listIsWalkable[i - 1]);
                            for (int b = 0; b < VB25d.ListOfPointLists.list[i - 1].list.Count; b++) {
                                string v3x = VB25d.ListOfPointLists.list[i - 1].list[b].x.ToString();
                                string v3y = VB25d.ListOfPointLists.list[i - 1].list[b].y.ToString();
                                string v3z = VB25d.ListOfPointLists.list[i - 1].list[b].z.ToString();
                                writer.WriteLine(v3x + ";" + v3y + ";" + v3z);
                            }
                            writer.Flush();
                            writer.Close();
                            count++;
                        }
                    }
                }
            }
        }
        if (count > 1) {
            Debug.Log("2.5D Toolkit:\nAreas data saved to 2.5dToolKit/VBDataAreaTK/[scenename] folder.");
        }
        EditorApplication.RepaintProjectWindow();
    }

    public void Traverse(GameObject obj) {
        if (obj.name != "VB25dTK" && obj.name != "VBAreaTK") {
            if (GameObject.Find(obj.name)) {
                isChild = "";
                GameObject foundObj = GameObject.Find(obj.name);
                if (foundObj.transform.root != foundObj.transform) {
                    isChild = "      ";
                }
                allData.Add(isChild + "[" + foundObj.name + "]");
                string v3x = foundObj.transform.position.x.ToString();
                string v3y = foundObj.transform.position.y.ToString();
                string v3z = foundObj.transform.position.z.ToString();
                allData.Add(isChild + "Position (" + v3x + "; " + v3y + "; " + v3z + ")");
                v3x = foundObj.transform.rotation.eulerAngles.x.ToString();
                v3y = foundObj.transform.rotation.eulerAngles.y.ToString();
                v3z = foundObj.transform.rotation.eulerAngles.z.ToString();
                allData.Add(isChild + "Rotation (" + v3x + "; " + v3y + "; " + v3z + ")");
                v3x = foundObj.transform.localScale.x.ToString();
                v3y = foundObj.transform.localScale.y.ToString();
                v3z = foundObj.transform.localScale.z.ToString();
                allData.Add(isChild + "Scale (" + v3x + "; " + v3y + "; " + v3z + ")");
                if ((foundObj.name == "Main Camera") || (foundObj.name == "VBBGCamera")) {
                    string space = "      ";
                    allData.Add(space + "Clear Flags (" + obj.GetComponent<Camera>().clearFlags.ToString() + ")");
                    allData.Add(space + "Background (" + obj.GetComponent<Camera>().backgroundColor.ToString() + ")");
                    allData.Add(space + "Culling Mask (" + obj.GetComponent<Camera>().cullingMask.ToString() + ")");
                    if (foundObj.name == "Main Camera") {
                        if (VB25d.cam.orthographic) {
                            allData.Add(space + "Projection (Orthographic)");
                            allData.Add(space + "Size (" + obj.GetComponent<Camera>().orthographicSize.ToString() + ")");
                        }
                        else {
                            allData.Add(space + "Projection (Perspective)");
                            allData.Add(space + "Field of View (" + obj.GetComponent<Camera>().fieldOfView.ToString() + ")");
                        }
                    }
                    else if (foundObj.name == "VBBGCamera") {
                        allData.Add(space + "Projection (Perspective)");
                        allData.Add(space + "Field of View (" + obj.GetComponent<Camera>().fieldOfView.ToString() + ")");
                    }
                    #if UNITY_2018_2_OR_NEWER
                    allData.Add(space + "Physical Camera (" + obj.GetComponent<Camera>().usePhysicalProperties.ToString() + ")");
                    if (obj.GetComponent<Camera>().usePhysicalProperties) {
                        allData.Add(space + space + "Focal Length (" + obj.GetComponent<Camera>().focalLength.ToString() + ")");
                        allData.Add(space + space + "Sensor Type (" + obj.GetComponent<Camera>().sensorSize.ToString() + ")");
                        allData.Add(space + space + "Sensor Size (" + obj.GetComponent<Camera>().lensShift.ToString() + ")");
                        allData.Add(space + space + "Lens Shift (" + obj.GetComponent<Camera>().gateFit.ToString() + ")");
                    }
                    #endif
                    allData.Add(space + "Clipping Planes (Near: " + obj.GetComponent<Camera>().nearClipPlane.ToString() + "; Far: " + obj.GetComponent<Camera>().farClipPlane.ToString() + ")");
                    allData.Add(space + "Viewport Rect (X: " + obj.GetComponent<Camera>().rect.x.ToString() + "; Y: " +
                                                               obj.GetComponent<Camera>().rect.y.ToString() + "; W: " +
                                                               obj.GetComponent<Camera>().rect.width.ToString() + "; H: " +
                                                               obj.GetComponent<Camera>().rect.height.ToString() + ")");
                    allData.Add(space + "Depth (" + obj.GetComponent<Camera>().depth.ToString() + ")");
                    allData.Add(space + "Rendering Path (" + obj.GetComponent<Camera>().renderingPath.ToString() + ")");
                    if (obj.GetComponent<Camera>().targetTexture != null) {
                        allData.Add(space + "Target Texture (" + obj.GetComponent<Camera>().targetTexture.ToString() + ")");
                    }
                    else {
                        allData.Add(space + "Target Texture (None)");
                    }
                    allData.Add(space + "Aspect Ratio (" + obj.GetComponent<Camera>().aspect.ToString() + ")");
                }
                allData.Add("");
            }
            if (obj.GetComponent<Animator>() != null) {
                return;
            }
            foreach (Transform child in obj.transform) {
                Traverse(child.gameObject);
            }
        }
    }

    public void _ExportAllData() {
        string scene = SceneManager.GetActiveScene().name;
        string path = EditorUtility.SaveFilePanel("Save all data", "Assets/", scene + " (Data)", "txt");
        if (string.IsNullOrEmpty(path)) {
            return;
        }
        allData.Clear();
        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject))) {
            if (obj.transform.parent == null) {
                Traverse(obj);
            }
        }
        using (StreamWriter writer = new StreamWriter(path, false)) {
            writer.WriteLine("");
            writer.WriteLine("=================");
            writer.WriteLine("| SCENE OBJECTS |");
            writer.WriteLine("=================");
            writer.WriteLine("");
            if (VB25d.use25dtkEnv) {
                writer.WriteLine("[Use 2.5D Toolkit Environment]");
                writer.WriteLine("Floor position (" + VB25d.vert.ToString() + ")");
                writer.WriteLine("");
            }
            for (int i = 1; i <= allData.Count - 1; i++) {
                writer.WriteLine(allData[i - 1]);
            }
            writer.WriteLine("");
            writer.WriteLine("==============");
            writer.WriteLine("| AREAS DATA |");
            writer.WriteLine("==============");
            writer.WriteLine("");
            int count = 1;
            for (int i = 1; i <= VB25d.listGroup.Count - 1; i++) {
                if (VB25d.ListOfPointLists.list != null) {
                    if (VB25d.ListOfPointLists.list[i - 1].list.Count > 0) {
                        if (VB25d.ListOfPointLists.list[i - 1].list.IndexOf(VB25d.ListOfPointLists.list[i - 1].list[0], 1) > 0) {
                            string nameArea = "[" + scene + "Area" + count.ToString() + "]";
                            writer.WriteLine(nameArea);
                            writer.WriteLine(VB25d.listIsWalkable[i - 1]);
                            for (int b = 0; b < VB25d.ListOfPointLists.list[i - 1].list.Count; b++) {
                                string v3x = VB25d.ListOfPointLists.list[i - 1].list[b].x.ToString();
                                string v3y = VB25d.ListOfPointLists.list[i - 1].list[b].y.ToString();
                                string v3z = VB25d.ListOfPointLists.list[i - 1].list[b].z.ToString();
                                writer.WriteLine(v3x + ";" + v3y + ";" + v3z);
                            }
                            writer.WriteLine("");
                            count++;
                        }
                    }
                }
            }
            writer.Flush();
            writer.Close();
            Debug.Log("2.5D Toolkit:\nAll data have been exported.");
        }
        EditorApplication.RepaintProjectWindow();
    }
}
