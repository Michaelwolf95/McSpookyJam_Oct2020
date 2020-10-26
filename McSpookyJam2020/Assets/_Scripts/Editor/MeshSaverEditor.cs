using System;
using UnityEditor;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public static class MeshSaverEditor 
{

    [MenuItem("CONTEXT/MeshFilter/Save Mesh...")]
    public static void SaveMeshInPlace (MenuCommand menuCommand) {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMesh(m, m.name, false, true);
    }

    [MenuItem("CONTEXT/MeshFilter/Save Mesh As New Instance...")]
    public static void SaveMeshNewInstanceItem (MenuCommand menuCommand) {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMesh(m, m.name, true, true);
    }
    
    [MenuItem("CONTEXT/Sprite/Create Mesh From Sprite...")]
    public static void GenerateMeshFromSprite (MenuCommand menuCommand) {
        Sprite sprite = menuCommand.context as Sprite;
        Mesh m = CreateMeshFromSprite(sprite);
        SaveMesh(m, m.name, true, true);
    }
    
    [MenuItem("CONTEXT/SpriteRenderer/Create Mesh From Sprite...")]
    public static void GenerateMeshFromSpriteRenderer (MenuCommand menuCommand) {
        SpriteRenderer spriteRenderer = menuCommand.context as SpriteRenderer;
        Mesh m = CreateMeshFromSprite(spriteRenderer.sprite);
        SaveMesh(m, m.name, true, true);
    }
    
    private static Mesh CreateMeshFromSprite(Sprite sprite)
    {
        
        
        List<Vector2> physicsShape = new List<Vector2>();
        sprite.GetPhysicsShape(0, physicsShape);

        Vector2[] vertices2D = physicsShape.ToArray();
        Triangulator tr = new Triangulator(vertices2D);
 
        // Create the mesh
        Mesh msh = new Mesh();
        //msh.vertices = vertices;
        msh.vertices = Array.ConvertAll(vertices2D, i => (Vector3)i);
        msh.triangles = tr.Triangulate();
        msh.RecalculateNormals();
        msh.RecalculateBounds();
        return msh;
        //Mesh mesh = new Mesh();
//        mesh.vertices = Array.ConvertAll(sprite.vertices, i => (Vector3)i);
//        mesh.uv = sprite.uv;
//        mesh.triangles = Array.ConvertAll(sprite.triangles, i => (int)i);
//        return mesh;
    }

    public static void SaveMesh (Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh) {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;
        
        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;
		
        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);
        
        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
	
}

// http://wiki.unity3d.com/index.php?title=Triangulator
public class Triangulator
{
    private List<Vector2> m_points = new List<Vector2>();
 
    public Triangulator (Vector2[] points) {
        m_points = new List<Vector2>(points);
    }
 
    public int[] Triangulate() {
        List<int> indices = new List<int>();
 
        int n = m_points.Count;
        if (n < 3)
            return indices.ToArray();
 
        int[] V = new int[n];
        if (Area() > 0) {
            for (int v = 0; v < n; v++)
                V[v] = v;
        }
        else {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }
 
        int nv = n;
        int count = 2 * nv;
        for (int v = nv - 1; nv > 2; ) {
            if ((count--) <= 0)
                return indices.ToArray();
 
            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;
 
            if (Snip(u, v, w, nv, V)) {
                int a, b, c, s, t;
                a = V[u];
                b = V[v];
                c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);
                for (s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }
 
        indices.Reverse();
        return indices.ToArray();
    }
 
    private float Area () {
        int n = m_points.Count;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++) {
            Vector2 pval = m_points[p];
            Vector2 qval = m_points[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5f);
    }
 
    private bool Snip (int u, int v, int w, int n, int[] V) {
        int p;
        Vector2 A = m_points[V[u]];
        Vector2 B = m_points[V[v]];
        Vector2 C = m_points[V[w]];
        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;
        for (p = 0; p < n; p++) {
            if ((p == u) || (p == v) || (p == w))
                continue;
            Vector2 P = m_points[V[p]];
            if (InsideTriangle(A, B, C, P))
                return false;
        }
        return true;
    }
 
    private bool InsideTriangle (Vector2 A, Vector2 B, Vector2 C, Vector2 P) {
        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
        float cCROSSap, bCROSScp, aCROSSbp;
 
        ax = C.x - B.x; ay = C.y - B.y;
        bx = A.x - C.x; by = A.y - C.y;
        cx = B.x - A.x; cy = B.y - A.y;
        apx = P.x - A.x; apy = P.y - A.y;
        bpx = P.x - B.x; bpy = P.y - B.y;
        cpx = P.x - C.x; cpy = P.y - C.y;
 
        aCROSSbp = ax * bpy - ay * bpx;
        cCROSSap = cx * apy - cy * apx;
        bCROSScp = bx * cpy - by * cpx;
 
        return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
    }
}