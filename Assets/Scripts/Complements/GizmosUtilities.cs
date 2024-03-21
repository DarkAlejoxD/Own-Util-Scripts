using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UtilsComplements
{
    public static class GizmosUtilities
    {

        public static void DrawFOVCone(Transform origin, Color gizmosColor, bool isDrawing = true,
                                       float distance = 10, float fov = 40, float spaceDottedLines = 5)
        {
#if UNITY_EDITOR
            Handles.color = gizmosColor;
            DrawFOVCone(origin, isDrawing, distance, fov, spaceDottedLines);
            Handles.color = Color.white;
#endif
        }

        public static void DrawFOVCone(Transform origin, bool isDrawing = true, float distance = 10,
                                        float fov = 40, float spaceDottedLines = 5)
        {
#if UNITY_EDITOR
            if (!isDrawing)
                return;

            Vector3 direction = origin.forward;

            //Draw Cylinder
            Vector3 center = origin.position + distance * direction.normalized;
            float radius = distance * Mathf.Tan(fov * Mathf.Deg2Rad / 2);
            Handles.DrawWireDisc(center, direction, radius);

            //Draw Lines
            Vector3 startPos = origin.position;
            //Up
            Vector3 endPos = center + radius * origin.up;
            Handles.DrawDottedLine(startPos, endPos, spaceDottedLines);
            //Down
            endPos = center + radius * (-1) * origin.up;
            Handles.DrawDottedLine(startPos, endPos, spaceDottedLines);

            //Right
            endPos = center + radius * origin.right;
            Handles.DrawDottedLine(startPos, endPos, spaceDottedLines);

            //Left
            endPos = center + radius * (-1) * origin.right;
            Handles.DrawDottedLine(startPos, endPos, spaceDottedLines);
#endif
        }

        public static void DrawPath(List<Transform> list, Color gizmosColor, bool isDrawingPath = true)
        {
#if UNITY_EDITOR
            Handles.color = gizmosColor;
            DrawPath(list, isDrawingPath);
            Handles.color = Color.white;
#endif
        }

        public static void DrawPath(List<Transform> list, bool isDrawingPath = true)
        {
#if UNITY_EDITOR
            if (!isDrawingPath)
                return;

            float spaceDottedLines = 10;
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector3 start = list[i].position;
                Vector3 end = list[i + 1].position;

                Handles.DrawDottedLine(start, end, spaceDottedLines);
            }
#endif
        }

        public static void DrawCircularZone(Transform origin, float radius, Color gizmosColor,
                                            bool isDrawingZone = true, float spaceDottedLines = 5)
        {
#if UNITY_EDITOR
            Handles.color = gizmosColor;
            DrawCircularZone(origin, radius, isDrawingZone, spaceDottedLines);
            Handles.color = Color.white;
#endif
        }

        public static void DrawCircularZone(Transform origin, float radius, bool isDrawingZone = true,
                                            float spaceDottedLines = 5)
        {
#if UNITY_EDITOR
            if (!isDrawingZone)
                return;

            Handles.DrawWireDisc(origin.position, origin.up, radius);

            Vector3 start = origin.position - radius * origin.right;
            Vector3 end = origin.position + radius * origin.right;
            Handles.DrawDottedLine(start, end, spaceDottedLines);

            start = origin.position - radius * origin.forward;
            end = origin.position + radius * origin.forward;
            Handles.DrawDottedLine(start, end, spaceDottedLines);
#endif
        }

        public static void DrawSphere(Vector3 position, Color gizmosColor, bool isDrawingSphere = true)
        {
#if UNITY_EDITOR
            Gizmos.color = gizmosColor;
            DrawSphere(position, isDrawingSphere);
            Gizmos.color = Color.white;
#endif
        }

        public static void DrawSphere(Vector3 position, bool isDrawingSphere = true)
        {
#if UNITY_EDITOR
            if (!isDrawingSphere)
                return;

            float sphereRadius = 1;
            Gizmos.DrawWireSphere(position, sphereRadius);
#endif
        }

        public static void DrawSphere(Vector3 position, float radius, Color gizmosColor, bool isDrawingSphere = true)
        {
#if UNITY_EDITOR
            Gizmos.color = gizmosColor;
            DrawSphere(position, radius, isDrawingSphere);
            Gizmos.color = Color.white;
#endif
        }

        public static void DrawSphere(Vector3 position, float radius, bool isDrawingSphere = true)
        {
#if UNITY_EDITOR
            if (!isDrawingSphere)
                return;

            float sphereRadius = radius;
            Gizmos.DrawWireSphere(position, sphereRadius);
#endif
        }
    }
}