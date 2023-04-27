using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using UnityEditor;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float threshold;
    List<Vector2> points;
    public bool complete = false;

    /// Create a new list of points if necessary, then add a point
    public void updateLine(Vector3 position) {
        if(points == null) {
            points = new List<Vector2>();
            addPoint(position);
        }

        if(Vector2.Distance(points.Last(), position) > threshold) {
            addPoint(position);
        }
    }

    /// Add a new point to the line
    void addPoint(Vector3 point) {
        // add to list
        points.Add(point);

        // set count to list size
        lineRenderer.positionCount = points.Count;
        // add the new point
        lineRenderer.SetPosition(points.Count - 1, point);
    }
}