using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;

[ExecuteInEditMode]
[System.Serializable]

class VBNavTK {
    private VB25dTK VB25d;
    public VBNavTK(VB25dTK target) {
        VB25d = target;
    }

    public void _ResetAreasFiles(string PathDataArea) {
        string path = PathDataArea;
        if (Directory.Exists(path)) {
            Directory.Delete(path, true);
        }
        Directory.CreateDirectory(path);
        EditorApplication.RepaintProjectWindow();
    }

    public void _CreateNewArea() {
        VB25d.nGroup = VB25d.listGroup.Count;
        if (VB25d.listIsWalkable.Count == 0) {
            VB25d.listIsWalkable.Add(0);
        }
        else {
            VB25d.listIsWalkable.Add(1);
        }
        VB25d.listGroup.Add(VB25d.nGroup.ToString());
        VB25d.ResetValue();
        if (VB25d.ListOfPointLists.list == null) {
            VB25d.ListOfPointLists.list = new List<Point>();
        }
        VB25d.ListOfPointLists.list.Add(new Point());
        VB25d.ListOfPointLists.list[VB25d.nGroup - 1].list = new List<Vector3>();
    }

    public void _CreateAreasToBake() {
        GameObject newMesh;
        VB25d.RemoveMesh();
        string objName = "";
        VBPolyTK.Polygon poly = new VBPolyTK.Polygon();
        poly.perimeter.Clear();
        for (int b = 0; b < VB25d.ListOfPointLists.list[VB25d.nGroup - 1].list.Count; b++) {
            poly.perimeter.Add(VB25d.ListOfPointLists.list[VB25d.nGroup - 1].list[b]);
        }
        if (VB25d.listIsWalkable[VB25d.nGroup - 1] == 0) {
            objName = "VBNavMeshWalkable" + VB25d.nGroup.ToString();
        }
        else {
            objName = "VBNavMeshNotWalkable" + VB25d.nGroup.ToString();
        }
        VBPolyTK.CreateGameObject(poly, false, objName);
        Material mat = null;
        mat = new Material(Shader.Find("Transparent/Diffuse"));
        newMesh = GameObject.Find(objName);
        newMesh.transform.parent = VB25d.newMainObj.gameObject.transform;
        newMesh.AddComponent<MeshCollider>();
        MeshFilter newMeshFilter = newMesh.GetComponent<MeshFilter>();
        Mesh msh = newMeshFilter.sharedMesh;
        GameObjectUtility.SetStaticEditorFlags(newMesh, (StaticEditorFlags.NavigationStatic | StaticEditorFlags.OffMeshLinkGeneration));
        newMesh.GetComponent<MeshRenderer>().sharedMaterial = new Material(mat);
        if (VB25d.listIsWalkable[VB25d.nGroup - 1] == 0) {
            newMesh.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(VB25d.walkCol.r, VB25d.walkCol.g, VB25d.walkCol.b, VB25d.walkCol.a);
            msh.name = "WalkableZone";
            GameObjectUtility.SetNavMeshArea(newMesh, VB25d.listIsWalkable[VB25d.nGroup - 1]);
        }
        else if (VB25d.listIsWalkable[VB25d.nGroup - 1] == 1) {
            newMesh.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(VB25d.notWalkCol.r, VB25d.notWalkCol.g, VB25d.notWalkCol.b, VB25d.notWalkCol.a);
            msh.name = "NotWalkableZone";
            GameObjectUtility.SetNavMeshArea(newMesh, VB25d.listIsWalkable[VB25d.nGroup - 1]);
        }
        newMesh.GetComponent<MeshRenderer>().sortingOrder = 1000;
        MeshDoubleSided(msh);
        VB25d.ResetValue();
    }

    public void MeshSave(string PathSaveMesh, string NameMesh) {
        string scene = SceneManager.GetActiveScene().name;
        GameObject newMesh = GameObject.Find("VBMeshTK");
        Mesh msh = newMesh.GetComponent<MeshFilter>().sharedMesh;
        string assetpath = PathSaveMesh + NameMesh + ".asset";
        string prefabpath = PathSaveMesh + NameMesh + ".prefab";
        AssetDatabase.DeleteAsset(assetpath);
        AssetDatabase.DeleteAsset(prefabpath);
        AssetDatabase.Refresh();
        AssetDatabase.CreateAsset(msh, assetpath);
        AssetDatabase.SaveAssets();
        #if UNITY_2018_3_OR_NEWER
            PrefabUtility.SaveAsPrefabAsset(newMesh, prefabpath);
        #else
            Object prefab = PrefabUtility.CreateEmptyPrefab(prefabpath);
		    PrefabUtility.ReplacePrefab(newMesh, prefab);
        #endif
        Material mat = newMesh.GetComponent<Renderer>().sharedMaterial;
        mat.color = new Color(VB25d.meshCol.r, VB25d.meshCol.g, VB25d.meshCol.b, VB25d.meshCol.a);
        AssetDatabase.AddObjectToAsset(mat, prefabpath);
        AssetDatabase.Refresh();
        VB25d.meshName = "Missing";
        if (!VB25d.isDontDestroyOnSave) {
            VB25d.DeleteMesh();
        }
        else {
            if (GameObject.Find("VBMeshTK")) {
                GameObject renameObj = GameObject.Find("VBMeshTK");
                renameObj.name = NameMesh;
            }
        }
        Debug.Log("2.5D Toolkit:\nMesh saved as .asset and .prefab to 2.5dToolKit/Resource/[scenename] folder.");
    }

