using UnityEngine;
using System.Collections.Generic;
using Poly2Tri;

public static class VBPolyTK {

    // incoming data.
    public class Polygon {
        public List<Vector3> perimeter;
        public List<List<Vector3>> holes;
        public List<Vector2> perimeterUVs;
        public List<List<Vector2>> holesUVs;
        public Vector3 planeNormal;
        public Quaternion rotation = Quaternion.identity;
        public Polygon() {
            perimeter = new List<Vector3>();
            holes = new List<List<Vector3>>();
        }

        // compute the plane normal.
        public void NormalPlane() {
            Vector3 v3Per = perimeter[0];
            Vector3 v3Cross = Vector3.zero;
            for (int i = 1; i < perimeter.Count - 1; i++) {
                if (perimeter[i] == v3Per || perimeter[i + 1] == v3Per) continue;
                v3Cross += Vector3.Cross(perimeter[i] - v3Per, perimeter[i + 1] - v3Per);
            }
            v3Cross.Normalize();
            planeNormal = v3Cross;
        }

        // compute the plane normal.
        public void NormalPlane(Vector3 nP) {
            planeNormal = Vector3.Cross(perimeter[1] - perimeter[0], perimeter[2] - perimeter[0]);
            planeNormal.Normalize();
            if (Vector3.Angle(planeNormal, nP) > Vector3.Angle(-planeNormal, nP)) {
                planeNormal = -planeNormal;
            }
        }

        // transfers the polygon to an XY plane.
        public void FindRotation() {
            if (planeNormal == Vector3.zero) {
                NormalPlane();
            }
            if (planeNormal == Vector3.forward) {
                rotation = Quaternion.identity;
            }
            else {
                rotation = Quaternion.FromToRotation(planeNormal, Vector3.forward);
            }
        }

        // UV
        public Vector2 UV(Vector3 pos) {
            Vector2 findUV = perimeterUVs[0];
            float findSqrM = (perimeter[0] - pos).sqrMagnitude;
            for (int i = 1; i < perimeterUVs.Count; i++) {
                float sqrM = (perimeter[i] - pos).sqrMagnitude;
                if (sqrM < findSqrM) {
                    findSqrM = sqrM;
                    findUV = perimeterUVs[i];
                }
            }
            for (int h = 0; h < holes.Count; h++) {
                List<Vector3> hole = holes[h];
                List<Vector2> holeUVs = holesUVs[h];
                for (int i = 0; i < holeUVs.Count; i++) {
                    float sqrM = (hole[i] - pos).sqrMagnitude;
                    if (sqrM < findSqrM) {
                        findSqrM = sqrM;
                        findUV = holeUVs[i];
                    }
                }
            }
            return findUV;
        }
    }

    // create gameobject wanted with Mesh, MeshFilter and MeshRenderer.
    public static GameObject CreateGameObject(Polygon polygon, bool reverseMesh, string name = "Polygon") {
        GameObject newObj = new GameObject();
        newObj.name = name;
        newObj.AddComponent(typeof(MeshRenderer));
        MeshFilter filter = newObj.AddComponent(typeof(MeshFilter)) as MeshFilter;
        filter.mesh = CreateMesh(polygon, reverseMesh);
        return newObj;
    }

    // Convert 3D points into the 2D polygon points
    static List<PolygonPoint> d3Tod2(List<Vector3> points, Quaternion rotation, Dictionary<uint, Vector3> codeToPosition) {
        int qty = points.Count;
        List<PolygonPoint> result = new List<PolygonPoint>(qty);
        for (int i = 0; i < qty; i++) {
            Vector3 prevPosition = points[i];
            Vector3 rxp = rotation * prevPosition;
            PolygonPoint xy = new PolygonPoint(rxp.x, rxp.y);
            codeToPosition[xy.VertexCode] = prevPosition;
            result.Add(xy);
        }
        return result;
    }

