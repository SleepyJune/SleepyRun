using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

public static class HelperExtensions
{
    public static void DestroyChildren(this Transform root)
    {
        foreach (Transform transform in root.transform)
        {
            GameObject.Destroy(transform.gameObject);
        }
    }

    public static Vector3 ProjectPoint2DOnLineSegment(this Vector3 point, Vector3 start, Vector3 end)
    {
        return ProjectPointOnLineSegment(point.To2D(), start.To2D(), end.To2D());
    }

    public static Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVec, Vector3 point)
    {
        Vector3 linePointToPoint = point - linePoint;

        float t = Vector3.Dot(linePointToPoint, lineVec);

        return linePoint + lineVec * t;
    }

    public static int PointOnWhichSideOfLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
    {
        Vector3 lineVec = linePoint2 - linePoint1;
        Vector3 pointVec = point - linePoint1;

        float dot = Vector3.Dot(pointVec, lineVec);

        if (dot > 0)
        {
            if (pointVec.magnitude <= lineVec.magnitude)
            {

                return 0;
            }
            else {

                return 2;
            }
        }

        else {
            return 1;
        }
    }

    public static Vector3 ProjectPointOnLineSegment(this Vector3 point, Vector3 linePoint1, Vector3 linePoint2)
    {
        Vector3 vector = linePoint2 - linePoint1;
        Vector3 projectedPoint = ProjectPointOnLine(linePoint1, vector.normalized, point);
        int side = PointOnWhichSideOfLineSegment(linePoint1, linePoint2, projectedPoint);

        if (side == 0)
        {
            return projectedPoint;
        }

        if (side == 1)
        {
            return linePoint1;
        }

        if (side == 2)
        {
            return linePoint2;
        }

        return Vector3.zero;
    }

    public static Vector3 PointIsBetweenLineSegment(this Vector3 point, Vector3 start, Vector3 end)
    {
        return Vector3.zero;
    }

    public static float GetAngleBetween2D(this Vector3 start, Vector3 v1, Vector3 v2)
    {
        start = start.To2D();
        v1 = v1.To2D();
        v2 = v2.To2D();

        return Vector3.Angle(v1 - start, v2 - start);
    }

    public static Vector3 To2D(this Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }

    public static bool IsPointerOverUI(this Touch touch)
    {
        if (touch.fingerId >= 0)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = touch.position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results.Count > 0)
            {
                return true;
            }
            return false;
        }
        else
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }

    public static Monster GetMonster(this Collider collider)
    {
        if (collider.gameObject.layer == LayerConstants.monsterLayer)
        {
            var monster = collider.gameObject.GetComponent<Monster>();
            if (monster != null && !monster.isDead)
            {
                return monster;
            }
        }

        return null;
    }    
}