    public void _CreateMeshToExport() {
        string objName;
        GameObject newMesh;
        VB25d.DeleteMesh();
        VBPolyTK.Polygon poly = new VBPolyTK.Polygon();
        poly.perimeter.Clear();
        poly.holes.Clear();
        List<Vector3>[] listVerticesHole = new List<Vector3>[VB25d.listGroup.Count - 1];
        for (int i = 0; i < listVerticesHole.Length; i++) {
            listVerticesHole[i] = new List<Vector3>();
        }
        int listSize = VB25d.ListOfPointLists.list[0].list.Count;
        for (int b = 0; b < listSize; b++) {
            poly.perimeter.Add(VB25d.ListOfPointLists.list[0].list[b]);
        }
        for (int k = 2; k <= VB25d.ListOfPointLists.list.Count; k++) {
            objName = "VBNavMeshNotWalkable" + k.ToString();
            if (!GameObject.Find(objName)) {
                continue;
            }
            listSize = VB25d.ListOfPointLists.list[k - 1].list.Count;
            for (int b = 0; b < listSize; b++) {
                listVerticesHole[k - 1].Add(VB25d.ListOfPointLists.list[k - 1].list[b]);
            }
            poly.holes.Add(listVerticesHole[k - 1]);
        }
        VBPolyTK.CreateGameObject(poly, VB25d.isReverseMesh, "VBMeshTK");
        Material mat = null;
        mat = new Material(Shader.Find("Standard"));
        newMesh = GameObject.Find("VBMeshTK");
        newMesh.AddComponent<MeshCollider>();
        MeshFilter newMeshFilter = newMesh.GetComponent<MeshFilter>();
        Mesh msh = newMeshFilter.sharedMesh;
        GameObjectUtility.SetStaticEditorFlags(newMesh, (StaticEditorFlags.NavigationStatic | StaticEditorFlags.OffMeshLinkGeneration));
        newMesh.GetComponent<MeshRenderer>().sharedMaterial = new Material(mat);
        newMesh.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(VB25d.meshCol.r, VB25d.meshCol.g, VB25d.meshCol.b, VB25d.meshCol.a);
        string scene = SceneManager.GetActiveScene().name;
        msh.name = scene;
        if (VB25d.isDoubleSidedMesh) {
            MeshDoubleSided(msh);
        }
        Vector3[] vertices = msh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
        msh.uv = uvs;
        Debug.Log("2.5D Toolkit:\nIf mesh does not appear as drawn, check there are no overlapping edges or vertices.");
    }

    public void MeshDoubleSided(Mesh msh) {
        Vector3[] normals = msh.normals;
        int szV = msh.vertexCount;
        Vector3[] newVerts = new Vector3[szV * 2];
        Vector3[] newNorms = new Vector3[szV * 2];
        for (int j = 0; j < szV; j++) {
            newVerts[j] = newVerts[j + szV] = msh.vertices[j];
            newNorms[j] = normals[j];
            newNorms[j + szV] = -normals[j];
        }
        int[] triangles = msh.triangles;
        int szT = msh.triangles.Length;
        int[] newTris = new int[szT * 2];
        for (int i = 0; i < szT; i += 3) {
            newTris[i] = msh.triangles[i];
            newTris[i + 1] = msh.triangles[i + 1];
            newTris[i + 2] = msh.triangles[i + 2];
            int j = i + szT;
            newTris[j] = triangles[i] + szV;
            newTris[j + 2] = triangles[i + 1] + szV;
            newTris[j + 1] = triangles[i + 2] + szV;
        }
        msh.vertices = newVerts;
        msh.normals = newNorms;
        msh.triangles = newTris;
        msh.RecalculateNormals();
        msh.RecalculateBounds();
    }
}