    // Create a Mesh from Polygon.
    public static Mesh CreateMesh(Polygon polygon, bool reverseMesh) {
        if (polygon.holes.Count == 0 && (polygon.perimeter.Count == 3
               || (polygon.perimeter.Count == 4 && polygon.perimeter[3] == polygon.perimeter[0]))) {
            return NewTri(polygon);
        }
        if (polygon.rotation == Quaternion.identity) polygon.FindRotation();
        if (polygon.planeNormal == Vector3.zero) return null;
        float z = (polygon.rotation * polygon.perimeter[0]).z;
        Dictionary<uint, Vector3> storeV3 = new Dictionary<uint, Vector3>();
        Poly2Tri.Polygon poly = new Poly2Tri.Polygon(d3Tod2(polygon.perimeter, polygon.rotation, storeV3));
        foreach (List<Vector3> hole in polygon.holes) {
            poly.AddHole(new Poly2Tri.Polygon(d3Tod2(hole, polygon.rotation, storeV3)));
        }
        try {
            DTSweepContext context = new DTSweepContext();
            context.PrepareTriangulation(poly);
            DTSweep.Triangulate(context);
            context = null;
        }
        catch (System.Exception e) {
            throw e;
        }
        Quaternion? revRotation = null;
        Dictionary<uint, int> storeInt = new Dictionary<uint, int>();
        List<Vector3> verts = new List<Vector3>();
        foreach (DelaunayTriangle t in poly.Triangles) {
            foreach (var p in t.Points) {
                if (storeInt.ContainsKey(p.VertexCode)) continue;
                storeInt[p.VertexCode] = verts.Count;
                Vector3 pos;
                if (!storeV3.TryGetValue(p.VertexCode, out pos)) {
                    if (!revRotation.HasValue) {
                        revRotation = Quaternion.Inverse(polygon.rotation);
                    }
                    pos = revRotation.Value * new Vector3(p.Xf, p.Yf, z);
                }
                verts.Add(pos);
            }
        }
        int[] indices = new int[poly.Triangles.Count * 3];
        {
            int i = 0;
            if (reverseMesh) {
                foreach (DelaunayTriangle t in poly.Triangles) {
                    indices[i++] = storeInt[t.Points[2].VertexCode];
                    indices[i++] = storeInt[t.Points[1].VertexCode];
                    indices[i++] = storeInt[t.Points[0].VertexCode];
                }
            }
            else {
                foreach (DelaunayTriangle t in poly.Triangles) {
                    indices[i++] = storeInt[t.Points[0].VertexCode];
                    indices[i++] = storeInt[t.Points[1].VertexCode];
                    indices[i++] = storeInt[t.Points[2].VertexCode];
                }
            }
        }
        // Create UV list.
        Vector2[] uv = null;
        if (polygon.perimeterUVs != null) {
            uv = new Vector2[verts.Count];
            for (int i = 0; i < verts.Count; i++) {
                uv[i] = polygon.UV(verts[i]);
            }
        }
        // Create mesh
        Mesh msh = new Mesh();
        msh.vertices = verts.ToArray();
        msh.triangles = indices;
        msh.uv = uv;
        msh.RecalculateNormals();
        msh.RecalculateBounds();
        return msh;
    }

    // create Mesh.
    public static Mesh NewTri(Polygon polygon) {
        Vector3[] vertices = new Vector3[3];
        vertices[0] = polygon.perimeter[0];
        vertices[1] = polygon.perimeter[1];
        vertices[2] = polygon.perimeter[2];
        int[] indices = new int[3] { 0, 1, 2 };
        Vector2[] uv = null;
        if (polygon.perimeterUVs != null) {
            uv = new Vector2[3];
            for (int i = 0; i < 3; i++) {
                uv[i] = polygon.UV(vertices[i]);
            }
        }
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.uv = uv;
        msh.RecalculateNormals();
        msh.RecalculateBounds();
        return msh;
    }
}
