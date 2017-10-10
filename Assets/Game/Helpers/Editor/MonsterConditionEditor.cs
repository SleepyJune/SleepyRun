using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(MonsterCondition))]
public class MonsterConditionEditor : Editor
{
    MonsterCondition condition;

    public SerializedProperty conditionsProperty;

    void OnEnable()
    {
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        condition = (MonsterCondition)target;        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        serializedObject.ApplyModifiedProperties();
    }

    public static MonsterCondition CreateCondition()
    {
        // Create a new instance of the Condition.
        //MonsterCondition newCondition = CreateInstance<MonsterCondition>();

        // Set the description and the hash based on it.
        //newCondition.description = description;
        //SetHash(newCondition);
        //return newCondition;

        return null;
    }
}
