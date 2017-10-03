using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(ShatterPiece))]
public class ShatterPieceGenerator : Editor
{
    SerializedProperty heightProperty;
    SerializedProperty radiusProperty;

    SerializedProperty boxAngle1Property;
    SerializedProperty boxAngle2Property;

    private void OnEnable()
    {
        // Cache the SerializedProperties.
        heightProperty = serializedObject.FindProperty("height");
        radiusProperty = serializedObject.FindProperty("radius");

        boxAngle1Property = serializedObject.FindProperty("boxAngle1");
        boxAngle2Property = serializedObject.FindProperty("boxAngle2");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ShatterPiece shatterScript = target as ShatterPiece;

        EditorGUILayout.PropertyField(heightProperty);
        EditorGUILayout.PropertyField(radiusProperty);

        EditorGUILayout.PropertyField(boxAngle1Property);
        EditorGUILayout.PropertyField(boxAngle2Property);

        if (GUILayout.Button("Generate"))
        {
            shatterScript.Generate();
        }

        serializedObject.ApplyModifiedProperties();
    }
}