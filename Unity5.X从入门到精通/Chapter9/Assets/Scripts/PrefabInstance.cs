﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

using System.Collections.Generic;

//EditMode下也可以执行脚本。Unity中默认情况下，脚本只有在运行的时候才被执行，
// 加上此属性后，不运行程序，也能执行脚本。
[ExecuteInEditMode]

public class PrefabInstance : MonoBehaviour {

    public GameObject prefab;

#if UNITY_EDITOR

    public struct Thingy {
        public Mesh mesh;
        public Matrix4x4 matrix;
        public List<Material> materials;
    }

     
    [System.NonSerializedAttribute]
    public List<Thingy> things = new List<Thingy>();

    void OnValidate() {
        things.Clear();
        if (enabled) {
            Rebuild(prefab, Matrix4x4.identity);
        }
    }

    void OnEnable() {
        things.Clear();
        if (enabled) {
            Rebuild(prefab, Matrix4x4.identity);
        }
    }

    void Rebuild(GameObject source, Matrix4x4 inMatrix)
    {

        if (!source)
        {
            return;
        }

        Matrix4x4 baseMat = inMatrix * Matrix4x4.TRS(-source.transform.position, Quaternion.identity, Vector3.one);

        foreach (MeshRenderer mr in source.GetComponentsInChildren(typeof(Renderer), true)) {
            things.Add(new Thingy()
            {

                mesh = mr.GetComponent<MeshFilter>().sharedMesh,
                matrix = baseMat * mr.transform.localToWorldMatrix,
                materials = new List<Material>(mr.sharedMaterials)
            });
        }

        foreach (PrefabInstance pi in source.GetComponentsInChildren(typeof(PrefabInstance), true)) {

            if (pi.enabled && pi.gameObject.activeSelf) {
                Rebuild(pi.prefab, baseMat * pi.transform.localToWorldMatrix);
            }
        }
    }

    // Update is called once per frame
    // Editor-time-only update : Draw the meshes so we can see the objects in the scene view
    void Update() {
        if (EditorApplication.isPlaying) {
            return;
        }

        Matrix4x4 mat = transform.localToWorldMatrix;

        foreach (Thingy t in things) {
            for (int i = 0; i < t.materials.Count; i++) {
                Graphics.DrawMesh(t.mesh, mat * t.matrix, t.materials[i], gameObject.layer, null, i);
            }
        }
    }

    //Picking logic:Since we don't have gizmos.drawmesh, draw a bounding cube around each thingy
    void onDrawGizmos() {

        DrawGizmos(new Color(0, 0, 0, 0));
    }

    void selectDrawGizmos() {

        DrawGizmos(new Color(0, 0, 1, 0.2f));
    }

    void DrawGizmos(Color color) {

        if (EditorApplication.isPlaying) {
            return;
        }

        Gizmos.color = color;

        Matrix4x4 mat = transform.localToWorldMatrix;

        foreach (Thingy t in things) {

            Gizmos.matrix = mat * t.matrix;
            Gizmos.DrawCube(t.mesh.bounds.center, t.mesh.bounds.size);

        }
    }

    //Baking stuff:Copy in all the referenced objects into the scene on play or build
    [PostProcessScene(-2)]

    public static void OnPostprocessScene() {

        foreach (PrefabInstance pi in UnityEngine.Object.FindObjectsOfType(typeof(PrefabInstance))) {
            BackInstance(pi);
        }
    }

    public static void BackInstance(PrefabInstance pi) {

        if (!pi.prefab || !pi.enabled) {
            return;
        }
        pi.enabled = false;

        GameObject go = PrefabUtility.InstantiatePrefab(pi.prefab) as GameObject;

        Quaternion rot = go.transform.localRotation;
        Vector3 scale = go.transform.localScale;
        go.transform.parent = pi.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = scale;
        go.transform.localRotation = rot;
        pi.prefab = null;

        foreach (PrefabInstance childPi in go.GetComponentsInChildren<PrefabInstance>()) {
            BackInstance(childPi);
        }
    }

#endif
    
}
