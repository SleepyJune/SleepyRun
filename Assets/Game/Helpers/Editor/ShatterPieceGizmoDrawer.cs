using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

public class ShatterPieceGizmoDrawer
{
    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawGizmoForShatterPiece(ShatterPiece script, GizmoType gizmoType)
    {
        Vector3 from = script.startPosition + script.rotation * Vector3.forward * (script.height / 2);
        Vector3 to = script.startPosition - script.rotation * Vector3.forward * (script.height / 2);

        Matrix4x4 cubeTransform = Matrix4x4.TRS(script.startPosition, script.rotation, Vector3.one);

        Gizmos.matrix *= cubeTransform;
        Gizmos.DrawWireCube(script.startPosition, new Vector3(.05f, script.height, .05f));

        script.capusleMatrix = Gizmos.matrix;

        Matrix4x4 oldGizmosMatrix = Gizmos.matrix; //remember the old transform

        //Box1
        Matrix4x4 cubeTransform1 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, script.boxAngle1, 0), Vector3.one);
        Gizmos.matrix *= cubeTransform1;

        Gizmos.DrawWireCube(script.startPosition, new Vector3(script.radius, script.height, .05f));
        script.boxMatrix1 = Gizmos.matrix;

        Gizmos.matrix = oldGizmosMatrix;

        //Box2
        Matrix4x4 cubeTransform2 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, script.boxAngle2, 0), Vector3.one);
        Gizmos.matrix *= cubeTransform2;

        Gizmos.DrawWireCube(script.startPosition, new Vector3(script.radius, script.height, .05f));
        script.boxMatrix2 = Gizmos.matrix;

        Gizmos.matrix = oldGizmosMatrix;

        //Gizmos.DrawLine(from, to);
    }
}
