using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateGrid))]
public class GridBuilderEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GenerateGrid map = target as GenerateGrid;
        map.Generate();
    }
}
