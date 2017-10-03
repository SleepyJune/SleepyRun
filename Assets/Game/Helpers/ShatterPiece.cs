using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

public class ShatterPiece : MonoBehaviour
{
    public float height = .5f;
    public float radius = .05f;

    public float boxAngle1 = 30;
    public float boxAngle2 = -30;

    public Matrix4x4 capusleMatrix;

    public Matrix4x4 boxMatrix1;
    public Matrix4x4 boxMatrix2;

    public Quaternion rotation
    {
        get
        {
            return transform.rotation;
        }
    }

    public Vector3 startPosition {
        get
        {
            return transform.position;
        }
    }

    public void Generate()
    {
        var holder = new GameObject("Collider");
        holder.transform.SetParent(gameObject.transform, false);

        //Box1
        var box1go = new GameObject("Box1");
        box1go.transform.SetParent(holder.transform, false);

        box1go.transform.rotation = QuaternionFromMatrix(boxMatrix1);
        box1go.transform.localPosition = boxMatrix1.GetColumn(3);

        var box1col = box1go.AddComponent<BoxCollider>();
        box1col.size = new Vector3(radius, height, .05f);

        //Box2
        var box2go = new GameObject("Box2");
        box2go.transform.SetParent(holder.transform, false);

        box2go.transform.rotation = QuaternionFromMatrix(boxMatrix2);
        box2go.transform.localPosition = boxMatrix2.GetColumn(3);

        var box2col = box2go.AddComponent<BoxCollider>();
        box2col.size = new Vector3(radius, height, .05f);

        //Capsule
        var capsulego = new GameObject("Capsule");
        capsulego.transform.SetParent(holder.transform, false);

        capsulego.transform.rotation = QuaternionFromMatrix(capusleMatrix);
        capsulego.transform.localPosition = capusleMatrix.GetColumn(3);

        var capsuleCol = capsulego.AddComponent<CapsuleCollider>();
        capsuleCol.height = height;
        capsuleCol.radius = .05f;       


    }

    public static Quaternion QuaternionFromMatrix(Matrix4x4 m) { return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1)); }
}

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
        Matrix4x4 cubeTransform1 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,script.boxAngle1,0), Vector3.one);
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
