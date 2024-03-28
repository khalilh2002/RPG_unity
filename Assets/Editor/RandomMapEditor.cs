using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbstructMapGenerator),true)]

public class RandomMapEditor : Editor
{
    AbstructMapGenerator generator;

    private void Awake() {
        generator = (AbstructMapGenerator)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("create Map"))
        {
            generator.GeneratedMap();
        }
    }
}
